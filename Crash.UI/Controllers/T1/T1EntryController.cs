namespace Crash.UI
{
    public sealed class T1EntryController : MysteryMultiItemEntryController
    {
        private T1Entry entry;

        public T1EntryController(EntryChunkController up,T1Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T1Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T1EntryController_Text,entry.EName);
        }
    }
}
