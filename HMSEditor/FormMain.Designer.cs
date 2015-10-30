namespace HMSEditorNS {
	partial class FormMain {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.checkBoxWatchHMS = new System.Windows.Forms.CheckBox();
			this.checkBoxDebug = new System.Windows.Forms.CheckBox();
			this.Editor = new HMSEditorNS.HMSEditor();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "C++Script",
            "PascalScript",
            "BasicScript",
            "JScript",
            "YAML"});
			this.comboBox1.Location = new System.Drawing.Point(12, 6);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(131, 21);
			this.comboBox1.TabIndex = 3;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.checkBoxWatchHMS);
			this.panel2.Controls.Add(this.checkBoxDebug);
			this.panel2.Controls.Add(this.comboBox1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(836, 33);
			this.panel2.TabIndex = 5;
			// 
			// checkBoxWatchHMS
			// 
			this.checkBoxWatchHMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxWatchHMS.AutoSize = true;
			this.checkBoxWatchHMS.Location = new System.Drawing.Point(699, 8);
			this.checkBoxWatchHMS.Name = "checkBoxWatchHMS";
			this.checkBoxWatchHMS.Size = new System.Drawing.Size(125, 17);
			this.checkBoxWatchHMS.TabIndex = 5;
			this.checkBoxWatchHMS.Text = "Наблюдать за HMS";
			this.checkBoxWatchHMS.UseVisualStyleBackColor = true;
			this.checkBoxWatchHMS.CheckedChanged += new System.EventHandler(this.checkBoxWatchHMS_CheckedChanged);
			// 
			// checkBoxDebug
			// 
			this.checkBoxDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBoxDebug.AutoSize = true;
			this.checkBoxDebug.Location = new System.Drawing.Point(624, 8);
			this.checkBoxDebug.Name = "checkBoxDebug";
			this.checkBoxDebug.Size = new System.Drawing.Size(69, 17);
			this.checkBoxDebug.TabIndex = 4;
			this.checkBoxDebug.Text = "Отладка";
			this.checkBoxDebug.UseVisualStyleBackColor = true;
			this.checkBoxDebug.CheckedChanged += new System.EventHandler(this.checkBoxDebug_CheckedChanged);
			// 
			// Editor
			// 
			this.Editor.AutoCompleteBrackets = true;
			this.Editor.AutoIdent = true;
			this.Editor.AutoIndentChars = true;
			this.Editor.AutoIndentExistingLines = false;
			this.Editor.DebugMode = false;
			this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Editor.Location = new System.Drawing.Point(0, 33);
			this.Editor.Modified = false;
			this.Editor.Name = "Editor";
			this.Editor.ScriptLanguage = "PascalScript";
			this.Editor.SelectionStart = 0;
			this.Editor.Size = new System.Drawing.Size(836, 418);
			this.Editor.TabIndex = 0;
			this.Editor.ToolStripVisible = true;
			this.Editor.TextChangedDelayed += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.Editor_TextChangedDelayed);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(836, 451);
			this.Controls.Add(this.Editor);
			this.Controls.Add(this.panel2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormMain";
			this.Text = "HMS Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Panel panel2;
		private HMSEditor Editor;
		private System.Windows.Forms.CheckBox checkBoxDebug;
		private System.Windows.Forms.CheckBox checkBoxWatchHMS;
	}
}

