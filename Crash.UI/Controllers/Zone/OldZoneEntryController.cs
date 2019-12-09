namespace Crash.UI
{
    public sealed class OldZoneEntryController : EntryController
    {
        public OldZoneEntryController(EntryChunkController up,OldZoneEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldZoneEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldZoneEntryController_Text,Entry.EName);
    }
}
