using Crash;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class GameVersionForm : Form
    {
        public GameVersionForm()
        {
            InitializeComponent();
            optCrash1.CheckedChanged += opt_CheckedChanged;
            optCrash2.CheckedChanged += opt_CheckedChanged;
            optCrash3.CheckedChanged += opt_CheckedChanged;
            optCrash1MAR08.CheckedChanged += opt_CheckedChanged;
            optCrash1MAY11.CheckedChanged += opt_CheckedChanged;
            optCrash2Beta.CheckedChanged += opt_CheckedChanged;
            optNone.CheckedChanged += opt_CheckedChanged;
        }

        public GameVersion GameVersion
        {
            get
            {
                if (optCrash1.Checked)
                {
                    return GameVersion.Crash1;
                }
                else if (optCrash2.Checked)
                {
                    return GameVersion.Crash2;
                }
                else if (optCrash3.Checked)
                {
                    return GameVersion.Crash3;
                }
                else if (optCrash3.Checked)
                {
                    return GameVersion.Crash3;
                }
                else if (optCrash1MAR08.Checked)
                {
                    return GameVersion.Crash1BetaMAR08;
                }
                else if (optCrash1MAY11.Checked)
                {
                    return GameVersion.Crash1BetaMAY11;
                }
                else if (optCrash2Beta.Checked)
                {
                    return GameVersion.Crash2;
                }
                else if (optNone.Checked)
                {
                    return GameVersion.None;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void opt_CheckedChanged(object sender,EventArgs e)
        {
            cmdOK.Enabled = true;
        }

        private void cmdOK_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
