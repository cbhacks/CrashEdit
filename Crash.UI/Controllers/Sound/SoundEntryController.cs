namespace Crash.UI
{
    public sealed class SoundEntryController : EntryController
    {
        private SoundEntry entry;

        public SoundEntryController(EntryChunkController up,SoundEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new SoundEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.SoundEntryController_Text,entry.EName);
        }
    }
}
