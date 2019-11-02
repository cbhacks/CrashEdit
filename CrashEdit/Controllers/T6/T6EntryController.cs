using Crash;

namespace CrashEdit
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        public T6EntryController(EntryChunkController entrychunkcontroller,T6Entry t6entry) : base(entrychunkcontroller,t6entry)
        {
            T6Entry = t6entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Level Data ({0})",T6Entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T6Entry T6Entry { get; }
    }
}
