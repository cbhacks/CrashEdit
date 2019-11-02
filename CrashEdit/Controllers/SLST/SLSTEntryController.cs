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
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Sort List ({0})",SLSTEntry.EName);
            Node.ImageKey = "greyb";
            Node.SelectedImageKey = "greyb";
        }

        public SLSTEntry SLSTEntry { get; }
    }
}
