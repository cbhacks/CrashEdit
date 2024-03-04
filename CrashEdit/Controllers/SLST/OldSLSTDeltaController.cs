using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldSLSTDelta))]
    public sealed class OldSLSTDeltaController : LegacyController
    {
        public OldSLSTDeltaController(OldSLSTDelta oldslstdelta, SubcontrollerGroup parentGroup) : base(parentGroup, oldslstdelta)
        {
            OldSLSTDelta = oldslstdelta;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new OldSLSTDeltaBox(OldSLSTDelta);
        }

        public OldSLSTDelta OldSLSTDelta { get; }
    }
}
