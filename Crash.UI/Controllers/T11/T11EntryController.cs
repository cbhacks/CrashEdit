namespace Crash.UI
{
    public sealed class GOOLEntryController : EntryController
    {
        public GOOLEntryController(EntryChunkController up,GOOLEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new GOOLEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.T11EntryController_Text,Entry.EName);
    }
}
