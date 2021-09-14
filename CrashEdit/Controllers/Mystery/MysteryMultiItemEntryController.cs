using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public abstract class MysteryMultiItemEntryController : EntryController
    {
        public MysteryMultiItemEntryController(EntryChunkController entrychunkcontroller,MysteryMultiItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            MysteryEntry = mysteryentry;
        }

        public MysteryMultiItemEntry MysteryEntry { get; }
    }
}
