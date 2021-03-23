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
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.SLSTEntryController_Text,SLSTEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "greyb";
        }

        public SLSTEntry SLSTEntry { get; }
    }
}
