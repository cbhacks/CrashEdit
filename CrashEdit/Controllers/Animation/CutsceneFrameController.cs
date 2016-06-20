using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class CutsceneFrameController : Controller
    {
        private CutsceneAnimationEntryController cutsceneanimationentrycontroller;
        private OldFrame oldframe;

        public CutsceneFrameController(CutsceneAnimationEntryController cutsceneanimationentrycontroller,OldFrame oldframe)
        {
            this.cutsceneanimationentrycontroller = cutsceneanimationentrycontroller;
            this.oldframe = oldframe;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Frame";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
            AddMenu("Export as OBJ",Menu_Export_OBJ);
        }

        protected override Control CreateEditor()
        {
            OldModelEntry modelentry = CutsceneAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);
            return new OldAnimationEntryViewer(oldframe,modelentry);
        }

        public CutsceneAnimationEntryController CutsceneAnimationEntryController
        {
            get { return cutsceneanimationentrycontroller; }
        }

        public OldFrame OldFrame
        {
            get { return oldframe; }
        }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = CutsceneAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(oldframe.ModelEID);
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
