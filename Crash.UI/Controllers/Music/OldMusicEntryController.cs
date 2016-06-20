namespace Crash.UI
{
    public sealed class OldMusicEntryController : EntryController
    {
        private OldMusicEntry entry;

        public OldMusicEntryController(EntryChunkController up,OldMusicEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldMusicEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldMusicEntryController_Text,entry.EName);
        }
    }
}
