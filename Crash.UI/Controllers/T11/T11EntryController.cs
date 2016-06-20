namespace Crash.UI
{
    public sealed class T11EntryController : MysteryMultiItemEntryController
    {
        private T11Entry entry;

        public T11EntryController(EntryChunkController up,T11Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T11Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T11EntryController_Text,entry.EName);
        }
    }
}
