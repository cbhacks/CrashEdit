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
            Node.Text = string.Format("Old Animation Entry ({0})",oldanimationentry.EIDString);
            Node.ImageKey = "oldanimationentry";
            Node.SelectedImageKey = "oldanimationentry";
            foreach (OldFrame frame in oldanimationentry.Frames)
            {
                AddNode(new OldFrameController(this,frame));
            }
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
