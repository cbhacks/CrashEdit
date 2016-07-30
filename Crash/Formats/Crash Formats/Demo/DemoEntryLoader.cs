namespace Crash
{
    [EntryType(19,GameVersion.Crash1BetaMAY11)]
    [EntryType(19,GameVersion.Crash1)]
    [EntryType(19,GameVersion.Crash2)]
    [EntryType(19,GameVersion.Crash3)]
    public sealed class DemoEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items.Length != 1)
            {
                ErrorManager.SignalError("DemoEntry: Wrong number of items");
            }
            return new DemoEntry(items[0],eid,size);
        }
    }
}
