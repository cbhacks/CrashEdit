namespace Crash.Unknown0
{
    [EntryType(4)]
    public sealed class T4EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items == null)
                throw new System.ArgumentNullException("Items cannot be null.");
            T4Item[] t4items = new T4Item [items.Length];
            for (int i = 0;i < items.Length;i++)
            {
                t4items[i] = T4Item.Load(items[i]);
            }
            return new T4Entry(t4items,unknown);
        }
    }
}
