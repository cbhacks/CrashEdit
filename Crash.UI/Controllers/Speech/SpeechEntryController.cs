namespace Crash.UI
{
    public sealed class SpeechEntryController : EntryController
    {
        public SpeechEntryController(EntryChunkController up,SpeechEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new SpeechEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.SpeechEntryController_Text,Entry.EName);
    }
}
