namespace Crash.UI
{
    public sealed class OldModelEntryController : EntryController
    {
        public OldModelEntryController(EntryChunkController up,OldModelEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new OldModelEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.OldModelEntryController_Text,Entry.EName);
    }
}
