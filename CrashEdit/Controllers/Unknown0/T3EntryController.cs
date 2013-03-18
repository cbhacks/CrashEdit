using Crash;
using Crash.Unknown0;

namespace CrashEdit
{
    public sealed class T3EntryController : MysteryMultiItemEntryController
    {
        private T3Entry t3entry;

        public T3EntryController(EntryChunkController entrychunkcontroller,T3Entry t3entry) : base(entrychunkcontroller,t3entry)
        {
            this.t3entry = t3entry;
            Node.Text = "T3 Entry";
            Node.ImageKey = "t3entry";
            Node.SelectedImageKey = "t3entry";
        }

        public T3Entry T3Entry
        {
            get { return t3entry; }
        }
    }
}
