namespace HMSEditorNS {
	partial class AboutDialog {
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent() {
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.labelProductName = new System.Windows.Forms.Label();
			this.labelVersion = new System.Windows.Forms.Label();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.labelCompanyName = new System.Windows.Forms.Label();
			this.textBoxDescription = new System.Windows.Forms.TextBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.okButton = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.logo = new HMSEditorNS.Logo();
			this.labelNewVersion = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(188, 180);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(296, 20);
			this.progressBar1.TabIndex = 1;
			this.progressBar1.Visible = false;
			// 
			// labelProductName
			// 
			this.labelProductName.AutoSize = true;
			this.labelProductName.Location = new System.Drawing.Point(185, 14);
			this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelProductName.Name = "labelProductName";
			this.labelProductName.Size = new System.Drawing.Size(106, 13);
			this.labelProductName.TabIndex = 30;
			this.labelProductName.Text = "Название продукта";
			this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelVersion
			// 
			this.labelVersion.AutoSize = true;
			this.labelVersion.Location = new System.Drawing.Point(185, 36);
			this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(44, 13);
			this.labelVersion.TabIndex = 28;
			this.labelVersion.Text = "Версия";
			this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCopyright
			// 
			this.labelCopyright.AutoSize = true;
			this.labelCopyright.Location = new System.Drawing.Point(185, 80);
			this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(94, 13);
			this.labelCopyright.TabIndex = 31;
			this.labelCopyright.Text = "Авторские права";
			this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelCompanyName
			// 
			this.labelCompanyName.AutoSize = true;
			this.labelCompanyName.Location = new System.Drawing.Point(185, 58);
			this.labelCompanyName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 17);
			this.labelCompanyName.Name = "labelCompanyName";
			this.labelCompanyName.Size = new System.Drawing.Size(125, 13);
			this.labelCompanyName.TabIndex = 32;
			this.labelCompanyName.Text = "Название организации";
			this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxDescription
			// 
			this.textBoxDescription.Location = new System.Drawing.Point(188, 102);
			this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
			this.textBoxDescription.Multiline = true;
			this.textBoxDescription.Name = "textBoxDescription";
			this.textBoxDescription.ReadOnly = true;
			this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxDescription.Size = new System.Drawing.Size(296, 72);
			this.textBoxDescription.TabIndex = 33;
			this.textBoxDescription.TabStop = false;
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(185, 217);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(198, 13);
			this.linkLabel1.TabIndex = 35;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "https://github.com/WendyH/HMSEditor";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(409, 212);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 22);
			this.okButton.TabIndex = 34;
			this.okButton.Text = "&ОК";
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(12, 212);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(138, 23);
			this.btnUpdate.TabIndex = 36;
			this.btnUpdate.Text = "Обновить программу";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Visible = false;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// logo
			// 
			this.logo.BackColor = System.Drawing.Color.White;
			this.logo.BackgroundImage = global::HMSEditorNS.Properties.Resources.logo;
			this.logo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.logo.Location = new System.Drawing.Point(13, 14);
			this.logo.Name = "logo";
			this.logo.Size = new System.Drawing.Size(160, 160);
			this.logo.TabIndex = 38;
			// 
			// labelNewVersion
			// 
			this.labelNewVersion.Location = new System.Drawing.Point(13, 180);
			this.labelNewVersion.Name = "labelNewVersion";
			this.labelNewVersion.Size = new System.Drawing.Size(160, 29);
			this.labelNewVersion.TabIndex = 39;
			this.labelNewVersion.TabStop = true;
			this.labelNewVersion.Text = "Есть новая версия 1.0.0.1";
			this.labelNewVersion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			this.labelNewVersion.Visible = false;
			this.labelNewVersion.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelNewVersion_LinkClicked);
			// 
			// AboutDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(500, 244);
			this.Controls.Add(this.labelNewVersion);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.labelProductName);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.labelCompanyName);
			this.Controls.Add(this.textBoxDescription);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.logo);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutDialog";
			this.Padding = new System.Windows.Forms.Padding(9);
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "О программе";
			this.Load += new System.EventHandler(this.AboutDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label labelProductName;
		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.Label labelCompanyName;
		private System.Windows.Forms.TextBox textBoxDescription;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button btnUpdate;
		private Logo logo;
		private System.Windows.Forms.LinkLabel labelNewVersion;
	}
}
