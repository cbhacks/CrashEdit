namespace Crash.UI
{
    public sealed class T15EntryController : MysteryUniItemEntryController
    {
        private T15Entry entry;

        public T15EntryController(EntryChunkController up,T15Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T15Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T15EntryController_Text,entry.EName);
        }
    }
}
