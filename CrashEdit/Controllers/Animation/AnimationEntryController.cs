using Crash;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class AnimationEntryController : EntryController
    {
        public AnimationEntryController(EntryChunkController entrychunkcontroller, AnimationEntry animationentry) : base(entrychunkcontroller, animationentry)
        {
            AnimationEntry = animationentry;
            foreach (Frame frame in animationentry.Frames)
            {
                AddNode(new FrameController(this, frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.AnimationEntryController_Text, AnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            if (AnimationEntry.IsNew)
                return new Crash3AnimationSelector(NSF, AnimationEntry);
            else
                return new UndockableControl(new AnimationEntryViewer(NSF, Entry.EID));
        }

        public AnimationEntry AnimationEntry { get; }
    }
}
