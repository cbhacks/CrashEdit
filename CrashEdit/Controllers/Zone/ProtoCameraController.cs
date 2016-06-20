using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoCameraController : Controller
    {
        private ProtoZoneEntryController protozoneentrycontroller;
        private OldCamera camera;

        public ProtoCameraController(ProtoZoneEntryController protozoneentrycontroller,OldCamera camera)
        {
            this.protozoneentrycontroller = protozoneentrycontroller;
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
            return new ProtoCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController
        {
            get { return protozoneentrycontroller; }
        }

        public OldCamera Camera
        {
            get { return camera; }
        }
    }
}
