namespace Crash.UI
{
    public abstract class MysteryUniItemEntryController : EntryController
    {
        private MysteryUniItemEntry entry;

        public MysteryUniItemEntryController(EntryChunkController up,MysteryUniItemEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new MysteryUniItemEntry Entry
        {
            get { return entry; }
        }
    }
}
