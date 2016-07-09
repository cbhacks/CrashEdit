using System;
using System.Windows.Forms;

namespace Crash.UI
{
    public partial class GameVersionForm : Form
    {
        private GameVersion version;

        public GameVersionForm()
        {
            this.version = GameVersion.None;
            InitializeComponent();
            this.Text = Properties.Resources.GameVersionForm_Text;
            lblMessage.Text = Properties.Resources.GameVersionForm_Message;
            fraRelease.Text = Properties.Resources.GameVersionForm_Release;
            fraPrerelease.Text = Properties.Resources.GameVersionForm_Prerelease;
            cmdCancel.Text = Properties.Resources.Cancel;
        }

        public GameVersion SelectedVersion
        {
            get { return version; }
        }

        private void cmdCrash1_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash1;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash2_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash2;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash3_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash3;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash1Beta1995_Click(object sender, EventArgs e)
        {
            version = GameVersion.Crash1Beta1995;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash1BetaMAR08_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash1BetaMAR08;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash1BetaMAY11_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash1BetaMAY11;
            DialogResult = DialogResult.OK;
        }

        private void cmdCrash2Beta_Click(object sender,EventArgs e)
        {
            version = GameVersion.Crash2;
            DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}