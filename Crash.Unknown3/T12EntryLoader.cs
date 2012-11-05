namespace Crash.Unknown3
{
    [EntryType(12)]
    public sealed class T12EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new T12Entry(items[0],unknown);
        }
    }
}
