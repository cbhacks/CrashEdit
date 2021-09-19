using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class SLSTEntryController : EntryController
    {
        public SLSTEntryController(EntryChunkController entrychunkcontroller,SLSTEntry slstentry) : base(entrychunkcontroller,slstentry)
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
