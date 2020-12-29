using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        public FrameController(AnimationEntryController animationentrycontroller,Frame frame)
        {
            AnimationEntryController = animationentrycontroller;
            Frame = frame;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.FrameController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            if (!Frame.IsNew)
            {
                ModelEntry modelentry = AnimationEntryController.EntryChunkController.NSFController.NSF.GetEntry<ModelEntry>(Frame.ModelEID);
                TextureChunk[] texturechunks = new TextureChunk[8];
                for (int i = 0; i < 8; ++i)
                {
                    texturechunks[i] = AnimationEntryController.EntryChunkController.NSFController.NSF.GetEntry<TextureChunk>(BitConv.FromInt32(modelentry.Info,0xC+i*4));
                }
                return new UndockableControl(new AnimationEntryViewer(Frame,modelentry,texturechunks));
            }
            else
            {
                return new Crash3AnimationSelector(AnimationEntryController.AnimationEntry, Frame, AnimationEntryController.EntryChunkController.NSFController.NSF);
            }
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
