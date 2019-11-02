using Crash;

namespace CrashEdit
{
    public sealed class OldSLSTEntryController : EntryController
    {
        public OldSLSTEntryController(EntryChunkController entrychunkcontroller,OldSLSTEntry oldslstentry) : base(entrychunkcontroller,oldslstentry)
        {
            OldSLSTEntry = oldslstentry;
            AddNode(new OldSLSTSourceController(null,oldslstentry.Start));
            foreach (OldSLSTDelta delta in oldslstentry.Deltas)
            {
                AddNode(new OldSLSTDeltaController(this,delta));
            }
            AddNode(new OldSLSTSourceController(null,oldslstentry.End));
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Sort List ({0})",OldSLSTEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldSLSTEntry OldSLSTEntry { get; }
    }
}
