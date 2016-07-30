using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryController : EntryController
    {
        private AnimationEntry animationentry;

        public AnimationEntryController(EntryChunkController entrychunkcontroller,AnimationEntry animationentry) : base(entrychunkcontroller,animationentry)
        {
            this.animationentry = animationentry;
            foreach (Frame frame in animationentry.Frames)
            {
                AddNode(new FrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Animation ({0})",animationentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            ModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<ModelEntry>(animationentry.Frames[0].ModelEID);
            return new AnimationEntryViewer(animationentry.Frames,modelentry);
        }

        public AnimationEntry AnimationEntry
        {
            get { return animationentry; }
        }
    }
}
