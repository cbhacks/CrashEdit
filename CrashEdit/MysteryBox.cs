using Crash;
using Crash.Game;
using Crash.Graphics;
using Crash.Audio;
using Crash.Unknown0;
using System;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    [EditorControl(typeof(UnknownChunk))]
    [EditorControl(typeof(T15Entry))]
    [EditorControl(typeof(DemoEntry))]
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
            tbbExport.Click += new EventHandler(tbbExport_Click);

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

        public MysteryBox(MysteryUniItemEntry entry) : this(entry.Data)
        {
        }

        void tbbExport_Click(object sender,EventArgs e)
        {
            FileUtil.SaveFile(data,FileUtil.AnyFilter);
        }
    }
}
