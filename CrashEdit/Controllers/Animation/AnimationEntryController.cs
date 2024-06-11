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
            return new AnimationEntryViewer(GetNSF(), Entry.EID);
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
