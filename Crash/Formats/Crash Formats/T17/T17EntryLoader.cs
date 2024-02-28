namespace CrashEdit.Crash
{
    [EntryType(17, GameVersion.Crash3)]
    public sealed class T17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            ArgumentNullException.ThrowIfNull(items);
            return new T17Entry(items, eid);
        }
    }
}
