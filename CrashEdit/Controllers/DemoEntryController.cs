using Crash;
using Crash.Game;

namespace CrashEdit
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        private DemoEntry demoentry;

        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            this.demoentry = demoentry;
            Node.Text = "Demo Entry";
            Node.ImageKey = "demoentry";
            Node.SelectedImageKey = "demoentry";
        }

        public DemoEntry DemoEntry
        {
            get { return demoentry; }
        }
    }
}
