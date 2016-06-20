namespace Crash.UI
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        private T21Entry entry;

        public T21EntryController(EntryChunkController up,T21Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T21Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T21EntryController_Text,entry.EName);
        }
    }
}
