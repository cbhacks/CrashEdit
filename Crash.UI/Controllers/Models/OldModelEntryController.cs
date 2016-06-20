namespace Crash.UI
{
    public sealed class OldModelEntryController : EntryController
    {
        private OldModelEntry entry;

        public OldModelEntryController(EntryChunkController up,OldModelEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldModelEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldModelEntryController_Text,entry.EName);
        }
    }
}
