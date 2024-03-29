namespace CrashEdit.Crash
{
    [EntryType(15, GameVersion.Crash2)]
    [EntryType(15, GameVersion.Crash3)]
    public sealed class T15EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items, int eid)
        {
            ArgumentNullException.ThrowIfNull(items);
            if (items.Length != 1)
            {
                ErrorManager.SignalError("T15Entry: Wrong number of items");
            }
            return new T15Entry(items[0], eid);
        }
    }
}
