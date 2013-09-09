using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSceneryEntryController : EntryController
    {
        private OldSceneryEntry oldsceneryentry;

        public OldSceneryEntryController(EntryChunkController entrychunkcontroller,OldSceneryEntry oldsceneryentry) : base(entrychunkcontroller,oldsceneryentry)
        {
            this.oldsceneryentry = oldsceneryentry;
            Node.Text = "Old Scenery Entry";
            Node.ImageKey = "oldsceneryentry";
            Node.SelectedImageKey = "oldsceneryentry";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new OldSceneryEntryViewer(oldsceneryentry));
        }

        public OldSceneryEntry OldSceneryEntry
        {
            get { return oldsceneryentry; }
        }
    }
}
