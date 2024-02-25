namespace Crash
{
    [EntryType(15, GameVersion.Crash1Beta1995)]
    [EntryType(15, GameVersion.Crash1BetaMAR08)]
    [EntryType(15, GameVersion.Crash1BetaMAY11)]
    [EntryType(15, GameVersion.Crash1)]
    public sealed class ImageEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            return new ImageEntry(items, eid);
        }
    }
}
