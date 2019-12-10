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

        private readonly string[] EIDErrors = {
            "EID is not 5 characters long.",
            "EID has invalid characters.",
            "EID cannot be \"NONE!\"",
            "EID already exists.",
            "EID final character mismatch."
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
        }

        public int Type => EntryTypes[(string)dpdType.SelectedItem];
        public int UnprocessedType => (int)numType.Value;
        public int EID => Entry.ENameToEID(txtEID.Text);

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numType.Enabled = (string)dpdType.SelectedItem == EntryTypeUnprocessed;
        }

        private void txtEID_TextChanged(object sender, EventArgs e)
        {
            cmdOK.Enabled = false;
            lblEIDErr.Visible = true;
            if (txtEID.TextLength < 5)
            {
                lblEIDErr.Text = EIDErrors[0];
                return;
            }
            int eid = Entry.NullEID;
            try
            {
                eid = Entry.ENameToEID(txtEID.Text);
            }
            catch (ArgumentException)
            {
                lblEIDErr.Text = EIDErrors[1];
                return;
            }
            if (eid == Entry.NullEID)
            {
                lblEIDErr.Text = EIDErrors[2];
                return;
            }
            IEntry existingentry = nsf.FindEID<Entry>(eid);
            if (existingentry == null)
            {
                existingentry = nsf.FindEID<TextureChunk>(eid);
            }
            if (existingentry != null)
            {
                lblEIDErr.Text = EIDErrors[3];
                return;
            }
            cmdOK.Enabled = true;
            lblEIDErr.Visible = false;
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
