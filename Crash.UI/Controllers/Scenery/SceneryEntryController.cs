namespace Crash.UI
{
    public sealed class SceneryEntryController : EntryController
    {
        private SceneryEntry entry;

        public SceneryEntryController(EntryChunkController up,SceneryEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new SceneryEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.SceneryEntryController_Text,entry.EName);
        }
    }
}
