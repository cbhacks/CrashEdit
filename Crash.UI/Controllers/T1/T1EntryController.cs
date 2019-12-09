namespace Crash.UI
{
    public sealed class T1EntryController : MysteryMultiItemEntryController
    {
        public T1EntryController(EntryChunkController up,T1Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new T1Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T1EntryController_Text,Entry.EName);
    }
}
