using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class PaletteEntryController : EntryController
    {
        public PaletteEntryController(EntryChunkController entrychunkcontroller,PaletteEntry paletteentry) : base(entrychunkcontroller,paletteentry)
        {
            PaletteEntry = paletteentry;
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.PaletteEntryController_Text,PaletteEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "yellowb";
            Node.SelectedImageKey = "yellowb";
        }

        protected override Control CreateEditor()
        {
            // Hack for Mono so it doesn't crash.
            if (Type.GetType("Mono.Runtime") != null)
                return base.CreateEditor();
            return new PaletteEntryBox(PaletteEntry);
        }

        public PaletteEntry PaletteEntry { get; }
    }
}
