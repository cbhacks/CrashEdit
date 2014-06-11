using Crash;

namespace CrashEdit
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        private DemoEntry demoentry;

        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            this.demoentry = demoentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Demo Entry ({0})",demoentry.EName);
            Node.ImageKey = "demoentry";
            Node.SelectedImageKey = "demoentry";
        }

        public DemoEntry DemoEntry
        {
            get { return demoentry; }
        }
    }
}
