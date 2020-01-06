using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryController : EntryController
    {
        public OldAnimationEntryController(EntryChunkController entrychunkcontroller,OldAnimationEntry oldanimationentry) : base(entrychunkcontroller,oldanimationentry)
        {
            OldAnimationEntry = oldanimationentry;
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this,frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldAnimationEntryController_Text,OldAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(OldAnimationEntry.Frames[0].ModelEID);
            return new UndockableControl(new OldAnimationEntryViewer(OldAnimationEntry.Frames,modelentry));
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
