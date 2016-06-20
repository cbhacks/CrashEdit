namespace Crash.UI
{
    public sealed class OldT15EntryController : MysteryMultiItemEntryController
    {
        private OldT15Entry entry;

        public OldT15EntryController(EntryChunkController up,OldT15Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldT15Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldT15EntryController_Text,entry.EName);
        }
    }
}
