namespace Crash.Unknown0
{
    [EntryType(2)]
    public sealed class T2EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length < 5)
            {
                throw new System.Exception();
            }
            if (items[0].Length != 80)
            {
                throw new System.Exception();
            }
            return new T2Entry(items,unknown);
        }
    }
}
