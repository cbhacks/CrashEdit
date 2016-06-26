using Crash;

namespace CrashEdit
{
    public sealed class PaletteEntryController : MysteryMultiItemEntryController
    {
        private PaletteEntry t18entry;

        public PaletteEntryController(EntryChunkController entrychunkcontroller,PaletteEntry t18entry) : base(entrychunkcontroller,t18entry)
        {
            this.t18entry = t18entry;
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format("Palette ({0})",t18entry.EName);
            Node.ImageKey = "thing";
            Node.SelectedImageKey = "thing";
        }

        public PaletteEntry PaletteEntry
        {
            get { return t18entry; }
        }
    }
}
