using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        private AnimationEntryController animationentrycontroller;
        private Frame frame;

        public FrameController(AnimationEntryController animationentrycontroller,Frame frame)
        {
            this.animationentrycontroller = animationentrycontroller;
            this.frame = frame;
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
            ModelEntry modelentry = AnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<ModelEntry>(frame.ModelEID);
            return new AnimationEntryViewer(frame, modelentry);
        }

        public AnimationEntryController AnimationEntryController
        {
            get { return animationentrycontroller; }
        }

        public Frame Frame
        {
            get { return frame; }
        }
    }
}
