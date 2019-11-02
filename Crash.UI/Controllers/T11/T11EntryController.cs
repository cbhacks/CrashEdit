namespace Crash.UI
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        private CodeEntry entry;

        public T11EntryController(EntryChunkController up,CodeEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new CodeEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T11EntryController_Text,entry.EName);
        }
    }
}
