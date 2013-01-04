using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class InputWindow : Form
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        public string Input
        {
            get { return txtInput.Text; }
        }

        private void cmdOK_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cmdCancel_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
