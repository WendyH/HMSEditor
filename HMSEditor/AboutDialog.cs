using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace HMSEditorNS {
	partial class AboutDialog: Form {
		private static AboutDialog ThisDialog = null;
		private static ProgressBar progress   = null;
		private static string tmpFile = "";
		private bool   ExistUpdate    = false;
		private Timer  timer          = new Timer();

		public AboutDialog() {
			ThisDialog = this;
			InitializeComponent();
			
			tmpFile = HMS.getDownloadFolderPath() + "\\HMSEditor.exe";
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

		private void CheckUpdate() {
			string lastVersion = GitHub.GetLatestReleaseVersion(HMS.GitHubHMSEditor);

			if (GitHub.CompareVersions(lastVersion, AssemblyVersion) > 0) {
				ExistUpdate = true;
				labelNewVersion.Text    = "Есть новая версия " + lastVersion;
				labelNewVersion.Visible = ExistUpdate;
				btnUpdate      .Visible = ExistUpdate;
			}
		}

		private void AboutBox1_Load(object sender, EventArgs e) {
			timer.Tick += Timer_Tick;
			timer.Interval = 1;
			if (HMSEditor.NeedRestart) SetNeedRestart();
			else timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs e) {
			timer.Stop();
			CheckUpdate();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start(linkLabel1.Text);
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
				Process.Start(Info);
				Application.Exit();
				Close();
				return;
			}

			progress.Show();

			GitHub.DownloadFileCompleted   += new AsyncCompletedEventHandler(DownloadFileCallback);
			GitHub.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);

			GitHub.DownloadLatestReleaseAsync(tmpFile);
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

	}
}
