namespace Crash.UI
{
    public sealed class ProtoZoneEntryController : EntryController
    {
        private ProtoZoneEntry entry;

        public ProtoZoneEntryController(EntryChunkController up,ProtoZoneEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new ProtoZoneEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.ProtoZoneEntryController_Text,entry.EName);
        }
    }
}
