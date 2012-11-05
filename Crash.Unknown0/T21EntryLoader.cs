namespace Crash.Unknown0
{
    [EntryType(21)]
    public sealed class T21EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            return new T21Entry(items,unknown);
        }
    }
}
