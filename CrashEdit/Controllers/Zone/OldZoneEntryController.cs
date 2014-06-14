using Crash;

namespace CrashEdit
{
    public sealed class OldZoneEntryController : MysteryMultiItemEntryController
    {
        private OldZoneEntry oldzoneentry;

        public OldZoneEntryController(EntryChunkController entrychunkcontroller,OldZoneEntry oldzoneentry) : base(entrychunkcontroller,oldzoneentry)
        {
            this.oldzoneentry = oldzoneentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Zone Entry ({0})",oldzoneentry.EName);
            Node.ImageKey = "oldzoneentry";
            Node.SelectedImageKey = "oldzoneentry";
        }

        public OldZoneEntry OldZoneEntry
        {
            get { return oldzoneentry; }
        }
    }
}
