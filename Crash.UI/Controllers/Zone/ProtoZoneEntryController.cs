namespace Crash.UI
{
    public sealed class ProtoZoneEntryController : EntryController
    {
        public ProtoZoneEntryController(EntryChunkController up,ProtoZoneEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new ProtoZoneEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.ProtoZoneEntryController_Text,Entry.EName);
    }
}
