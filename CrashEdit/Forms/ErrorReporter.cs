using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ErrorReporter : Form
    {
        private IWin32Window owner = null;

        private bool lastcheckedwasskip = false;

        public ErrorReporter(IWin32Window owner)
        {
            this.owner = owner;

            InitializeComponent();
            ErrorManager.Signal += ErrorManager_Signal;

            lblTitle.Text = Properties.Resources.ErrorReporter_Title;
            optAbort.Text = Properties.Resources.ErrorReporter_Abort;
            optSkip.Text = Properties.Resources.ErrorReporter_Skip;
            optIgnore.Text = Properties.Resources.ErrorReporter_Ignore;
            optIgnoreAll.Text = Properties.Resources.ErrorReporter_IgnoreAll;
            optBreak.Text = Properties.Resources.ErrorReporter_Break;
        }

        private void ErrorManager_Signal(object sender,ErrorSignalEventArgs e)
        {
            lblMessage.Text = e.Message;
            optSkip.Enabled = e.CanSkip;
            optIgnore.Enabled = e.CanIgnore;
            optIgnoreAll.Enabled = e.CanIgnore && (e.Subject != null);
            if (lastcheckedwasskip && e.CanSkip)
            {
                optSkip.Checked = true;
            }
            else if (e.CanIgnore)
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
            ShowDialog(owner);
            if (optAbort.Checked)
            {
                e.Response = ErrorResponse.Abort;
                lastcheckedwasskip = false;
            }
            else if (optSkip.Checked)
            {
                e.Response = ErrorResponse.Skip;
                lastcheckedwasskip = true;
            }
            else if (optIgnore.Checked)
            {
                e.Response = ErrorResponse.Ignore;
                lastcheckedwasskip = false;
            }
            else if (optIgnoreAll.Checked)
            {
                e.Response = ErrorResponse.IgnoreAll;
                lastcheckedwasskip = false;
            }
            else if (optBreak.Checked)
            {
                e.Response = ErrorResponse.Break;
                lastcheckedwasskip = false;
            }
        }

        private void cmdOK_Click(object sender,EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
