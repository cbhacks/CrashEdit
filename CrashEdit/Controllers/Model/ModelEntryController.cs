using Crash;

namespace CrashEdit
{
    public sealed class ModelEntryController : EntryController
    {
        private ModelEntry modelentry;

        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            this.modelentry = modelentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Model ({0})",modelentry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public ModelEntry ModelEntry
        {
            get { return modelentry; }
        }
    }
}
