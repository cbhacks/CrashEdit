namespace Crash.UI
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        private UnprocessedEntry entry;

        public UnprocessedEntryController(EntryChunkController up,UnprocessedEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new UnprocessedEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.UnprocessedEntryController_Text,entry.EName);
        }
    }
}
