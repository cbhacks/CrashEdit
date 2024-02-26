using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(AnimationEntry))]
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(AnimationEntry animationentry, SubcontrollerGroup parentGroup) : base(animationentry, parentGroup)
        {
            AnimationEntry = animationentry;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            if (AnimationEntry.IsNew)
                return new Crash3AnimationSelector(GetNSF(), AnimationEntry);
            else
                return new AnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
