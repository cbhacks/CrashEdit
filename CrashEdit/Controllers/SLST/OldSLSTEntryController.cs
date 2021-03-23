using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class OldSLSTEntryController : EntryController
    {
        public OldSLSTEntryController(EntryChunkController entrychunkcontroller,OldSLSTEntry oldslstentry) : base(entrychunkcontroller,oldslstentry)
        {
            OldSLSTEntry = oldslstentry;
            AddNode(new OldSLSTSourceController(this,oldslstentry.Start));
            foreach (OldSLSTDelta delta in oldslstentry.Deltas)
            {
                AddNode(new OldSLSTDeltaController(this,delta));
            }
            AddNode(new OldSLSTSourceController(this,oldslstentry.End));
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            NodeText = string.Format(CrashUI.Properties.Resources.OldSLSTEntryController_Text,OldSLSTEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "greyb";
        }

        public OldSLSTEntry OldSLSTEntry { get; }
    }
}
