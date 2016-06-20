using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldAnimationEntryController : EntryController
    {
        private OldAnimationEntry oldanimationentry;

        public OldAnimationEntryController(EntryChunkController entrychunkcontroller,OldAnimationEntry oldanimationentry) : base(entrychunkcontroller,oldanimationentry)
        {
            this.oldanimationentry = oldanimationentry;
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Animation ({0})",oldanimationentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldanimationentry.Frames[0].ModelEID);
            return new OldAnimationEntryViewer(oldanimationentry.Frames,modelentry);
        }

        public OldAnimationEntry OldAnimationEntry
        {
            get { return oldanimationentry; }
        }
    }
}
