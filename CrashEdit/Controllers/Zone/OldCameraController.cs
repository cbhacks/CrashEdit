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
            AddMenu("Delete Camera", Menu_Delete);
            InvalidateNode();
        }

        public OldCameraController(OldZoneEntryController oldzoneentrycontroller,OldCamera camera)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            Camera = camera;
            AddMenu("Delete Camera", Menu_Delete);
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Old Camera";
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController { get; }
        public OldZoneEntryController OldZoneEntryController { get; }
        public OldCamera Camera { get; }
        
        private void Menu_Delete()
        {
            OldZoneEntryController.OldZoneEntry.Cameras.Remove(Camera);
            OldZoneEntryController.OldZoneEntry.CameraCount = OldZoneEntryController.OldZoneEntry.Cameras.Count;
            Dispose();
        }
    }
}
