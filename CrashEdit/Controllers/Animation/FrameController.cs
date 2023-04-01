using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        public FrameController(AnimationEntryController animationentrycontroller, Frame frame)
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
            return new AnimationEntryViewer(AnimationEntryController.NSF, AnimationEntryController.AnimationEntry.EID, AnimationEntryController.AnimationEntry.Frames.IndexOf(Frame));
        }

        public AnimationEntryController AnimationEntryController { get; }
        public Frame Frame { get; }
    }
}
