using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace HMSEditorNS {
	partial class AboutDialog: Form {
		private static AboutDialog ThisDialog = null;
		private static ProgressBar progress   = null;
		private static string tmpFile = "";
		private bool   ExistUpdate    = false;
		private System.Threading.Timer UpdateTimer = new System.Threading.Timer(UpdateTimer_Task, null, Timeout.Infinite, Timeout.Infinite);
		private string UpdateInfo     = "";

		public AboutDialog() {
			ThisDialog = this;
			InitializeComponent();
			
			tmpFile = HMS.DownloadDir + HMS.DS + "HMSEditor.exe";
			this.Text = string.Format("О программе {0}", AssemblyTitle);
			this.labelProductName.Text = AssemblyProduct;
			this.labelVersion      .Text = string.Format("Версия {0}", AssemblyVersion);
			this.labelCopyright    .Text = AssemblyCopyright;
			this.labelCompanyName  .Text = AssemblyCompany;
			this.textBoxDescription.Text = AssemblyDescription;

			progress = progressBar1;
			logo.Init();
		}

		#region Методы доступа к атрибутам сборки

		public string AssemblyTitle {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0) {
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (titleAttribute.Title != "") {
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public string AssemblyVersion {
			get {
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public string AssemblyDescription {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public string AssemblyProduct {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public string AssemblyCopyright {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public string AssemblyCompany {
			get {
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0) {
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}
		#endregion

		private void CheckUpdate(string lastVersion) {
			if (GitHub.CompareVersions(lastVersion, AssemblyVersion) > 0) {
				ExistUpdate = true;
				labelNewVersion.Text    = "Есть новая версия " + lastVersion;
				labelNewVersion.Visible = ExistUpdate;
				btnUpdate      .Visible = ExistUpdate;
			}
		}

		private static void UpdateTimer_Task(object state) {
			string info;
			string lastVersion = GitHub.GetLatestReleaseVersion(HMS.GitHubHMSEditor, out info);
			ThisDialog.Invoke((MethodInvoker)delegate {
				ThisDialog.UpdateInfo = info;
                ThisDialog.CheckUpdate(lastVersion);
			});
		}

		private void AboutDialog_Load(object sender, EventArgs e) {
			if (HMSEditor.NeedRestart) SetNeedRestart();
			UpdateTimer.Change(1, Timeout.Infinite);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(linkLabel1.Text);
		}

		private void btnUpdate_Click(object sender, EventArgs e) {
			if (HMSEditor.NeedRestart && AuthenticodeTools.IsTrusted(HMSEditor.NeedCopyNewFile)) {
				string msg = "При перезапуске программы будет возвращён встроенный редактор.\n" +
							 "После перезапуска, чтобы вернуться к данному альтернативному редактору, " +
							 "достаточно закрыть окно и открыть редактирование скриптов заного. ";
				MessageBox.Show(msg, HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
				HMSEditor.Exit();
				// waiting 3 sek, copy new file to our path and start our executable
				string rargs = "/C ping 127.0.0.1 -n 3 && Copy /Y \"" + HMSEditor.NeedCopyNewFile + "\" \"" + Application.ExecutablePath + "\" && \"" + Application.ExecutablePath + "\" "+HMS.StartArgs;
				ProcessStartInfo Info = new ProcessStartInfo();
				Info.Arguments      = rargs;
				Info.WindowStyle    = ProcessWindowStyle.Hidden;
				Info.CreateNoWindow = true;
				Info.FileName       = "cmd.exe";
				if (NeedPrivilegies()) {
					msg = "Текущая программа находится в каталоге, где нужны привилегии для записи файлов.\n" +
					      "Будет сделан запрос на ввод имени и пароля пользователя,\n" +
					      "который данными привилегиями обладает.";
					MessageBox.Show(msg, HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
					Info.Verb = "runas";
				}
				try {
					Process.Start(Info);
				} catch {
					msg = "Ошибка обновления программы.\n" +
					      "Возможно, из-за нарушения прав доступа или по какой-то другой причине.\n" +
					      "Автоматическое обновление не произошло.";
					MessageBox.Show(msg, HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
					return;
				}
				Application.Exit();
				Close();
				return;
			}
			progress.Show();

			GitHub.DownloadFileCompleted   += new AsyncCompletedEventHandler(DownloadFileCallback);
			GitHub.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);

			GitHub.DownloadLatestReleaseAsync(tmpFile);
		}

		private bool NeedPrivilegies() {
			string file = Application.ExecutablePath + ".TestPrivilegies";
            try {
				File.Create(file);
				File.Delete(file);
			} catch {
				return true;
			}
			return false;
		}

		public void SetNeedRestart() {
			labelNewVersion.Text    = "Требуется перезапуск программы";
			labelNewVersion.Visible = true;
			btnUpdate.Text          = "Перезапустить";
			btnUpdate.Visible       = true;
		}

		private static void InstallNewFile() {
			HMSEditor.NeedRestart     = true;
			HMSEditor.NeedCopyNewFile = tmpFile;
			ThisDialog.SetNeedRestart();
		}

		private static void DownloadFileCallback(object sender, AsyncCompletedEventArgs e) {
			progress.Hide();
			if (!AuthenticodeTools.IsTrusted(tmpFile)) {
				string msg = "У полученного файла не верная цифровая подпись. Обновление прервано.\n\n"+
				             "Это может означать, что произошла подмена файла или автор забыл подписать файл. "+
				             "Может быть временные проблемы с интернетом. В любом случае, можно попробовать " +
				             "посетить пару мест, где знают о существовании данной программы и спросить там:\n" +
				             "https://homemediaserver.ru/forum\nhttps://hms.lostcut.net\nhttps://github.com/WendyH/HMSEditor/issues";
				MessageBox.Show(msg, HMSEditor.MsgCaption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}
			InstallNewFile();
		}

		private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e) {
			progress.Maximum = (int)e.TotalBytesToReceive;
			progress.Value   = (int)e.BytesReceived;
		}

		private void labelNewVersion_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			if (UpdateInfo.Length == 0) return;
			frmUpdateInfoDialog form = new frmUpdateInfoDialog();
			form.Info = UpdateInfo;
			form.ShowDialog();
        }
	}
}
