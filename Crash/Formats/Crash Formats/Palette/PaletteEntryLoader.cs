using System;

namespace Crash
{
    [EntryType(18, GameVersion.Crash1BetaMAR08)]
    [EntryType(18, GameVersion.Crash1BetaMAY11)]
    [EntryType(18, GameVersion.Crash1)]
    public sealed class PaletteEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            return new PaletteEntry(items, eid);
        }
    }
}
