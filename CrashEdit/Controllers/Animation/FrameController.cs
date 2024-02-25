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
            if (!Frame.IsNew)
            {
                ModelEntry modelentry = GetEntry<ModelEntry>(Frame.ModelEID);
                TextureChunk[] texturechunks = new TextureChunk[8];
                for (int i = 0; i < 8; ++i)
                {
                    texturechunks[i] = GetEntry<TextureChunk>(BitConv.FromInt32(modelentry.Info, 0xC+i*4));
                }
                return new AnimationEntryViewer(Frame, modelentry, texturechunks);
            }
            else
            {
                return new Crash3AnimationSelector(AnimationEntryController.AnimationEntry, Frame, GetNSF());
            }
        }

        public AnimationEntryController AnimationEntryController => (AnimationEntryController)Modern.Parent.Legacy;
        public Frame Frame { get; }
    }
}
