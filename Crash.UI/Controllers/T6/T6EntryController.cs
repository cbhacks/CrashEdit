namespace Crash.UI
{
    public sealed class T6EntryController : MysteryUniItemEntryController
    {
        public T6EntryController(EntryChunkController up,T6Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new T6Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T6EntryController_Text,Entry.EName);
    }
}
