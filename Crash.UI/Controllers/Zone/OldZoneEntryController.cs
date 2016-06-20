namespace Crash.UI
{
    public sealed class OldZoneEntryController : EntryController
    {
        private OldZoneEntry entry;

        public OldZoneEntryController(EntryChunkController up,OldZoneEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new OldZoneEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.OldZoneEntryController_Text,entry.EName);
        }
    }
}
