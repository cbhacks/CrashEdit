namespace Crash.UI
{
    public sealed class T4EntryController : EntryController
    {
        private T4Entry entry;

        public T4EntryController(EntryChunkController up,T4Entry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new T4Entry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.T4EntryController_Text,entry.EName);
        }
    }
}
