using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class FrameController : Controller
    {
        private AnimationEntryController animationentrycontroller;
        private Frame frame;

        public FrameController(AnimationEntryController animationentrycontroller,Frame frame)
        {
            this.animationentrycontroller = animationentrycontroller;
            this.frame = frame;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Frame";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
            //AddMenu("Export as OBJ",Menu_Export_OBJ);
        }

        protected override Control CreateEditor()
        {
            //ModelEntry modelentry = AnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<ModelEntry>(frame.ModelEID);
            return new AnimationEntryViewer(frame/*,modelentry*/);
            //return new FrameBox(this);
        }

        public AnimationEntryController AnimationEntryController
        {
            get { return animationentrycontroller; }
        }

        public Frame Frame
        {
            get { return frame; }
        }

        /*private void Menu_Export_OBJ()
        {
            ModelEntry modelentry = AnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OModelEntry>(frame.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(frame.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }*/
    }
}
