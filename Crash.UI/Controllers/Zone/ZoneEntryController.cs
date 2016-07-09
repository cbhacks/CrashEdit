namespace Crash.UI
{
    public sealed class ZoneEntryController : EntryController
    {
        private ZoneEntry entry;

        public ZoneEntryController(EntryChunkController up,ZoneEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new ZoneEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.ZoneEntryController_Text,entry.EName);
        }
    }
}
