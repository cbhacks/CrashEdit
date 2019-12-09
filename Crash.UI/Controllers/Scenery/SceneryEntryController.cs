namespace Crash.UI
{
    public sealed class SceneryEntryController : EntryController
    {
        public SceneryEntryController(EntryChunkController up,SceneryEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new SceneryEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.SceneryEntryController_Text,Entry.EName);
    }
}
