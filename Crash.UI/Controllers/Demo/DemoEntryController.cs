namespace Crash.UI
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        private DemoEntry entry;

        public DemoEntryController(EntryChunkController up,DemoEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new DemoEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.DemoEntryController_Text,entry.EName);
        }
    }
}
