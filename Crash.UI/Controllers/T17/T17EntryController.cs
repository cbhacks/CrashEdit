namespace Crash.UI
{
    public sealed class T17EntryController : MysteryMultiItemEntryController
    {
        public T17EntryController(EntryChunkController up,T17Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new T17Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T17EntryController_Text,Entry.EName);
    }
}
