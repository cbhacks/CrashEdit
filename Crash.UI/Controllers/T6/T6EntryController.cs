namespace Crash.UI
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        private T6Entry entry;

        public T6EntryController(EntryChunkController up,T6Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T6Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T6EntryController_Text,entry.EName);
        }
    }
}
