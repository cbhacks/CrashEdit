using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldCameraController : Controller
    {
        public OldCameraController(ProtoZoneEntryController protozoneentrycontroller,OldCamera camera)
        {
            ProtoZoneEntryController = protozoneentrycontroller;
            Camera = camera;
            AddMenu("Delete Camera",Menu_DeleteProto);
            InvalidateNodeImage();
            InvalidateNode();
        }

        public OldCameraController(OldZoneEntryController oldzoneentrycontroller,OldCamera camera)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            Camera = camera;
            AddMenu("Delete Camera",Menu_DeleteOld);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.OldCameraController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController { get; }
        public ProtoZoneEntry ProtoZoneEntry => ProtoZoneEntryController.ProtoZoneEntry;
        public OldZoneEntryController OldZoneEntryController { get; }
        public OldZoneEntry OldZoneEntry => OldZoneEntryController.OldZoneEntry;
        public OldCamera Camera { get; }
        
        private void Menu_DeleteProto()
        {
            ProtoZoneEntry.Cameras.Remove(Camera);
            ProtoZoneEntry.CameraCount = ProtoZoneEntry.Cameras.Count;
            Dispose();
        }
        
        private void Menu_DeleteOld()
        {
            OldZoneEntry.Cameras.Remove(Camera);
            Dispose();
        }
    }
}
