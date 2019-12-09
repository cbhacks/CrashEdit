namespace Crash.UI
{
    public sealed class SLSTEntryController : EntryController
    {
        public SLSTEntryController(EntryChunkController up,SLSTEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new SLSTEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.SLSTEntryController_Text,Entry.EName);
    }
}
