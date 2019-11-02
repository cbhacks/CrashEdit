using Crash;

namespace CrashEdit
{
    public sealed class OldT17EntryController : MysteryMultiItemEntryController
    {
        public OldT17EntryController(EntryChunkController entrychunkcontroller,OldT17Entry oldt17entry) : base(entrychunkcontroller,oldt17entry)
        {
            OldT17Entry = oldt17entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old T17 ({0})",OldT17Entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldT17Entry OldT17Entry { get; }
    }
}
