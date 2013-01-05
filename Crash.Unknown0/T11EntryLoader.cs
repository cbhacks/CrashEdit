namespace Crash.Unknown0
{
    [EntryType(11)]
    public sealed class T11EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            if (items.Length < 3)
            {
                throw new System.Exception();
            }
            if (items[0].Length != 24)
            {
                throw new System.Exception();
            }
            return new T11Entry(items,unknown);
        }
    }
}
