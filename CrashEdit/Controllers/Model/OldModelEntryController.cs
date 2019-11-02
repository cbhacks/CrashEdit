using Crash;

namespace CrashEdit
{
    public sealed class OldModelEntryController : EntryController
    {
        public OldModelEntryController(EntryChunkController entrychunkcontroller,OldModelEntry oldmodelentry) : base(entrychunkcontroller,oldmodelentry)
        {
            OldModelEntry = oldmodelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Old Model ({0})",OldModelEntry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public OldModelEntry OldModelEntry { get; }
    }
}
