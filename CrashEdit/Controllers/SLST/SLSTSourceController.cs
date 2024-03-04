using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(SLSTSource))]
    public sealed class SLSTSourceController : LegacyController
    {
        public SLSTSourceController(SLSTSource slstsource, SubcontrollerGroup parentGroup) : base(parentGroup, slstsource)
        {
            SLSTSource = slstsource;
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new SLSTSourceBox(SLSTSource);
        }

        public SLSTSource SLSTSource { get; }
    }
}
