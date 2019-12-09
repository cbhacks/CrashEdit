namespace Crash.UI
{
    public sealed class OldMusicEntryController : EntryController
    {
        public OldMusicEntryController(EntryChunkController up,OldMusicEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldMusicEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldMusicEntryController_Text,Entry.EName);
    }
}
