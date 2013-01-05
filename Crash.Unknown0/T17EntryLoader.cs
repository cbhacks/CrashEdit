namespace Crash.Unknown0
{
    [EntryType(17)]
    public sealed class T17EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            return new T17Entry(items,unknown);
        }
    }
}
