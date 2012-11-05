namespace Crash.Unknown0
{
    [EntryType(17)]
    public sealed class T17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            return new T17Entry(items,unknown);
        }
    }
}
