using Crash;

namespace CrashEdit
{
    public sealed class OldT17EntryController : MysteryMultiItemEntryController
    {
        private OldT17Entry oldt17entry;

        public OldT17EntryController(EntryChunkController entrychunkcontroller,OldT17Entry oldt17entry) : base(entrychunkcontroller,oldt17entry)
        {
            this.oldt17entry = oldt17entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old T17 ({0})",oldt17entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldT17Entry OldT17Entry
        {
            get { return oldt17entry; }
        }
    }
}
