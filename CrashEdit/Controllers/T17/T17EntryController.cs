using Crash;

namespace CrashEdit
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        public T17EntryController(EntryChunkController entrychunkcontroller,T17Entry t17entry) : base(entrychunkcontroller,t17entry)
        {
            T17Entry = t17entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("T17 ({0})",T17Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T17Entry T17Entry { get; }
    }
}
