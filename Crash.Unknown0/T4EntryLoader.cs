namespace Crash.Unknown0
{
    [EntryType(4)]
    public sealed class T4EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            return new T4Entry(items,unknown);
        }
    }
}
