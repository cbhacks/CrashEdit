using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        public T21EntryController(EntryChunkController entrychunkcontroller,T21Entry t21entry) : base(entrychunkcontroller,t21entry)
        {
            T21Entry = t21entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format("T21 ({0})",T21Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "image";
        }

        public T21Entry T21Entry { get; }
    }
}
