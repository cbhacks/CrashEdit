namespace Crash.UI
{
    public sealed class T18EntryController : MysteryMultiItemEntryController
    {
        private T18Entry entry;

        public T18EntryController(EntryChunkController up,T18Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T18Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T18EntryController_Text,entry.EName);
        }
    }
}
