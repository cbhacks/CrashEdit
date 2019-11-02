using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoAnimationEntryController : EntryController
    {
        public ProtoAnimationEntryController(EntryChunkController entrychunkcontroller,ProtoAnimationEntry protoanimationentry) : base(entrychunkcontroller,protoanimationentry)
        {
            ProtoAnimationEntry = protoanimationentry;
            foreach (ProtoFrame frame in protoanimationentry.Frames)
            {
                AddNode(new ProtoFrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Proto Animation ({0})",ProtoAnimationEntry.EName);
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(ProtoAnimationEntry.Frames[0].ModelEID);
            return new ProtoAnimationEntryViewer(ProtoAnimationEntry.Frames,modelentry);
        }

        public ProtoAnimationEntry ProtoAnimationEntry { get; }
    }
}
