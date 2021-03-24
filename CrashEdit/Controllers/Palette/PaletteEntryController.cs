using CrashEdit.Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit.CE
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
            NodeText = string.Format(CrashUI.Properties.Resources.PaletteEntryController_Text,PaletteEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            NodeImageKey = "ThingYellow";
        }

        public override bool EditorAvailable => Type.GetType("Mono.Runtime") == null;

        public override Control CreateEditor()
        {
            // Hack for Mono so it doesn't crash.
            if (Type.GetType("Mono.Runtime") != null)
                return base.CreateEditor();
            return new PaletteEntryBox(PaletteEntry);
        }

        public PaletteEntry PaletteEntry { get; }
    }
}
