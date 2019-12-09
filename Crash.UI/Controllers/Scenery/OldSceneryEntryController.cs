namespace Crash.UI
{
    public sealed class OldSceneryEntryController : EntryController
    {
        public OldSceneryEntryController(EntryChunkController up,OldSceneryEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldSceneryEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldSceneryEntryController_Text,Entry.EName);
    }
}
