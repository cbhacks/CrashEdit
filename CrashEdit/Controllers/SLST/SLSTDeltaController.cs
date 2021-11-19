using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class SLSTDeltaController : LegacyController
    {
        public SLSTDeltaController(SLSTEntryController slstentrycontroller,SLSTDelta slstdelta) : base(slstentrycontroller, slstdelta)
        {
            SLSTDelta = slstdelta;
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = "Delta";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTDeltaBox(SLSTDelta);
        }

        public SLSTEntryController SLSTEntryController => (SLSTEntryController)Modern.Parent.Legacy;
        public SLSTDelta SLSTDelta { get; }
    }
}
