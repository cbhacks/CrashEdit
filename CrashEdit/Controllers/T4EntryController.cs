using Crash;

namespace CrashEdit
{
    public sealed class T4EntryController : EntryController
    {
        private T4Entry t4entry;

        public T4EntryController(EntryChunkController entrychunkcontroller,T4Entry t4entry) : base(entrychunkcontroller,t4entry)
        {
            this.t4entry = t4entry;
            Node.Text = "T4 Entry";
            Node.ImageKey = "t4entry";
            Node.SelectedImageKey = "t4entry";
            foreach (T4Item t4item in t4entry.T4Items)
            {
                AddNode(new LegacyController(t4item));
            }
        }

        public T4Entry T4Entry
        {
            get { return t4entry; }
        }
    }
}
