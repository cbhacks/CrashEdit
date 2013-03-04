using Crash;
using Crash.Audio;
using System;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(VH))]
    public sealed class VHBox : UserControl
    {
        private VH vh;

        private ToolStrip tsToolbar;
        private ToolStripButton tbbExport;

        public VHBox(VH vh)
        {
            this.vh = vh;

            tbbExport = new ToolStripButton();
            tbbExport.Text = "Export";
            tbbExport.Click += new EventHandler(tbbExport_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.Items.Add(tbbExport);

            this.Controls.Add(tsToolbar);
        }

        void tbbExport_Click(object sender,EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "VAB Header Files (*.vh)|*.vh";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    switch (dialog.FilterIndex)
                    {
                        case 1:
                            File.WriteAllBytes(dialog.FileName,vh.Save());
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }
    }
}
