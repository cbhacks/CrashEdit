using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;
using Crash.Unknown4;
using Crash.Unknown5;
using System.Windows.Forms;

using IO = System.IO;

namespace CrashEdit
{
    [EditorControl(typeof(TextureChunk))]
    [EditorControl(typeof(UnknownChunk))]
    [EditorControl(typeof(T14Entry))]
    [EditorControl(typeof(T15Entry))]
    [EditorControl(typeof(DemoEntry))]
    [EditorControl(typeof(T20Entry))]
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

        public MysteryBox(TextureChunk chunk) : this(chunk.Data)
        {
        }

        public MysteryBox(UnknownChunk chunk) : this(chunk.Data)
        {
        }

        public MysteryBox(IMysteryUniItemEntry entry) : this(entry.Data)
        {
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
