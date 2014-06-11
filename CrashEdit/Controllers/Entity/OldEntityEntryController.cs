using Crash;

namespace CrashEdit
{
    public sealed class OldEntityEntryController : MysteryMultiItemEntryController
    {
        private OldEntityEntry oldentityentry;

        public OldEntityEntryController(EntryChunkController entrychunkcontroller,OldEntityEntry oldentityentry) : base(entrychunkcontroller,oldentityentry)
        {
            this.oldentityentry = oldentityentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Entity Entry ({0})",oldentityentry.EName);
            Node.ImageKey = "oldentityentry";
            Node.SelectedImageKey = "oldentityentry";
        }

        public OldEntityEntry OldEntityEntry
        {
            get { return oldentityentry; }
        }
    }
}
