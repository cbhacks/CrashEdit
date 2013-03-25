using Crash;
using Crash.Unknown0;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class WeirdEntryController : MysteryMultiItemEntryController
    {
        private WeirdEntry weirdentry;

        public WeirdEntryController(EntryChunkController entrychunkcontroller,WeirdEntry weirdentry) : base(entrychunkcontroller,weirdentry)
        {
            this.weirdentry = weirdentry;
            Node.Text = string.Format("Weird Entry (T{0})",weirdentry.Type);
            Node.ImageKey = "weirdentry";
            Node.SelectedImageKey = "weirdentry";
        }

        public WeirdEntry WeirdEntry
        {
            get { return weirdentry; }
        }

        protected override Control CreateEditor()
        {
            Control tabs = base.CreateEditor();
            tabs.Dock = DockStyle.Fill;
            Label type = new Label();
            type.Dock = DockStyle.Top;
            type.AutoSize = true;
            type.Text = string.Format("Type = {0} (0x{0:X})",weirdentry.Type);
            Panel panel = new Panel();
            panel.Controls.Add(tabs);
            panel.Controls.Add(type);
            return panel;
        }
    }
}
