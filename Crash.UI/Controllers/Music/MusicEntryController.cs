namespace Crash.UI
{
    public sealed class MusicEntryController : EntryController
    {
        private MusicEntry entry;

        public MusicEntryController(EntryChunkController up,MusicEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new MusicEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.MusicEntryController_Text,entry.EName);
        }
    }
}
