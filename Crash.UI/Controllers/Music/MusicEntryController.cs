namespace Crash.UI
{
    public sealed class MusicEntryController : EntryController
    {
        public MusicEntryController(EntryChunkController up,MusicEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new MusicEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.MusicEntryController_Text,Entry.EName);
    }
}
