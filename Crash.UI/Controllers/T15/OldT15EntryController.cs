namespace Crash.UI
{
    public sealed class OldT15EntryController : MysteryMultiItemEntryController
    {
        public OldT15EntryController(EntryChunkController up,OldT15Entry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldT15Entry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldT15EntryController_Text,Entry.EName);
    }
}
