using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SLSTDeltaController : LegacyController
    {
        public SLSTDeltaController(SLSTEntryController slstentrycontroller,SLSTDelta slstdelta) : base(slstentrycontroller, slstdelta)
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

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTDeltaBox(SLSTDelta);
        }

        public SLSTEntryController SLSTEntryController { get; }
        public SLSTDelta SLSTDelta { get; }
    }
}
