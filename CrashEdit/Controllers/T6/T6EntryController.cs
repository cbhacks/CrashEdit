using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        public T6EntryController(EntryChunkController entrychunkcontroller,T6Entry t6entry) : base(entrychunkcontroller,t6entry)
        {
            T6Entry = t6entry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format("T6 ({0})",T6Entry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "thing";
        }

        public T6Entry T6Entry { get; }
    }
}
