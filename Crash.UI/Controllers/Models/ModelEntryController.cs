namespace Crash.UI
{
    public sealed class ModelEntryController : EntryController
    {
        private ModelEntry entry;

        public ModelEntryController(EntryChunkController up,ModelEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new ModelEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.ModelEntryController_Text,entry.EName);
        }
    }
}
