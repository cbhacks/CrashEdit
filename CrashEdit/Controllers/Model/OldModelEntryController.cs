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
            Node.Text = string.Format("Old Model ({0})",oldmodelentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldModelEntry OldModelEntry
        {
            get { return oldmodelentry; }
        }
    }
}
