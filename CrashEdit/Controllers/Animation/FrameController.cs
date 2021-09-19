using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class FrameController : LegacyController
    {
        public FrameController(AnimationEntryController animationentrycontroller,Frame frame) : base(animationentrycontroller, frame)
        {
            AnimationEntryController = animationentrycontroller;
            Frame = frame;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = CrashUI.Properties.Resources.FrameController_Text;
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
                    texturechunks[i] = GetEntry<TextureChunk>(BitConv.FromInt32(modelentry.Info,0xC+i*4));
                }
                return new AnimationEntryViewer(Frame,modelentry,texturechunks);
            }
            else
            {
                return new Crash3AnimationSelector(AnimationEntryController.AnimationEntry, Frame, GetNSF());
            }
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
