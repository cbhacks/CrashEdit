using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(SLSTEntry))]
    public sealed class SLSTEntryController : EntryController
    {
        public SLSTEntryController(SLSTEntry slstentry, SubcontrollerGroup parentGroup) : base(slstentry, parentGroup)
        {
            SLSTEntry = slstentry;
            AddNode(new SLSTSourceController(this,slstentry.Start));
            foreach (SLSTDelta delta in slstentry.Deltas)
            {
                AddNode(new SLSTDeltaController(this,delta));
            }
            AddNode(new SLSTSourceController(this,slstentry.End));
        }

        public SLSTEntry SLSTEntry { get; }
    }
}
