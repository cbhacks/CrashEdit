using Crash;

namespace CrashEdit
{
    public sealed class PaletteEntryController : MysteryMultiItemEntryController
    {
        private PaletteEntry paletteentry;

        public PaletteEntryController(EntryChunkController entrychunkcontroller,PaletteEntry paletteentry) : base(entrychunkcontroller,paletteentry)
        {
            this.paletteentry = paletteentry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Palette Entry ({0})",paletteentry.EName);
            Node.ImageKey = "paletteentry";
            Node.SelectedImageKey = "paletteentry";
        }

        public PaletteEntry PaletteEntry
        {
            get { return paletteentry; }
        }
    }
}
