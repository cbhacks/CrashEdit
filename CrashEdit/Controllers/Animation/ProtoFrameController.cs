using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoFrameController : Controller
    {
        public ProtoFrameController(ProtoAnimationEntryController protoanimationentrycontroller,ProtoFrame protoframe)
        {
            ProtoAnimationEntryController = protoanimationentrycontroller;
            ProtoFrame = protoframe;
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
            OldModelEntry modelentry = ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<OldModelEntry>(ProtoFrame.ModelEID);
            Dictionary<int,TextureChunk> textures = new Dictionary<int,TextureChunk>();
            foreach (OldModelStruct str in modelentry.Structs)
                if (str is OldModelTexture tex && !textures.ContainsKey(tex.EID))
                    textures.Add(tex.EID,ProtoAnimationEntryController.EntryChunkController.NSFController.NSF.FindEID<TextureChunk>(tex.EID));
            return new UndockableControl(new OldAnimationEntryViewer(ProtoFrame,modelentry,textures));
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
