namespace Crash.UI
{
    public sealed class PaletteEntryController : MysteryMultiItemEntryController
    {
        private PaletteEntry entry;

        public PaletteEntryController(EntryChunkController up,PaletteEntry entry) : base(up,entry)
        {
            this.entry = entry;
        }

        public new PaletteEntry Entry
        {
            get { return entry; }
        }

        public override string ToString()
        {
            return string.Format(Properties.Resources.PaletteEntryController_Text,entry.EName);
        }
    }
}
