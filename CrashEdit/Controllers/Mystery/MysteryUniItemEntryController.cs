using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public abstract class MysteryUniItemEntryController : EntryController
    {
        public MysteryUniItemEntryController(EntryChunkController entrychunkcontroller,MysteryUniItemEntry mysteryentry) : base(entrychunkcontroller,mysteryentry)
        {
            MysteryEntry = mysteryentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new MysteryBox(MysteryEntry.Data);
        }

        public MysteryUniItemEntry MysteryEntry { get; }
    }
}
