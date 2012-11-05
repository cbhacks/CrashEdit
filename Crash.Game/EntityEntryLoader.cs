namespace Crash.Game
{
    //[EntryType(7)]
    public sealed class EntityEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int unknown)
        {
            if (items.Length < 2)
            {
                throw new System.Exception();
            }
            byte[] unknown1 = items[0];
            byte[] unknown2 = items[1];
            Entity[] entities = new Entity [items.Length - 2];
            for (int i = 2;i < items.Length;i++)
            {
                entities[i - 2] = Entity.Load(items[i]);
            }
            return new EntityEntry(unknown1,unknown2,entities,unknown);
        }
    }
}
