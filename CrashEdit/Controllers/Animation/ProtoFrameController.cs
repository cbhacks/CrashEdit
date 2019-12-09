using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoFrameController : Controller
    {
        public ProtoFrameController(ProtoAnimationEntryController protoanimationentrycontroller,ProtoFrame protoframe)
        {
            ProtoAnimationEntryController = protoanimationentrycontroller;
            ProtoFrame = protoframe;
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
            OldModelEntry modelentry = ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(ProtoFrame.ModelEID);
            return new UndockableControl(new OldAnimationEntryViewer(ProtoFrame,modelentry));
        }

        public ProtoAnimationEntryController ProtoAnimationEntryController { get; }
        public ProtoFrame ProtoFrame { get; }

        private void Menu_Export_OBJ()
        {
            OldModelEntry modelentry = ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(ProtoFrame.ModelEID);
            if (modelentry == null)
            {
                throw new GUIException("The linked model entry could not be found.");
            }
            if (MessageBox.Show("Texture and color information will not be exported.\n\nContinue anyway?","Export as OBJ",MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            FileUtil.SaveFile(ProtoFrame.ToOBJ(modelentry),FileFilters.OBJ,FileFilters.Any);
        }
    }
}
