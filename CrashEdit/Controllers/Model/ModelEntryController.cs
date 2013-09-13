using Crash;

namespace CrashEdit
{
    public sealed class ModelEntryController : MysteryMultiItemEntryController
    {
        private ModelEntry modelentry;

        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            this.modelentry = modelentry;
            Node.Text = "Model Entry";
            Node.ImageKey = "modelentry";
            Node.SelectedImageKey = "modelentry";
        }

        public ModelEntry ModelEntry
        {
            get { return modelentry; }
        }
    }
}
