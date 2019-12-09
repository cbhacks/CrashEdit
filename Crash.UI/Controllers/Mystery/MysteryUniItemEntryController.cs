namespace Crash.UI
{
    public abstract class MysteryUniItemEntryController : EntryController
    {
        public MysteryUniItemEntryController(EntryChunkController up,MysteryUniItemEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new MysteryUniItemEntry Entry { get; }
    }
}
