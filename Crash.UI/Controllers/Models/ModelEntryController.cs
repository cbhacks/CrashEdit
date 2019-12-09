namespace Crash.UI
{
    public sealed class ModelEntryController : EntryController
    {
        public ModelEntryController(EntryChunkController up,ModelEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new ModelEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.ModelEntryController_Text,Entry.EName);
    }
}
