using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ColoredAnimationEntry))]
    public sealed class ColoredAnimationEntryController : EntryController
    {
        public ColoredAnimationEntryController(ColoredAnimationEntry coloredanimationentry, SubcontrollerGroup parentGroup)
            : base(coloredanimationentry, parentGroup)
        {
            ColoredAnimationEntry = coloredanimationentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldAnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public ColoredAnimationEntry ColoredAnimationEntry { get; }
    }
}

