using Crash;

namespace CrashEdit
{
    public sealed class T1EntryController : MysteryMultiItemEntryController
    {
        private T1Entry t1entry;

        public T1EntryController(EntryChunkController entrychunkcontroller,T1Entry t1entry) : base(entrychunkcontroller,t1entry)
        {
            this.t1entry = t1entry;
            Node.Text = "T1 Entry";
            Node.ImageKey = "t1entry";
            Node.SelectedImageKey = "t1entry";
        }

        public T1Entry T1Entry
        {
            get { return t1entry; }
        }
    }
}
