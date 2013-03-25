using Crash;
using Crash.Unknown0;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class UnknownEntryController : MysteryMultiItemEntryController
    {
        private UnknownEntry unknownentry;

        public UnknownEntryController(EntryChunkController entrychunkcontroller,UnknownEntry unknownentry) : base(entrychunkcontroller,unknownentry)
        {
            this.unknownentry = unknownentry;
            Node.Text = string.Format("Unknown Entry (T{0})",unknownentry.Type);
            Node.ImageKey = "unknownentry";
            Node.SelectedImageKey = "unknownentry";
        }

        public UnknownEntry UnknownEntry
        {
            get { return unknownentry; }
        }

        protected override Control CreateEditor()
        {
            Control tabs = base.CreateEditor();
            tabs.Dock = DockStyle.Fill;
            Label type = new Label();
            type.Dock = DockStyle.Top;
            type.AutoSize = true;
            type.Text = string.Format("Type = {0} (0x{0:X})",unknownentry.Type);
            Panel panel = new Panel();
            panel.Controls.Add(tabs);
            panel.Controls.Add(type);
            return panel;
        }
    }
}
