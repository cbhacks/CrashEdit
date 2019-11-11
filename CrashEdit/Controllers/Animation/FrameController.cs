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
        }

        public override void InvalidateNode()
        {
            Node.Text = "Frame";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            ModelEntry modelentry = AnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<ModelEntry>(Frame.ModelEID);
            TextureChunk[] texturechunks = new TextureChunk[8];
            for (int i = 0; i < 8; ++i)
            {
                texturechunks[i] = AnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<TextureChunk>(BitConv.FromInt32(modelentry.Info,0xC+i*4));
            }
            return new UndockableControl(new AnimationEntryViewer(Frame,modelentry,texturechunks));
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
