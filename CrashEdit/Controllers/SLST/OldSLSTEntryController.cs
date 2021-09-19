using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class OldSLSTEntryController : EntryController
    {
        public OldSLSTEntryController(EntryChunkController entrychunkcontroller,OldSLSTEntry oldslstentry) : base(entrychunkcontroller,oldslstentry)
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
