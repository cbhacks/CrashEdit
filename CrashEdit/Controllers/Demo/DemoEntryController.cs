using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            DemoEntry = demoentry;
        }

        public DemoEntry DemoEntry { get; }
    }
}
