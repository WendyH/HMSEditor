/* This code is released under WTFPL Version 2 (http://www.wtfpl.net/). Created by WendyH. Copyleft. */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HMSEditorNS {
	public partial class FormMain: Form {
		string FormCaption = "HMS Editor";
		bool   noUpdate    = false;

		public FormMain() {
			InitializeComponent();
			SetTooltip(checkBoxWatchHMS, "Наблюдать за HMS", "Внедрятся в окна редактирования скриптов программы Home Media Server");
			SetTooltip(checkBoxDebug   , "Режим отладки"   , "Режим вывода дополнительной информации в окно консоли");
			FormCaption = HMSEditor.MsgCaption;
		}

		private void SetTooltip(Control cntrl, string title, string msg) {
			ToolTip tooltip = new ToolTip();
			tooltip.ToolTipIcon  = ToolTipIcon.Info;
			tooltip.ToolTipTitle = title;
			tooltip.SetToolTip(cntrl, msg);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
			if (!noUpdate) Editor.ScriptLanguage = comboBox1.Text;
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e) {
			if (Editor.Modified) {
				DialogResult answ = MessageBox.Show("Данные были изменены. Сохранить изменения?", FormCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if      (answ == DialogResult.Cancel) e.Cancel = true;
				else if (answ == DialogResult.Yes   ) Editor.SaveFile();
			}
			string section = Editor.DialogClass;
			HMSEditor.Settings.Set("WindowWidth" , this.Width      , section);
			HMSEditor.Settings.Set("WindowHeight", this.Height     , section);
			HMSEditor.Settings.Set("WindowTop"   , this.Top        , section);
			HMSEditor.Settings.Set("WindowLeft"  , this.Left       , section);
			HMSEditor.Settings.Set("WindowState" , this.WindowState, section);

			HMSEditor.Settings.Set("WatchHMS"    , checkBoxWatchHMS.Checked, section);
			Editor.SaveSettings();
		}

		private void FormMain_Load(object sender, EventArgs e) {
			string section = Editor.DialogClass;
			noUpdate = true;
			Editor.LoadSettings();
			Editor.ToolStripVisible = true;
			
			WindowState = (HMSEditor.Settings.Get("WindowState", section, "") == "Maximized") ? FormWindowState.Maximized : FormWindowState.Normal;
			if (!Editor.LoadFile(Editor.Filename)) Editor.Filename = "";

			checkBoxWatchHMS.Checked = (HMSEditor.Settings.Get("WatchHMS" , section, "0") == "1");
			checkBoxDebug   .Checked = HMSEditor.DebugMe;
			checkBoxDebug   .Visible = HMSEditor.DebugMe;

			comboBox1.Text = Editor.ScriptLanguage;
			noUpdate = false;

			UpdateCaption();
		}

		private void checkBoxWatchHMS_CheckedChanged(object sender, EventArgs e) {
			if (checkBoxWatchHMS.Checked)
				checkBoxWatchHMS.Checked = HMSEditor.WatchHMS();
			else
				HMSEditor.Exit();
		}

		private void UpdateCaption() {
			this.Text = FormCaption + (Editor.Filename.Length>0 ? " - " + Editor.Filename : "") + (Editor.Modified ? " *" : "");
		}

		private void Editor_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e) {
			UpdateCaption();
		}

		private void checkBoxDebug_CheckedChanged(object sender, EventArgs e) {
			HMSEditor.DebugMe = checkBoxDebug.Checked;
		}
	}

}
