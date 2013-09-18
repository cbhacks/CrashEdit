using Crash;

namespace CrashEdit
{
    public sealed class ModelEntryController : MysteryMultiItemEntryController
    {
        private ModelEntry modelentry;

        public ModelEntryController(EntryChunkController entrychunkcontroller,ModelEntry modelentry) : base(entrychunkcontroller,modelentry)
        {
            this.modelentry = modelentry;
            Node.Text = string.Format("Model Entry ({0})",modelentry.EIDString);
            Node.ImageKey = "modelentry";
            Node.SelectedImageKey = "modelentry";
        }

        public ModelEntry ModelEntry
        {
            get { return modelentry; }
        }
    }
}
