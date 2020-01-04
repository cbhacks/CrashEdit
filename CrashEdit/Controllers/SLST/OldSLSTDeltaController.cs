using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldSLSTDeltaController : Controller
    {
        public OldSLSTDeltaController(OldSLSTEntryController oldslstentrycontroller,OldSLSTDelta oldslstdelta)
        {
            OldSLSTEntryController = oldslstentrycontroller;
            OldSLSTDelta = oldslstdelta;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = "Delta";
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new OldSLSTDeltaBox(OldSLSTDelta);
        }

        public OldSLSTEntryController OldSLSTEntryController { get; }
        public OldSLSTDelta OldSLSTDelta { get; }
    }
}
