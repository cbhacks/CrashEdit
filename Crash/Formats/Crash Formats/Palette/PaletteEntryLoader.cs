namespace CrashEdit.Crash
{
    [EntryType(18, GameVersion.Crash1BetaMAR08)]
    [EntryType(18, GameVersion.Crash1BetaMAY11)]
    [EntryType(18, GameVersion.Crash1)]
    public sealed class PaletteEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            return new PaletteEntry(items, eid);
        }
    }
}
