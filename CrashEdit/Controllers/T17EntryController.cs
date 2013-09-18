using Crash;

namespace CrashEdit
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        private T17Entry t17entry;

        public T17EntryController(EntryChunkController entrychunkcontroller,T17Entry t17entry) : base(entrychunkcontroller,t17entry)
        {
            this.t17entry = t17entry;
            Node.Text = string.Format("T17 Entry ({0})",t17entry.EIDString);
            Node.ImageKey = "t17entry";
            Node.SelectedImageKey = "t17entry";
        }

        public T17Entry T17Entry
        {
            get { return t17entry; }
        }
    }
}
