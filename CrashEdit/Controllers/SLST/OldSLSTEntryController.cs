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
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.OldSLSTEntryController_Text,OldSLSTEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "greyb";
            Node.SelectedImageKey = "greyb";
        }

        public OldSLSTEntry OldSLSTEntry { get; }
    }
}
