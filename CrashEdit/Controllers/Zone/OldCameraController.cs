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
            InvalidateNode();
        }

        public OldCameraController(OldZoneEntryController oldzoneentrycontroller,OldCamera camera)
        {
            OldZoneEntryController = oldzoneentrycontroller;
            Camera = camera;
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
    }
}
