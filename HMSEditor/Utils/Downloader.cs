/* This code is released under WTFPL Version 2 (http://www.wtfpl.net/)
 * Created by WendyH. Copyleft. 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace WHUtils {
	/// <summary>
	/// Class for download files with http, https, ftp protocols.
	/// Support requests and downloading queue. By WendyH.
	/// </summary>
	public class WHDownloader {
		#region ------------ STATIC Properties and Methods ------------
		private const int BUFFER_SIZE = 1048576;

		/// <summary>
		/// Static method for just downloading the page.
		/// </summary>
		/// <param name="url">Url</param>
		/// <param name="method">Request method. Default: "GET"</param>
		/// <param name="postData">Data for POST request (optional).</param>
		/// <returns>Downloaded data as utf8 string</returns>
		public static string Load(string url, string method = "GET", string postData = "") {
			if (method == "") method = "GET";
			WHDownloader downloader = new WHDownloader();
			downloader.RequestUrl(url, method, postData);
			return downloader.ResponseText;
		}
		#endregion

		#region ------------ Properties ------------
		public string UserAgent     = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36";
		public string Referer       = "";
		public string Cookies       = "";
		public string Username      = "";
		public string Password      = "";
		public string Accept        = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
		public string ContentType   = "application/x-www-form-urlencoded";
		public string Proxy         = "";
		public string ProxyUsername = "";
		public string ProxyPassword = "";
		public bool KeepAlive = true;
		public bool NotUseProxy = false;

		public string Url = "";
		public string Method = "GET";
		public string Status = "";
		public int RetCode = 0;
		public int Threads = 1;             // Maximum threads for downloading files from queue
		public int Timeout = 1 * 60 * 1000; // 2 minutes timeout
		public string OutputDir = "";
		public WebHeaderCollection Headers = new WebHeaderCollection();
		public DownloaderQueue Queue = new DownloaderQueue();
		public byte[] ResponseData;
		/// <summary>Delete state from queue when finished</summary>
		public bool DeleteFromQueue = false;

		private CookieContainer DownloaderCookies = new CookieContainer();
		private bool _stopDownload = false;

		/// <summary>
		/// Response as Utf8 string.
		/// </summary>
		public string ResponseText { get { return ResponseData == null ? String.Empty : ASCIIEncoding.UTF8.GetString(ResponseData); } }

		/// <summary>
		/// Count of running threads to async download files in queue
		/// </summary>
		private int ThreadsRun { get { return Queue.CountBusy; } }
		#endregion

		#region Constructor
		// CONSTUCTOR
		public WHDownloader() {
			// Default headers
			Headers["Accept-Language"] = "en-us,en;q=0.5";
			Headers["Accept-Encoding"] = "gzip,deflate";
		}
		public WHDownloader(string accept) {
			// Default headers
			Headers["Accept-Language"] = "en-us,en;q=0.5";
			Headers["Accept-Encoding"] = "gzip,deflate";
			Accept = accept;
		}
		#endregion

		#region ------------ Methods ------------
		public string DownloadString(string url, string method = "GET", string postData = "") {
			if (method == "") method = "GET";
			RequestUrl(url, method, postData);
			return ResponseText;
		}

		/// <summary>
		/// Callback for timeout
		/// </summary>
		/// <param name="stateObject">State</param>
		/// <param name="timedOut">If true - time is out</param>
		private void AsyncTimeoutCallback(object stateObject, bool timedOut) {
			if (timedOut) {
				DownloadState state = stateObject as DownloadState;
				if (state != null)
					state.Abort();
				state.Status = "Aborted by timeout";
				AsyncDoneCallBack(state);
			}
		}

		/// <summary>
		/// Check the possible continous download and init state
		/// </summary>
		/// <param name="state">DownloadState object</param>
		/// <returns>Return true if the file already fownloaded</returns>
		private bool InitContinousDownload(DownloadState state) {
			bool isSupport = false, alreadyDownloaded = false;
			if (state.StreamDst.CanSeek)
				state.StreamDst.Seek(0, SeekOrigin.End);
			if (state.StreamDst.Position == 0) return false;

			if (state.UriScheme == Uri.UriSchemeFile) {
				if (state.StreamSrc.CanSeek && (state.StreamDst.Position <= state.TotalBytes))
					isSupport = (state.StreamSrc.Seek(state.StreamDst.Position, SeekOrigin.Begin) > 0);
				if (isSupport) {
					state.SkipBytes = state.StreamDst.Position;
					state.BytesRead = state.SkipBytes;
				}
			} else if ((state.UriScheme == Uri.UriSchemeHttp) || (state.UriScheme == Uri.UriSchemeHttps)) {
				HttpWebRequest request = CreateHttpRequest(state.URI, state.Method, state.PostData);
				request.Method = "HEAD";
				HttpWebResponse response = null;
				try { response = (HttpWebResponse)request.GetResponse(); } catch (WebException we) { response = we.Response as HttpWebResponse; }
				if ((response != null) && (response.Headers["Accept-Ranges"] != null))
					isSupport = (response.Headers["Accept-Ranges"].IndexOf("bytes") >= 0);
				if (isSupport) {
					state.HttpRequest.AddRange((int)state.StreamDst.Position);
					state.TotalBytes = response.ContentLength;
					state.SkipBytes = state.StreamDst.Position;
					state.BytesRead = state.SkipBytes;
				}
			} else if (state.UriScheme == Uri.UriSchemeFtp) {
				if (state.TotalBytes > 0) {
					state.SkipBytes = state.StreamDst.Position;
					state.BytesRead = state.SkipBytes;
					state.FtpRequest.ContentOffset = state.SkipBytes;
					isSupport = true;
				}
			}
			if (!isSupport) {
				state.StreamDst.Seek(0, SeekOrigin.Begin);
				state.StreamDst.SetLength(0);
			}
			if ((state.TotalBytes > 0) && (state.TotalBytes == state.BytesRead)) {
				state.Status = "Already downloaded";
				alreadyDownloaded = true;
			}
			return alreadyDownloaded;
		}

		/// <summary>
		/// Start asynchrously download with specified request state
		/// </summary>
		/// <param name="state">specified DownloadState object</param>
		private void StartDownloadAsync(DownloadState state) {
			IAsyncResult result = null;
			state.Busy = true;
			state.SkipBytes = 0;
			state.BytesRead = 0;
			state.Completed = 0;
			state.Start = DateTime.Now;

			CreateFoldersOfFilePath(state.File);
			state.StreamDst = File.OpenWrite(state.File);
			if (!state.StreamDst.CanWrite) {
				state.Status = "Can't write to output file";
				AsyncDoneCallBack(state);
				return;
			}

			if (state.UrlTouch.Length > 0) HEAD(state.UrlTouch);

			if ((state.UriScheme == Uri.UriSchemeHttp) || (state.UriScheme == Uri.UriSchemeHttps)) {
				state.HttpRequest = CreateHttpRequest(state.URI, state.Method, state.PostData);
				if (InitContinousDownload(state))
					AsyncDoneCallBack(state);
				else
					result = state.HttpRequest.BeginGetResponse(new AsyncCallback(AsyncRespCallback), state);
			} else if (state.UriScheme == Uri.UriSchemeFtp) {
				state.FtpRequest = (FtpWebRequest)FtpWebRequest.Create(state.URI);
				state.FtpRequest.Credentials = new NetworkCredential(Username, Password);
				state.FtpRequest.Method      = WebRequestMethods.Ftp.GetFileSize; // First - get size. After - download.
				state.FtpRequest.KeepAlive   = false;
				result = state.FtpRequest.BeginGetResponse(new AsyncCallback(AsyncRespCallback), state);
			} else if (state.UriScheme == Uri.UriSchemeFile) {
				FileInfo info = new FileInfo(state.Url);
				state.TotalBytes = info.Length;
				state.StreamSrc = File.OpenRead(state.Url);
				state.Status = "OK";
				if (InitContinousDownload(state))
					AsyncDoneCallBack(state);
				else
					state.StreamSrc.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallback), state);
			}

			if (result != null)
				ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(AsyncTimeoutCallback), state, Timeout, true);
		}

		/// <summary>
		/// Main response callback, invoked once we have first Response packet from server. 
		/// This is where we initiate the actual file transfer, reading from a stream.
		/// </summary>
		private void AsyncRespCallback(IAsyncResult asyncResult) {
			DownloadState state = ((DownloadState)(asyncResult.AsyncState));
			if (!state.Busy || _stopDownload) {
				state.Status = "Stopped";
				state.StopAsync(asyncResult);
				AsyncDoneCallBack(state);
				return;
			}
			try {
				if ((state.UriScheme == Uri.UriSchemeHttp) || (state.UriScheme == Uri.UriSchemeHttps)) {
					HttpWebResponse resp = ((HttpWebResponse)(state.HttpRequest.EndGetResponse(asyncResult)));
					state.HttpResponse = resp;
					state.Status = resp.StatusDescription;
					state.TotalBytes = state.HttpResponse.ContentLength + state.SkipBytes;
				} else if (state.UriScheme == Uri.UriSchemeFtp) {
					FtpWebResponse resp = ((FtpWebResponse)(state.FtpRequest.EndGetResponse(asyncResult)));
					state.FtpResponse = resp;
					state.Status = resp.StatusDescription;
					if (state.FtpRequest.Method == WebRequestMethods.Ftp.GetFileSize) {
						FtpWebRequest req2 = (FtpWebRequest)FtpWebRequest.Create(state.URI);
						req2.Credentials = state.FtpRequest.Credentials;
						req2.UseBinary = true;
						req2.KeepAlive = true;
						req2.Method = WebRequestMethods.Ftp.DownloadFile;
						state.FtpRequest = req2;
						state.TotalBytes = resp.ContentLength;
						if (InitContinousDownload(state))
							AsyncDoneCallBack(state);
						else {
							IAsyncResult result = (IAsyncResult)req2.BeginGetResponse(new AsyncCallback(AsyncRespCallback), state);
							ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(AsyncTimeoutCallback), state, Timeout, true);
						}
						return;
					}
				}
				state.StreamSrc.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallback), state);
			} catch (WebException we) {
				state.Status = we.Message;
				AsyncDoneCallBack(state);
			}

		}

		/// <summary>
		/// Main callback invoked in response to the Stream.BeginRead method, when we have some data.
		/// </summary>
		private void AsyncReadCallback(IAsyncResult asyncResult) {
			DownloadState state = ((DownloadState)(asyncResult.AsyncState));
			if (!state.Busy || _stopDownload) {
				state.Status = "Stopped";
				state.StopAsync(asyncResult);
				AsyncDoneCallBack(state);
				return;
			}

			int bytesRead = 0;

			if (state.StreamSrc.CanRead)
				bytesRead = state.StreamSrc.EndRead(asyncResult);

			if (bytesRead > 0) {
				state.BytesRead += bytesRead;
				state.StreamDst.Write(state.Buffer, 0, bytesRead);
				state.StreamDst.Flush();
				state.StreamSrc.BeginRead(state.Buffer, 0, state.Buffer.Length, new AsyncCallback(AsyncReadCallback), state);
			} else {
				AsyncDoneCallBack(state);
			}
		}

		/// <summary>
		/// Finish callback at the end of the file downloading
		/// </summary>
		/// <param name="state">DownloadState object</param>
		private void AsyncDoneCallBack(DownloadState state) {
			state.Close();
			if (DeleteFromQueue && Queue.IndexOf(state) >= 0)
				Queue.Remove(state);
			state.Busy = false;
		}

		/// <summary>
		/// Create all folders and subfolders in the file path
		/// </summary>
		/// <param name="path">Filename with path</param>
		private void CreateFoldersOfFilePath(string path) {
			try {
				string directory = "";
				foreach (string dir in Path.GetDirectoryName(path).Split(Path.DirectorySeparatorChar)) {
					directory += dir + Path.DirectorySeparatorChar;
					if (!Directory.Exists(directory))
						Directory.CreateDirectory(directory);
				}
			} catch {
			}
		}

		/// <summary>
		/// Download first free non downloaded file in the queue
		/// </summary>
		private void DownloadFromQueue() {
			for (int i = 0; i < Queue.Count; i++) {
				DownloadState state = Queue[i];
				if (state.Busy || state.Done) continue;
				state.Busy = true;
				StartDownloadAsync(state);
				break;
			}
		}

		/// <summary>
		/// Stop the thread downloading files from queue
		/// </summary>
		public void StopDownloadQueue() {
			_stopDownload = true;
		}

		/// <summary>
		/// Main thread for run threads to downloading files in quere. Control maximum running threads.
		/// </summary>
		private void MainThreadDownload() {
			while (Queue.ExistFreeForDownloadind()) {
				if (_stopDownload) break;
				if (ThreadsRun < Threads)
					DownloadFromQueue();
				Thread.Sleep(300);
			}
		}

		/// <summary>
		/// Strart downloading files from queue
		/// </summary>
		public void StartDownloadQueue() {
			if (Queue.Count < 1) return;
			for (int i = 0; i < Queue.Count; i++)
				Queue[i].Busy = false;
			_stopDownload = false;
			Thread MainThread = new Thread(MainThreadDownload);
			MainThread.IsBackground = true;
			MainThread.Start();
		}

		/// <summary>
		/// Method HEAD for request the page
		/// </summary>
		/// <param name="url">Url</param>
		/// <returns>Return the http answer code. Returned Headers in the ResponseText property.</returns>
		public int HEAD(string url) {
			return RequestUrl(url, "HEAD");
		}

		/// <summary>
		/// Method POST for downloading the page
		/// </summary>
		/// <param name="url">Url</param>
		/// <param name="postData">POST data request</param>
		/// <returns>Return the http answer code. Data in the ResponseData property.</returns>
		public int POST(string url, string postData) {
			return RequestUrl(url, "POST", postData);
		}

		/// <summary>
		/// Method GET for downloading the page
		/// </summary>
		/// <param name="url">Url</param>
		/// <returns>Return the http answer code. Data in the ResponseData property.</returns>
		public int GET(string url) {
			return RequestUrl(url, "GET");
		}

		/// <summary>
		/// Make http or https request
		/// </summary>
		/// <param name="uri">URI</param>
		/// <param name="method">Http method</param>
		/// <param name="postData">Data for POST method (optional).</param>
		/// <returns>Returns the code of result http request</returns>
		private int HttpRequestUrl(Uri uri, string method = "", string postData = "") {
			HttpWebRequest request = CreateHttpRequest(uri, method, postData);
			HttpWebResponse response = null;
			try {
				response = (HttpWebResponse)request.GetResponse();
			} catch (WebException we) {

				response = we.Response as HttpWebResponse;
				if (response == null) {
					Status = we.Message;
					RetCode = 0;
					return RetCode;
				}
			}

			Status = response.StatusDescription;
			RetCode = (int)response.StatusCode;

			Stream dataStream = response.GetResponseStream();
			string enc = response.ContentEncoding.ToLower();
			if (enc.Contains("gzip")) dataStream = new GZipStream(dataStream, CompressionMode.Decompress);
			else if (enc.Contains("deflate")) dataStream = new DeflateStream(dataStream, CompressionMode.Decompress);

			byte[] buffer = new byte[BUFFER_SIZE];
			using (MemoryStream ms = new MemoryStream()) {
				int readBytes;
				while ((readBytes = dataStream.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, readBytes);
				}
				this.ResponseData = ms.ToArray();
			}
			if (request.Method == "HEAD") this.ResponseData = response.Headers.ToByteArray();
			dataStream.Close();
			response.Close();
			return RetCode;
		}

		/// <summary>
		/// Send http request
		/// </summary>
		/// <param name="url">Url</param>
		/// <param name="postData">Data for POST method</param>
		/// <returns>Return the http answer code. Data in ResponseData property.</returns>
		private int RequestUrl(string url, string method = "", string postData = "") {
			ResponseData = null;
			if (String.IsNullOrEmpty(url)) {
				RetCode = 200;
				Status = "";
				return RetCode;
			}
			Uri uri = new Uri(url);
			if (uri.Scheme == Uri.UriSchemeFile) {
				if (File.Exists(url)) {
					ResponseData = File.ReadAllBytes(url);
					RetCode = 200;
					Status = "";
				} else {
					RetCode = 404;
					Status = "File not exist";
				}
			} else if ((uri.Scheme == Uri.UriSchemeHttp) || (uri.Scheme == Uri.UriSchemeHttps)) {
				HttpRequestUrl(uri, method, postData);
			} else {
				RetCode = 0;
				Status = "Not supported protocol";
			}
			return RetCode;
		}

		/// <summary>
		/// Creates the request object with filled properties.
		/// </summary>
		/// <param name="url">URL</param>
		/// <returns>Returns createdd new HttpWebRequest object</returns>
		private HttpWebRequest CreateHttpRequest(Uri uri, string method = "", string postData = "") {
			if (postData != "") method = "POST";
			LeaveDotsAndSlashesEscaped(uri);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			request.CookieContainer = DownloaderCookies;
			request.Method = this.Method;
			request.Accept = this.Accept;
			request.ContentType = this.ContentType;
			request.Referer = this.Referer;
			request.UserAgent = this.UserAgent;
			request.KeepAlive = this.KeepAlive;
			foreach (string key in Headers.AllKeys) {   // Some possible headers we must set through properties
				if (key == "Referer") request.Referer = Headers[key];
				else if (key == "Accept") request.Accept = Headers[key];
				else if (key == "User-Agent") request.UserAgent = Headers[key];
				else if (key == "Content-Type") request.ContentType = Headers[key];
				else request.Headers.Set(key, Headers[key]);
			}
			if (!String.IsNullOrEmpty(Cookies)) {
				if (!Cookies.TrimEnd().EndsWith(";")) Cookies += "; ";
				request.Headers.Add("Cookie: " + Cookies);
			}
			if (!String.IsNullOrEmpty(method)) request.Method = method;
			if (!String.IsNullOrEmpty(Username)) request.Credentials = new NetworkCredential(Username, Password);
			if (!String.IsNullOrEmpty(postData)) {
				using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
					sw.Write(postData);
			}
			if (!String.IsNullOrEmpty(Proxy) && !NotUseProxy) {
				if (!Proxy.StartsWith("http")) Proxy = "http://" + Proxy;
				WebProxy myProxy = new WebProxy();
				myProxy.Address = new Uri(Proxy);
				if (!String.IsNullOrEmpty(ProxyUsername))
					myProxy.Credentials = new NetworkCredential(ProxyUsername, ProxyPassword);
				request.Proxy = myProxy;
			}
			return request;
		}

		/// <summary>
		/// Add url to queue for downloading file
		/// </summary>
		/// <param name="url">Url for downloading (unique in queue)</param>
		/// <param name="file">CSVFile for saving</param>
		/// <returns>Returns the download state object in the queue</returns>
		public DownloadState Add2Queue(string url, string file = "", string touchBefore = "") {
			DownloadState state = Queue.FindByUrl(url);
			if (state != null) return state;
			if (file == "") {
				if (!String.IsNullOrEmpty(OutputDir))
					file = OutputDir + Path.DirectorySeparatorChar;
				file += Path.GetFileName(url);
			}
			return Add2Queue(new DownloadState(Queue, url, file, touchBefore));
		}

		/// <summary>
		/// Add the download state object to queue
		/// </summary>
		/// <param name="state">Download state object</param>
		/// <returns>Download state object</returns>
		public DownloadState Add2Queue(DownloadState state) {
			if (state.Queue != Queue) state.Queue = Queue;
			int i = Queue.IndexOf(state);
			if (i >= 0) return Queue[i];
			Queue.Add(state);
			return state;
		}

		/// <summary>
		/// Remove url from queue
		/// </summary>
		/// <param name="url">URL</param>
		public void RemoveFromQueue(string url) {
			RemoveFromQueue(Queue.FindByUrl(url));
		}

		/// <summary>
		/// Remove download file from queue by index
		/// </summary>
		/// <param name="index">Index of download state in queue</param>
		public void RemoveFromQueue(int index) {
			if (Queue.Count > index)
				Queue.RemoveAt(index);
		}

		/// <summary>
		/// Remove download state from queue
		/// </summary>
		/// <param name="state">DownloadState object</param>
		public void RemoveFromQueue(DownloadState state) {
			if (state != null) {
				if (Queue.IndexOf(state) >= 0)
					Queue.Remove(state);
			}
		}

		/// <summary>
		/// Convert bytes to human readable string
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns>Return string</returns>
		public string BytesToSize(long bytes) {
			string size; int i;
			if (bytes == 0) return "unknown";
			List<string> dict = new List<string> { "B", "KB", "MB", "GB", "TB" };
			for (i = 0; i < dict.Count; i++)
				if (bytes < Math.Pow(1024, i + 1)) break;
			if (i >= dict.Count) i = dict.Count - 1;
			size = Math.Round((double)bytes / Math.Pow(1024, i), 2) + " " + dict[i];
			return size;
		}

		private const int UnEscapeDotsAndSlashes = 0x2000000;
		/// <summary>
		/// Uri hack. Leave unescaped dots and slashes in url.
		/// </summary>
		/// <param name="uri"></param>
		public void LeaveDotsAndSlashesEscaped(Uri uri) {
			if (uri == null) throw new ArgumentNullException("uri");
			FieldInfo fieldInfo = uri.GetType().GetField("m_Syntax", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fieldInfo != null) {
				object uriParser = fieldInfo.GetValue(uri);
				fieldInfo = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
				if (fieldInfo != null) {
					object uriSyntaxFlags = fieldInfo.GetValue(uriParser);
					// Clear the flag that we don't want
					uriSyntaxFlags = (int)uriSyntaxFlags & ~UnEscapeDotsAndSlashes;
					fieldInfo.SetValue(uriParser, uriSyntaxFlags);
				}
			}
		}
		#endregion
	}

	#region Additional classes
	/// <summary>
	/// Class for override validation https request. Always success.
	/// </summary>
	public static class SSLValidator {
		private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain,
			SslPolicyErrors sslPolicyErrors) {
			return true;
		}
		public static void OverrideValidation() {
			ServicePointManager.ServerCertificateValidationCallback =
				OnValidateCertificate;
			ServicePointManager.Expect100Continue = true;
		}
	}

	/// <summary>
	/// Class for state object that gets passed around amongst async methods 
	/// </summary>
	public class DownloadState {
		public event EventHandler Changed = null;
		protected virtual void OnChanged(EventArgs e) { if (Changed != null) Changed(this, e); }

		public event EventHandler Finished = null;
		protected virtual void OnFinished(EventArgs e) { if (Finished != null) Finished(this, e); }

		public string UrlTouch   = "";
		public string Url        = "";
		public Uri    URI;
		public string File       = ""; // Output file
		public string Method     = ""; // Method for http request
		public string PostData   = ""; // Data for POST request
		public long   TotalBytes = 0;  // Total bytes need to read
		public long   SkipBytes  = 0;  // Skipped bytes when continous download
		public double Speed      = 0;  // Transfer rate in kb/sec
		public double Completed  = 0;  // % complete
		public string Status     = ""; // Current status string
		public byte[] Buffer     = new byte[4096];
		public DateTime Start    = DateTime.Now; // Starting download time
		public string Version    = "";    // Any additional info
		public object Tag        = null;  // Any additional info

		public bool Done { get { return ((TotalBytes > 0) && (BytesRead >= TotalBytes)); } }

		private bool _busy = false;
		public bool Busy {
			get { return _busy; }
			set {
				if (_busy != value) {
					_busy = value;
					OnChanged(EventArgs.Empty);
					Queue.StateChanged();
				}
			}
		}

		public DownloaderQueue Queue = null; // Parent queue, which consist this state object

		private long _bytesRead = 0;
		public long BytesRead {
			get { return _bytesRead; }
			set {
				_bytesRead = value;
				double totalMS = (DateTime.Now - Start).TotalMilliseconds;
				if ((TotalBytes > 0) && (totalMS > 0)) {
					// BytesRead/totalMS is in bytes/ms. Convert to kb/sec.
					Speed = ((_bytesRead - SkipBytes) * 1000.0f) / (totalMS * 1024.0f);
					Completed = ((double)_bytesRead / (double)TotalBytes) * 100.0f;
				}
				OnChanged(EventArgs.Empty);
				if (Done)
					Queue.StateChanged();
			}
		}

		public string UriScheme { get { return URI.Scheme; } }

		public HttpWebRequest  HttpRequest  { get; set; }
		public HttpWebResponse HttpResponse { get; set; }

		public FtpWebRequest   FtpRequest   { get; set; }
		public FtpWebResponse  FtpResponse  { get; set; }

		public Stream          StreamDst    { get; set; }

		private Stream _streamIn;
		public virtual Stream StreamSrc {
			get {
				if ((UriScheme == Uri.UriSchemeHttp) || (UriScheme == Uri.UriSchemeHttps))
					return HttpResponse != null ? HttpResponse.GetResponseStream() : null;
				else if (UriScheme == Uri.UriSchemeFtp)
					return FtpResponse != null ? FtpResponse.GetResponseStream() : null;
				return _streamIn;
			}
			set { _streamIn = value; }
		}

		// CONSTRUCTOR
		public DownloadState(DownloaderQueue queue, string url, string file = "", string touchBefore = "") {
			this.Queue = queue;
			this.Url = url;
			this.File = file;
			this.URI = new Uri(url);
			this.Start = DateTime.Now;
			if (File == "")
				File = Path.GetFileName(Url);
			this.BytesRead = 0;
			this.UrlTouch = touchBefore;
		}

		/// <summary>
		/// Close all open streams
		/// </summary>
		public void Close() {
			if (FtpResponse  != null) FtpResponse.Close();
			if (HttpResponse != null) HttpResponse.Close();
			if (StreamSrc    != null) StreamSrc.Close();
			if (StreamDst    != null) StreamDst.Close();
			OnFinished(EventArgs.Empty);
		}

		public void StopAsync(IAsyncResult asyncResult) {
			HttpWebResponse resp1; FtpWebResponse resp2;
			if (StreamSrc.CanRead) StreamSrc.EndRead(asyncResult);
			if (HttpRequest != null) resp1 = ((HttpWebResponse)HttpRequest.EndGetResponse(asyncResult));
			if (FtpRequest  != null) resp2 = ((FtpWebResponse)FtpRequest.EndGetResponse(asyncResult));
		}

		/// <summary>
		/// Abort the request
		/// </summary>
		public void Abort() {
			if (FtpRequest  != null) FtpRequest .Abort();
			if (HttpRequest != null) HttpRequest.Abort();
		}

		/// <summary>
		/// Wait a get size of the file
		/// </summary>
		/// <returns>Return true if size is got</returns>
		public bool WaitGetSize() {
			int maxTimeoutCount = 5 * 10; // wait max 5 sec
			for (int i = 0; i < maxTimeoutCount; i++) {
				if (TotalBytes > 0) break;
				Thread.Sleep(100);
			}
			return (TotalBytes > 0);
		}

	}

	/// <summary>
	/// Class for Downloader queue with events support
	/// </summary>
	public class DownloaderQueue: List<DownloadState> {
		/// <summary>
		/// States count of busy downloading
		/// </summary>
		public int CountBusy {
			get { int n = 0; for (int i = 0; i < base.Count; i++) if (base[i].Busy) n++; return n; }
		}

		/// <summary>
		/// States count with done
		/// </summary>
		public int CountDone {
			get { int n = 0; for (int i = 0; i < base.Count; i++) if (base[i].Done) n++; return n; }
		}

		/// <summary>
		/// Checks if exist in queue the state ready for downloading (not busy and not completed).
		/// </summary>
		/// <returns>Return true if exist state ready for downloading</returns>
		public bool ExistFreeForDownloadind() {
			for (int i = 0; i < base.Count; i++)
				if (!base[i].Busy && !base[i].Done) return true;
			return false;
		}

		/// <summary>
		/// Find state with a specific url
		/// </summary>
		/// <returns>If founded return the DownloadState object, if not return null</returns>
		public DownloadState FindByUrl(string url) {
			for (int i = 0; i < base.Count; i++)
				if (base[i].Url == url) return base[i];
			return null;
		}

		/// <summary>
		/// Find first active state
		/// </summary>
		/// <returns>If founded return the DownloadState object, if not return null</returns>
		public DownloadState FindActiveState() {
			for (int i = 0; i < base.Count; i++)
				if (base[i].Busy) return base[i];
			return null;
		}

		public EventHandler OnStateChanged = null;

		public void StateChanged() {
			if (OnStateChanged != null)
				OnStateChanged(null, EventArgs.Empty);
		}

	}
	#endregion

}