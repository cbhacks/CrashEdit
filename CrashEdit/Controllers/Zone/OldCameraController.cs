using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldCameraController : Controller
    {
        private OldZoneEntryController oldzoneentrycontroller;
        private OldCamera camera;

        public OldCameraController(OldZoneEntryController oldzoneentrycontroller,OldCamera camera)
        {
            this.oldzoneentrycontroller = oldzoneentrycontroller;
            this.camera = camera;
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

        public OldZoneEntryController OldZoneEntryController
        {
            get { return oldzoneentrycontroller; }
        }

        public OldCamera Camera
        {
            get { return camera; }
        }
    }
}
