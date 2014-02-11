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
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Frame";
            Node.ImageKey = "oldframe";
            Node.SelectedImageKey = "oldframe";
            AddMenu("Export as OBJ",Menu_Export_OBJ);
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

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = OldAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(oldframe.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
