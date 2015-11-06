using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace HMSEditorNS {
	public static class GitHub {
		internal static WebClient client = new WebClient();
		internal static string      giturl = "https://api.github.com/repos/";
		internal static Regex regexUpdDate = new Regex(@"""updated_at""\s*?:\s*?""(.*?)""", RegexOptions.Compiled);
		internal static Regex regexVersion = new Regex(@"""tag_name""\s*?:\s*?""(.*?)"""  , RegexOptions.Compiled);
		internal static Regex regexRelease = new Regex(@"""browser_download_url""\s*?:\s*?""([^""]+HMSEditor.exe)""", RegexOptions.Compiled);
		internal static bool  initialized  = false;
		internal static string ReleaseUrl  = "";

		public static bool IsWinVistaOrHigher() {
			OperatingSystem OS = Environment.OSVersion;
			return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
		}

		public static void Init() {
			// Без указанного UserAgent github возвращает ошибку
			client.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
			client.Headers.Set(HttpRequestHeader.Accept   , "application/vnd.github.v3+json");
			client.Encoding = System.Text.Encoding.UTF8;
			initialized = true;
        }

		public static string GetRepoUpdatedDate(string userRepo) {
			if (!initialized) Init();
			string lastDate = "";
			try {
				string jsonInfo = client.DownloadString(giturl + userRepo);
				lastDate = regexUpdDate.Match(jsonInfo).Groups[1].Value;
			} catch {
			}
			return lastDate;
        }

		public static string GetLatestReleaseVersion(string userRepo, out string updateInfo) {
			if (!initialized) Init();
			string version = "";
			updateInfo = "";
            try {
				string jsonInfo = client.DownloadString(giturl + userRepo + "/releases");
				version    = regexVersion.Match(jsonInfo).Groups[1].Value;
				ReleaseUrl = regexRelease.Match(jsonInfo).Groups[1].Value;
				MatchCollection mc = Regex.Matches(jsonInfo, @"""tag_name""\s*?:\s*?""(.*?)"".*?""body""\s*?:\s*?""(.*?[^\\])""");
				string verTag, verInfo;
                foreach (Match m in mc) {
					verTag  = m.Groups[1].Value;
					verInfo = m.Groups[2].Value;
					if (verInfo=="\"},{") continue;
					updateInfo += "v"+verTag+"\r\n-------------------------\r\n"+verInfo+"\r\n\r\n";
                }
				updateInfo = updateInfo.Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\", "");
            } catch {
			}
			return version;
		}

		public static void DownloadLegacyArchive(string userRepo, string tmpFile) {
			if (!initialized) Init();
			DownloadFile("https://codeload.github.com/" + userRepo + "/legacy.zip/master", tmpFile);
		}

		public static void DownloadFile(string dwnFile, string tmpFile) {
			if (!initialized) Init();
			try {
				client.DownloadFile(dwnFile, tmpFile);
			} catch (Exception e) {
				HMS.LogError("Ошибка загрузки файла по адресу " + dwnFile);
				HMS.LogError(e.ToString());
			}
		}

		public static event AsyncCompletedEventHandler          DownloadFileCompleted   { add { client.DownloadFileCompleted   += value; } remove { client.DownloadFileCompleted   -= value; } }
		public static event DownloadProgressChangedEventHandler DownloadProgressChanged { add { client.DownloadProgressChanged += value; } remove { client.DownloadProgressChanged -= value; } }

		public static bool DownloadLatestReleaseAsync(string tmpFile) {
			bool success = false;
			if (!initialized) Init();
			if (ReleaseUrl.Length == 0) {
                return success;
			}
			try {
				Uri uri = new Uri(ReleaseUrl);
				client.DownloadFileAsync(uri, tmpFile);
			} catch (Exception e) {
				HMS.LogError("Ошибка загрузки файла по адресу " + ReleaseUrl);
				HMS.LogError(e.ToString());
			}
			return success;
		}

		public static void DownloadFileAsync(Uri uri, string tmpFile) {
			if (!initialized) Init();
			try {
				client.DownloadFileAsync(uri, tmpFile);
			} catch (Exception e) {
				HMS.LogError("Ошибка загрузки файла по адресу " + uri.AbsolutePath);
				HMS.LogError(e.ToString());
			}
		}

		internal static Regex extractOnlyVersion = new Regex(@"[\d\.,\s]+", RegexOptions.Compiled);

		/// <summary>
		/// Compare versions of form "1,2,3,4" or "1.2.3.4". Throws FormatException
		/// in case of invalid version.
		/// </summary>
		/// <param name="strA">the first version</param>
		/// <param name="strB">the second version</param>
		/// <returns>less than zero if strA is less than strB, equal to zero if
		/// strA equals strB, and greater than zero if strA is greater than strB</returns>
		public static int CompareVersions(string strA, string strB) {
			strA = extractOnlyVersion.Match(strA).Value.Replace(" ", "").Trim();
			strB = extractOnlyVersion.Match(strB).Value.Replace(" ", "").Trim();
			if (strA.Length == 0) return -1;
			if (strB.Length == 0) return  1;
			int result = 0;
			try {
				Version vA = new Version(strA.Replace(",", "."));
				Version vB = new Version(strB.Replace(",", "."));
				return vA.CompareTo(vB);
			} catch (Exception e) {
				HMS.LogError(e.ToString());
			}
			return result;
		}

	}
}
