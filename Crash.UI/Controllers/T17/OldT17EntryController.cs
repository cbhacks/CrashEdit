namespace Crash.UI
{
    public sealed class OldT17EntryController : EntryController
    {
        public OldT17EntryController(EntryChunkController up,OldT17Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldT17Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldT17EntryController_Text,Entry.EName);
    }
}
