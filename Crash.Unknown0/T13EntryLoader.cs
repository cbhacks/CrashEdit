namespace Crash.Unknown0
{
    [EntryType(13)]
    public sealed class T13EntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 3)
            {
                throw new System.Exception();
            }
            return new T13Entry(items,unknown);
        }
    }
}
