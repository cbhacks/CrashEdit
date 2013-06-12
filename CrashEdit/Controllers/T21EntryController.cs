using Crash;

namespace CrashEdit
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        private T21Entry t21entry;

        public T21EntryController(EntryChunkController entrychunkcontroller,T21Entry t21entry) : base(entrychunkcontroller,t21entry)
        {
            this.t21entry = t21entry;
            Node.Text = "T21 Entry";
            Node.ImageKey = "t21entry";
            Node.SelectedImageKey = "t21entry";
        }

        public T21Entry T21Entry
        {
            get { return t21entry; }
        }
    }
}
