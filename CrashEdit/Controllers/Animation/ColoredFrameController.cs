using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ColoredFrameController : Controller
    {
        public ColoredFrameController(ColoredAnimationEntryController coloranimationentrycontroller,OldFrame oldframe)
        {
            ColorAnimationEntryController = coloranimationentrycontroller;
            OldFrame = oldframe;
            AddMenu("Export as OBJ", Menu_Export_OBJ);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.FrameController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldAnimationEntryViewer(ColorAnimationEntryController.NSF, ColorAnimationEntryController.Entry.EID, ColorAnimationEntryController.ColoredAnimationEntry.Frames.IndexOf(OldFrame), true);
        }

        public ColoredAnimationEntryController ColorAnimationEntryController { get; }
        public OldFrame OldFrame { get; }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = ColorAnimationEntryController.EntryChunkController.NSFController.NSF.GetEntry<OldModelEntry>(OldFrame.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(OldFrame.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
