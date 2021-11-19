using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldSLSTDelta))]
    public sealed class OldSLSTDeltaController : LegacyController
    {
        public OldSLSTDeltaController(OldSLSTDelta oldslstdelta, SubcontrollerGroup parentGroup) : base(parentGroup, oldslstdelta)
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

        public OldSLSTDelta OldSLSTDelta { get; }
    }
}
