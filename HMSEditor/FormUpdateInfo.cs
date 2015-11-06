using System.Windows.Forms;

namespace HMSEditorNS {
	public partial class frmUpdateInfoDialog: Form {

		public string Info { get { return TextBox.Text; } set { TextBox.Text = value; } }

		public frmUpdateInfoDialog() {
			InitializeComponent();
		}

		private void frmUpdateInfoDialog_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) this.Close();
		}

		private void TextBox_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) this.Close();
		}
	}
}
