namespace Crash
{
    [EntryType(7,GameVersion.Crash3)]
    public sealed class NewZoneEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items.Length < 2)
            {
                ErrorManager.SignalError("NewZoneEntry: Wrong number of items");
            }
            byte[] unknown1 = items[0];
            byte[] unknown2 = items[1];
            Entity[] entities = new Entity [items.Length - 2];
            for (int i = 2;i < items.Length;i++)
            {
                entities[i - 2] = Entity.Load(items[i]);
            }
            return new NewZoneEntry(unknown1,unknown2,entities,eid,size);
        }
    }
}
