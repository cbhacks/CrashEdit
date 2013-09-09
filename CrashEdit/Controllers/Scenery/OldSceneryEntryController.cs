using Crash;

namespace CrashEdit
{
    public sealed class OldSceneryEntryController : MysteryMultiItemEntryController
    {
        private OldSceneryEntry oldsceneryentry;

        public OldSceneryEntryController(EntryChunkController entrychunkcontroller,OldSceneryEntry oldsceneryentry) : base(entrychunkcontroller,oldsceneryentry)
        {
            this.oldsceneryentry = oldsceneryentry;
            Node.Text = "Old Scenery Entry";
            Node.ImageKey = "oldsceneryentry";
            Node.SelectedImageKey = "oldsceneryentry";
        }

        public OldSceneryEntry OldSceneryEntry
        {
            get { return oldsceneryentry; }
        }
    }
}
