namespace Crash.UI
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        private MysteryMultiItemEntry entry;

        public MysteryMultiItemEntryController(EntryChunkController up,MysteryMultiItemEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new MysteryMultiItemEntry Entry
        {
            get { return entry; }
        }
    }
}
