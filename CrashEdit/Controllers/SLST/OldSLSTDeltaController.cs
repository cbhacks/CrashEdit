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
        }

        public override void InvalidateNode()
        {
            Node.Text = "Delta";
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
