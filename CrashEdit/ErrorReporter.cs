using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ErrorReporter : Form
    {
        public ErrorReporter()
        {
            InitializeComponent();
            ErrorManager.Signal += ErrorManager_Signal;
        }

        private void ErrorManager_Signal(object sender,ErrorSignalEventArgs e)
        {
            lblMessage.Text = e.Message;
            optSkip.Enabled = e.CanSkip;
            optIgnore.Enabled = e.CanIgnore;
            if (e.CanIgnore)
            {
                optIgnore.Checked = true;
            }
            else if (e.CanSkip)
            {
                optSkip.Checked = true;
            }
            else
            {
                optAbort.Checked = true;
            }
            ShowDialog();
            if (optAbort.Checked)
            {
                e.Response = ErrorResponse.Abort;
            }
            else if (optSkip.Checked)
            {
                e.Response = ErrorResponse.Skip;
            }
            else if (optIgnore.Checked)
            {
                e.Response = ErrorResponse.Ignore;
            }
            else if (optBreak.Checked)
            {
                e.Response = ErrorResponse.Break;
            }
        }

        private void cmdOK_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
