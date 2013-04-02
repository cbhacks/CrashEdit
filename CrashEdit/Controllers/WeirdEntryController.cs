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

        protected override Control CreateEditor()
        {
            TextBox box = new TextBox();
            box.ReadOnly = true;
            box.Multiline = true;
            box.Text = weirdentry.Exception.ToString();
            return box;
        }

        public WeirdEntry WeirdEntry
        {
            get { return weirdentry; }
        }
    }
}
