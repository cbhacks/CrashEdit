using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class NewEntryForm : Form
    {
        private const string EntryTypeUnprocessed = "Unprocessed";
        private const string EntryTypeZone = "Zone (T7 ZDAT)";
        private const string EntryTypeGOOL = "GOOL (T11 GOOL)";

        private readonly Dictionary<string, int> EntryTypes = new Dictionary<string, int>() {
            { EntryTypeUnprocessed, -1 },
            { EntryTypeZone, 7 },
            { EntryTypeGOOL, 11 }
        };

        private NSF nsf;

        public NewEntryForm(NSFController nsfc)
        {
            nsf = nsfc.NSF;
            InitializeComponent();
            dpdType.Items.Add(EntryTypeUnprocessed);
            switch (nsfc.GameVersion)
            {
                case GameVersion.Crash1BetaMAR08:
                case GameVersion.Crash1BetaMAY11:
                case GameVersion.Crash1:
                    dpdType.Items.Add(EntryTypeZone);
                    dpdType.Items.Add(EntryTypeGOOL);
                    break;
                case GameVersion.Crash2:
                case GameVersion.Crash3:
                    dpdType.Items.Add(EntryTypeGOOL);
                    break;
            }
            dpdType.SelectedIndex = 0;
            txtEID.Text = "";

            Text = Properties.Resources.NewEntryForm;
            fraName.Text = Properties.Resources.NewEntryForm_fraName;
            fraType.Text = Properties.Resources.NewEntryForm_fraType;
        }

        public int Type => EntryTypes[(string)dpdType.SelectedItem];
        public int UnprocessedType => (int)numType.Value;
        public int EID => Entry.ENameToEID(txtEID.Text);

        public void SetRenameMode(string ename)
        {
            txtEID.Text = ename;
            fraType.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numType.Enabled = (string)dpdType.SelectedItem == EntryTypeUnprocessed;
        }

        private void txtEID_TextChanged(object sender, EventArgs e)
        {
            lblEIDErr.Text = Entry.CheckEIDErrors(txtEID.Text, false, nsf);
            if (lblEIDErr.Text == string.Empty)
            {
                cmdOK.Enabled = true;
                lblEIDErr.Visible = false;
            }
            else
            {
                cmdOK.Enabled = false;
                lblEIDErr.Visible = true;
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
