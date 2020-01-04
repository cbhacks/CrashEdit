using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class SLSTDeltaController : Controller
    {
        public SLSTDeltaController(SLSTEntryController slstentrycontroller,SLSTDelta slstdelta)
        {
            SLSTEntryController = slstentrycontroller;
            SLSTDelta = slstdelta;
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
            return new SLSTDeltaBox(SLSTDelta);
        }

        public SLSTEntryController SLSTEntryController { get; }
        public SLSTDelta SLSTDelta { get; }
    }
}
