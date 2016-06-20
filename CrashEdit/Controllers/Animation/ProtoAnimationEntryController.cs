using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoAnimationEntryController : EntryController
    {
        private ProtoAnimationEntry protoanimationentry;

        public ProtoAnimationEntryController(EntryChunkController entrychunkcontroller,ProtoAnimationEntry protoanimationentry) : base(entrychunkcontroller,protoanimationentry)
        {
            this.protoanimationentry = protoanimationentry;
            foreach (ProtoFrame frame in protoanimationentry.Frames)
            {
                AddNode(new ProtoFrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Proto Animation ({0})",protoanimationentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(protoanimationentry.Frames[0].ModelEID);
            return new ProtoAnimationEntryViewer(protoanimationentry.Frames,modelentry);
        }

        public ProtoAnimationEntry ProtoAnimationEntry
        {
            get { return protoanimationentry; }
        }
    }
}
