namespace Crash.UI
{
    public sealed class DemoEntryController : MysteryUniItemEntryController
    {
        public DemoEntryController(EntryChunkController up,DemoEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new DemoEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.DemoEntryController_Text,Entry.EName);
    }
}
