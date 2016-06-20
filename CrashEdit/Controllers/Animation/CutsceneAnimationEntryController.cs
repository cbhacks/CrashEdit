using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class CutsceneAnimationEntryController : EntryController
    {
        private CutsceneAnimationEntry cutsceneanimationentry;

        public CutsceneAnimationEntryController(EntryChunkController entrychunkcontroller,CutsceneAnimationEntry cutsceneanimationentry)
            : base(entrychunkcontroller,cutsceneanimationentry)
        {
            this.cutsceneanimationentry = cutsceneanimationentry;
            foreach (OldFrame frame in cutsceneanimationentry.Frames)
            {
                AddNode(new CutsceneFrameController(this,frame));
            }
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Cutscene Animation ({0})",cutsceneanimationentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(cutsceneanimationentry.Frames[0].ModelEID);
            return new OldAnimationEntryViewer(cutsceneanimationentry.Frames,modelentry);
        }

        public CutsceneAnimationEntry CutsceneAnimationEntry
        {
            get { return cutsceneanimationentry; }
        }
    }
}

