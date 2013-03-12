using Crash;
using Crash.Unknown0;

namespace CrashEdit
{
    public sealed class T2EntryController : MysteryMultiItemEntryController
    {
        private T2Entry t2entry;

        public T2EntryController(EntryChunkController entrychunkcontroller,T2Entry t2entry) : base(entrychunkcontroller,t2entry)
        {
            this.t2entry = t2entry;
            Node.Text = "T2 Entry";
            Node.ImageKey = "t2entry";
            Node.SelectedImageKey = "t2entry";
        }

        public T2Entry T2Entry
        {
            get { return t2entry; }
        }
    }
}
