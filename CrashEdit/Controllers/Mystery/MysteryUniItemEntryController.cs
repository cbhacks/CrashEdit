using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(MysteryUniItemEntry))]
    public class MysteryUniItemEntryController : EntryController
    {
        public MysteryUniItemEntryController(MysteryUniItemEntry mysteryentry, SubcontrollerGroup parentGroup) : base(mysteryentry, parentGroup)
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
