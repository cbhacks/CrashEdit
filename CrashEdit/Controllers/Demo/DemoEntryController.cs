using CrashEdit.Crash;

namespace CrashEdit.CE
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
            NodeText = string.Format(CrashUI.Properties.Resources.DemoEntryController_Text,DemoEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "ThingOrange";
        }

        public DemoEntry DemoEntry { get; }
    }
}
