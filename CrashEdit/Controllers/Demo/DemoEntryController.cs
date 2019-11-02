using Crash;

namespace CrashEdit
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            DemoEntry = demoentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Demo ({0})",DemoEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public DemoEntry DemoEntry { get; }
    }
}
