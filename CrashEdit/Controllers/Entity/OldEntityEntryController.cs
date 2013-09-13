using Crash;

namespace CrashEdit
{
    public sealed class OldEntityEntryController : MysteryMultiItemEntryController
    {
        private OldEntityEntry oldentityentry;

        public OldEntityEntryController(EntryChunkController entrychunkcontroller,OldEntityEntry oldentityentry) : base(entrychunkcontroller,oldentityentry)
        {
            this.oldentityentry = oldentityentry;
            Node.Text = "Old Entity Entry";
            Node.ImageKey = "oldentityentry";
            Node.SelectedImageKey = "oldentityentry";
        }

        public OldEntityEntry OldEntityEntry
        {
            get { return oldentityentry; }
        }
    }
}
