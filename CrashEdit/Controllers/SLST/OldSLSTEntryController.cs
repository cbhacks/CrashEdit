using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(OldSLSTEntry))]
    public sealed class OldSLSTEntryController : EntryController
    {
        public OldSLSTEntryController(OldSLSTEntry oldslstentry, SubcontrollerGroup parentGroup) : base(oldslstentry, parentGroup)
        {
            OldSLSTEntry = oldslstentry;
            AddNode(new OldSLSTSourceController(this,oldslstentry.Start));
            foreach (OldSLSTDelta delta in oldslstentry.Deltas)
            {
                AddNode(new OldSLSTDeltaController(this,delta));
            }
            AddNode(new OldSLSTSourceController(this,oldslstentry.End));
        }

        public OldSLSTEntry OldSLSTEntry { get; }
    }
}
