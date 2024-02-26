using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldAnimationEntry))]
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(OldAnimationEntry oldanimationentry, SubcontrollerGroup parentGroup) : base(oldanimationentry, parentGroup)
        {
            OldAnimationEntry = oldanimationentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldAnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
