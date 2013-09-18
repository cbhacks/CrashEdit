using Crash;

namespace CrashEdit
{
    public sealed class OldEntityEntryController : MysteryMultiItemEntryController
    {
        private OldEntityEntry oldentityentry;

        public OldEntityEntryController(EntryChunkController entrychunkcontroller,OldEntityEntry oldentityentry) : base(entrychunkcontroller,oldentityentry)
        {
            this.oldentityentry = oldentityentry;
            Node.Text = string.Format("Old Entity Entry ({0})",oldentityentry.EIDString);
            Node.ImageKey = "oldentityentry";
            Node.SelectedImageKey = "oldentityentry";
        }

        public OldEntityEntry OldEntityEntry
        {
            get { return oldentityentry; }
        }
    }
}
