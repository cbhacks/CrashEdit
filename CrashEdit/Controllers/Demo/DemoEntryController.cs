using Crash;

namespace CrashEdit
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        public DemoEntryController(EntryChunkController entrychunkcontroller,DemoEntry demoentry) : base(entrychunkcontroller,demoentry)
        {
            DemoEntry = demoentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.DemoEntryController_Text,DemoEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public DemoEntry DemoEntry { get; }
    }
}
