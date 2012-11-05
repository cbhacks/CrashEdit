namespace Crash.Unknown5
{
    [EntryType(20)]
    public sealed class T20EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new T20Entry(items[0],unknown);
        }
    }
}
