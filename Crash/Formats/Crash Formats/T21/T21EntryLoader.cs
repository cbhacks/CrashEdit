namespace Crash
{
    [EntryType(21, GameVersion.Crash2)]
    [EntryType(21, GameVersion.Crash3)]
    public sealed class T21EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid, GameVersion version)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            return new T21Entry(items, eid);
        }
    }
}
