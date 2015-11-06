namespace HMSEditorNS {
	partial class frmUpdateInfoDialog {
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
			this.TextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// TextBox
			// 
			this.TextBox.BulletIndent = 32;
			this.TextBox.Location = new System.Drawing.Point(13, 13);
			this.TextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.TextBox.Name = "TextBox";
			this.TextBox.ReadOnly = true;
			this.TextBox.Size = new System.Drawing.Size(669, 345);
			this.TextBox.TabIndex = 0;
			this.TextBox.Text = "";
			this.TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyDown);
			// 
			// frmUpdateInfoDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(695, 371);
			this.Controls.Add(this.TextBox);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmUpdateInfoDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Информация о новой версии программы";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmUpdateInfoDialog_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox TextBox;
	}
}