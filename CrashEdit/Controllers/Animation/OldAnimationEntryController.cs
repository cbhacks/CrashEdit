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
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Animation ({0})",OldAnimationEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(OldAnimationEntry.Frames[0].ModelEID);
            return new OldAnimationEntryViewer(OldAnimationEntry.Frames,modelentry);
        }

        public OldAnimationEntry OldAnimationEntry { get; }
    }
}
