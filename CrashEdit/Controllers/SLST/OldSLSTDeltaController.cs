using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    public sealed class OldSLSTDeltaController : LegacyController
    {
        public OldSLSTDeltaController(OldSLSTEntryController oldslstentrycontroller,OldSLSTDelta oldslstdelta) : base(oldslstentrycontroller, oldslstdelta)
        {
            OldSLSTDelta = oldslstdelta;
            InvalidateNode();
        }

        public void InvalidateNode()
        {
            NodeText = "Delta";
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldSLSTDeltaBox(OldSLSTDelta);
        }

        public OldSLSTEntryController OldSLSTEntryController => (OldSLSTEntryController)Modern.Parent.Legacy;
        public OldSLSTDelta OldSLSTDelta { get; }
    }
}
