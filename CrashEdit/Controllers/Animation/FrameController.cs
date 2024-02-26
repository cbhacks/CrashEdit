using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(Frame))]
    public sealed class FrameController : LegacyController
    {
        public FrameController(Frame frame, SubcontrollerGroup parentGroup) : base(parentGroup, frame)
        {
            Frame = frame;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            if (AnimationEntryController.AnimationEntry.IsNew)
                return new Crash3AnimationSelector(GetNSF(), AnimationEntryController.AnimationEntry, Frame);
            else
                return new AnimationEntryViewer(GetNSF(), AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame));
        }

        public AnimationEntryController AnimationEntryController => (AnimationEntryController)Modern.Parent.Legacy;
        public Frame Frame { get; }
    }
}
