namespace Crash.UI
{
    public sealed class T21EntryController : MysteryMultiItemEntryController
    {
        public T21EntryController(EntryChunkController up,T21Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new T21Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T21EntryController_Text,Entry.EName);
    }
}
