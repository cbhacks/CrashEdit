namespace Crash.UI
{
    public sealed class OldT17EntryController : MysteryMultiItemEntryController
    {
        private OldT17Entry entry;

        public OldT17EntryController(EntryChunkController up,OldT17Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldT17Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldT17EntryController_Text,entry.EName);
        }
    }
}
