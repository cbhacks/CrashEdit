namespace Crash.UI
{
    public sealed class T15EntryController : MysteryUniItemEntryController
    {
        public T15EntryController(EntryChunkController up,T15Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new T15Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T15EntryController_Text,Entry.EName);
    }
}
