namespace Crash.UI
{
    public sealed class ZoneEntryController : EntryController
    {
        public ZoneEntryController(EntryChunkController up,ZoneEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new ZoneEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.ZoneEntryController_Text,Entry.EName);
    }
}
