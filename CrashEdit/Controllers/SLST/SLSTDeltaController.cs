using CrashEdit.Crash;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(SLSTDelta))]
    public sealed class SLSTDeltaController : LegacyController
    {
        public SLSTDeltaController(SLSTDelta slstdelta, SubcontrollerGroup parentGroup) : base(parentGroup, slstdelta)
        {
            SLSTDelta = slstdelta;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTDeltaBox(SLSTDelta);
        }

        public SLSTDelta SLSTDelta { get; }
    }
}
