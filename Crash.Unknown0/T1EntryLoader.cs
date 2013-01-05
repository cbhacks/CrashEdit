namespace Crash.Unknown0
{
    [EntryType(1)]
    public sealed class T1EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            return new T1Entry(items,unknown);
        }
    }
}
