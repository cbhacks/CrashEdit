namespace Crash.Game
{
    public sealed class DemoEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length != 1)
            {
                throw new System.Exception();
            }
            return new DemoEntry(items[0],unknown);
        }
    }
}
