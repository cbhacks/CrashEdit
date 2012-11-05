namespace Crash.Unknown0
{
    [EntryType(1)]
    public sealed class T1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            return new T1Entry(items,unknown);
        }
    }
}
