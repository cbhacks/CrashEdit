namespace Crash.Unknown0
{
    [EntryType(3)]
    public sealed class T3EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            if (items.Length != 7)
            {
                throw new System.Exception();
            }
            if (items[0].Length != 76)
            {
                throw new System.Exception();
            }
            return new T3Entry(items,unknown);
        }
    }
}
