
using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoFrameController : Controller
    {
        private ProtoAnimationEntryController protoanimationentrycontroller;
        private ProtoFrame protoframe;

        public ProtoFrameController(ProtoAnimationEntryController protoanimationentrycontroller,ProtoFrame protoframe)
        {
            this.protoanimationentrycontroller = protoanimationentrycontroller;
            this.protoframe = protoframe;
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
            OldModelEntry modelentry = ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(protoframe.ModelEID);
            return new ProtoAnimationEntryViewer(protoframe,modelentry);
            //return new ProtoFrameBox(this);
        }

        public ProtoAnimationEntryController ProtoAnimationEntryController
        {
            get { return protoanimationentrycontroller; }
        }

        public ProtoFrame ProtoFrame
        {
            get { return protoframe; }
        }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(protoframe.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(protoframe.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
