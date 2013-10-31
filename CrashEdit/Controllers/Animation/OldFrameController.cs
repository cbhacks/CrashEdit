using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldFrameController : Controller
    {
        private OldAnimationEntryController oldanimationentrycontroller;
        private OldFrame oldframe;

        public OldFrameController(OldAnimationEntryController oldanimationentrycontroller,OldFrame oldframe)
        {
            this.oldanimationentrycontroller = oldanimationentrycontroller;
            this.oldframe = oldframe;
            Node.Text = "Frame";
            Node.ImageKey = "oldframe";
            Node.SelectedImageKey = "oldframe";
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = OldAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);
            return new OldAnimationEntryViewer(oldframe,modelentry);
        }

        public OldAnimationEntryController OldAnimationEntryController
        {
            get { return oldanimationentrycontroller; }
        }

        public OldFrame OldFrame
        {
            get { return oldframe; }
        }
    }
}
