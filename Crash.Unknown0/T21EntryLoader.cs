namespace Crash.Unknown0
{
    [EntryType(21)]
    public sealed class T21EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            return new T21Entry(items,unknown);
        }
    }
}
