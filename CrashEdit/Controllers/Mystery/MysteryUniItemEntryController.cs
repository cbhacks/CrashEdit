using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public abstract class MysteryUniItemEntryController : EntryController
    {
        public MysteryUniItemEntryController(EntryChunkController entrychunkcontroller,MysteryUniItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            MysteryEntry = mysteryentry;
        }

        protected override Control CreateEditor()
        {
            return new MysteryBox(MysteryEntry.Data);
        }

        public MysteryUniItemEntry MysteryEntry { get; }
    }
}
