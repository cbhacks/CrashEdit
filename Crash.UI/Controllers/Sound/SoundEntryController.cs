namespace Crash.UI
{
    public sealed class SoundEntryController : EntryController
    {
        public SoundEntryController(EntryChunkController up,SoundEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new SoundEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.SoundEntryController_Text,Entry.EName);
    }
}
