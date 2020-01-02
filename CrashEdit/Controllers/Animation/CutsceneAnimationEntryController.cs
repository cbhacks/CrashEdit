using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        public CutsceneAnimationEntryController(EntryChunkController entrychunkcontroller,CutsceneAnimationEntry cutsceneanimationentry)
            : base(entrychunkcontroller,cutsceneanimationentry)
        {
            CutsceneAnimationEntry = cutsceneanimationentry;
            foreach (OldFrame frame in cutsceneanimationentry.Frames)
            {
                AddNode(new CutsceneFrameController(this,frame));
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Cutscene Animation ({0})",CutsceneAnimationEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "limeb";
            Node.SelectedImageKey = "limeb";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(CutsceneAnimationEntry.Frames[0].ModelEID);
            return new UndockableControl(new OldAnimationEntryViewer(CutsceneAnimationEntry.Frames,modelentry));
        }

        public CutsceneAnimationEntry CutsceneAnimationEntry { get; }
    }
}

