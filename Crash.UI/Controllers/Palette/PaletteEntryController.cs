namespace Crash.UI
{
    public sealed class PaletteEntryController : EntryController
    {
        public PaletteEntryController(EntryChunkController up,PaletteEntry entry) : base(up,entry)
        {
            Entry = entry;
        }

        public new PaletteEntry Entry { get; }

        public override string ToString() => string.Format(Properties.Resources.PaletteEntryController_Text,Entry.EName);
    }
}
