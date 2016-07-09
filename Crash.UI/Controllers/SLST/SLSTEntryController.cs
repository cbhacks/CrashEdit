namespace Crash.UI
{
    public sealed class SLSTEntryController : EntryController
    {
        private SLSTEntry entry;

        public SLSTEntryController(EntryChunkController up,SLSTEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new SLSTEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.SLSTEntryController_Text,entry.EName);
        }
    }
}
