using Crash;

namespace CrashEdit
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        private T6Entry t6entry;

        public T6EntryController(EntryChunkController entrychunkcontroller,T6Entry t6entry) : base(entrychunkcontroller,t6entry)
        {
            this.t6entry = t6entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Level Data ({0})",t6entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public T6Entry T6Entry
        {
            get { return t6entry; }
        }
    }
}
