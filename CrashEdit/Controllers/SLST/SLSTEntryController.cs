using Crash;

namespace CrashEdit
{
    public sealed class SLSTEntryController : EntryController
    {
        public SLSTEntryController(EntryChunkController entrychunkcontroller,SLSTEntry slstentry) : base(entrychunkcontroller,slstentry)
        {
            SLSTEntry = slstentry;
            AddNode(new SLSTSourceController(null,slstentry.Start));
            foreach (SLSTDelta delta in slstentry.Deltas)
            {
                AddNode(new SLSTDeltaController(this,delta));
            }
            AddNode(new SLSTSourceController(null,slstentry.End));
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.SLSTEntryController_Text,SLSTEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "greyb";
            Node.SelectedImageKey = "greyb";
        }

        public SLSTEntry SLSTEntry { get; }
    }
}
