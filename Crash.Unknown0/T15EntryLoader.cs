namespace Crash.Unknown0
{
    [EntryType(15)]
    public sealed class T15EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new T15Entry(items[0],unknown);
        }
    }
}
