namespace Crash.UI
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        private T17Entry entry;

        public T17EntryController(EntryChunkController up,T17Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T17Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T17EntryController_Text,entry.EName);
        }
    }
}
