namespace Crash.UI
{
    public sealed class OldSceneryEntryController : EntryController
    {
        private OldSceneryEntry entry;

        public OldSceneryEntryController(EntryChunkController up,OldSceneryEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldSceneryEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldSceneryEntryController_Text,entry.EName);
        }
    }
}
