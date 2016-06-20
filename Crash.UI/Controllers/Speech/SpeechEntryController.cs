namespace Crash.UI
{
    public sealed class SpeechEntryController : EntryController
    {
        private SpeechEntry entry;

        public SpeechEntryController(EntryChunkController up,SpeechEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new SpeechEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.SpeechEntryController_Text,entry.EName);
        }
    }
}
