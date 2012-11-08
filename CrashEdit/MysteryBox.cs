using System.Windows.Forms;

using IO = System.IO;

namespace CrashEdit
{
    public sealed class MysteryBox : UserControl
    {
        private byte[] data;

        private ToolStrip tsToolbar;
        private ToolStripButton tbbExport;
        private HexBox hbData;

        public MysteryBox(byte[] data)
        {
            this.data = data;

            tbbExport = new ToolStripButton();
            tbbExport.Text = "Export";
            tbbExport.Click += new System.EventHandler(tbbExport_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.Items.Add(tbbExport);

            hbData = new HexBox();
            hbData.Dock = DockStyle.Fill;
            hbData.Data = data;

            Controls.Add(hbData);
            Controls.Add(tsToolbar);
        }

        void tbbExport_Click(object sender,System.EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "All Files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    IO.File.WriteAllBytes(dialog.FileName,data);
                }
            }
        }
    }
}
