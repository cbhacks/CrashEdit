using CrashEdit.Crash;

namespace CrashEdit.CrashUI
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        public MysteryMultiItemEntryController(EntryChunkController up,MysteryMultiItemEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new MysteryMultiItemEntry Entry { get; }
    }
}
