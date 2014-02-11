using Crash;

namespace CrashEdit
{
    public sealed class OldModelEntryController : EntryController
    {
        private OldModelEntry oldmodelentry;

        public OldModelEntryController(EntryChunkController entrychunkcontroller,OldModelEntry oldmodelentry) : base(entrychunkcontroller,oldmodelentry)
        {
            this.oldmodelentry = oldmodelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Model Entry ({0})",oldmodelentry.EIDString);
            Node.ImageKey = "oldmodelentry";
            Node.SelectedImageKey = "oldmodelentry";
        }

        public OldModelEntry OldModelEntry
        {
            get { return oldmodelentry; }
        }
    }
}
