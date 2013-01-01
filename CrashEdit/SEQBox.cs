using Crash;
using Crash.Audio;
using System.Windows.Forms;

using IO = System.IO;

namespace CrashEdit
{
    public sealed class SEQBox : UserControl
    {
        private SEQ seq;

        private ToolStrip tsToolbar;
        private ToolStripButton tbbExport;

        public SEQBox(SEQ seq)
        {
            this.seq = seq;

            tbbExport = new ToolStripButton();
            tbbExport.Text = "Export";
            tbbExport.Click += new System.EventHandler(tbbExport_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.Items.Add(tbbExport);

            this.Controls.Add(tsToolbar);
        }

        void tbbExport_Click(object sender,System.EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PS1 Sequence Files (*.seq)|*.seq|All Files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    IO.File.WriteAllBytes(dialog.FileName,seq.Save());
                }
            }
        }
    }
}
