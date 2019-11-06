using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoCameraController : Controller
    {
        public ProtoCameraController(ProtoZoneEntryController protozoneentrycontroller,OldCamera camera)
        {
            ProtoZoneEntryController = protozoneentrycontroller;
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
            return new ProtoCameraBox(this);
        }

        public ProtoZoneEntryController ProtoZoneEntryController { get; }
        public OldCamera Camera { get; }
    }
}
