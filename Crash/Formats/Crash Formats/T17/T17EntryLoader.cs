namespace Crash
{
    [EntryType(17, GameVersion.Crash3)]
    public sealed class T17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            return new T17Entry(items, eid);
        }
    }
}
