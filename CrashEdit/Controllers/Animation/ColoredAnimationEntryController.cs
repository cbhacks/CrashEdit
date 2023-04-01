using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ColoredAnimationEntryController : EntryController
    {
        public ColoredAnimationEntryController(EntryChunkController entrychunkcontroller, ColoredAnimationEntry coloredanimationentry)
            : base(entrychunkcontroller, coloredanimationentry)
        {
            ColoredAnimationEntry = coloredanimationentry;
            foreach (OldFrame frame in coloredanimationentry.Frames)
            {
                AddNode(new ColoredFrameController(this, frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.ColoredAnimationEntryController_Text, ColoredAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            return new UndockableControl(new OldAnimationEntryViewer(NSF, Entry.EID));
        }

        public ColoredAnimationEntry ColoredAnimationEntry { get; }
    }
}

