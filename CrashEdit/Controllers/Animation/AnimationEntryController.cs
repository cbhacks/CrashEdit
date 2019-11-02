using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(EntryChunkController entrychunkcontroller,AnimationEntry animationentry) : base(entrychunkcontroller,animationentry)
        {
            AnimationEntry = animationentry;
            foreach (Frame frame in animationentry.Frames)
            {
                AddNode(new FrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Animation ({0})",AnimationEntry.EName);
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            ModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<ModelEntry>(AnimationEntry.Frames[0].ModelEID);
            return new AnimationEntryViewer(AnimationEntry.Frames,modelentry);
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
