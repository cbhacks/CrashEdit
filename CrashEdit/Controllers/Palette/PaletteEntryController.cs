using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(PaletteEntry))]
    public sealed class PaletteEntryController : EntryController
    {
        public PaletteEntryController(PaletteEntry paletteentry, SubcontrollerGroup parentGroup) : base(paletteentry, parentGroup)
        {
            PaletteEntry = paletteentry;
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
