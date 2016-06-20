namespace Crash.UI
{
    public sealed class ZoneEntryController : EntryController
    {
        private NewZoneEntry entry;

        public ZoneEntryController(EntryChunkController up,NewZoneEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new NewZoneEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.ZoneEntryController_Text,entry.EName);
        }
    }
}
