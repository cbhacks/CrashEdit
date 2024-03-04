namespace CrashEdit.Crash
{
    [EntryType(18, GameVersion.Crash1BetaMAR08)]
    [EntryType(18, GameVersion.Crash1BetaMAY11)]
    [EntryType(18, GameVersion.Crash1)]
    public sealed class PaletteEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            ArgumentNullException.ThrowIfNull(items);
            return new PaletteEntry(items, eid);
        }
    }
}
