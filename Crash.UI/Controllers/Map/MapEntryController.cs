namespace Crash.UI
{
    public sealed class MapEntryController : EntryController
    {
        public MapEntryController(EntryChunkController up,MapEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new MapEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.MapEntryController_Text,Entry.EName);
    }
}
