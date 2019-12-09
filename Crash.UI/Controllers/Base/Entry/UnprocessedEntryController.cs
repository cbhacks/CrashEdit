namespace Crash.UI
{
    public sealed class UnprocessedEntryController : MysteryMultiItemEntryController
    {
        public UnprocessedEntryController(EntryChunkController up,UnprocessedEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new UnprocessedEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.UnprocessedEntryController_Text,Entry.EName);
    }
}
