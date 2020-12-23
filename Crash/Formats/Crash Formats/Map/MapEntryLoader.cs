using System;

namespace Crash
{
    [EntryType(17,GameVersion.Crash1BetaMAR08)]
    [EntryType(17,GameVersion.Crash1BetaMAY11)]
    [EntryType(17,GameVersion.Crash1)]
    [EntryType(17,GameVersion.Crash2)]
    public sealed class MapEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
            {
                ErrorManager.SignalError("MapEntry: Wrong number of items");
            }
            byte[] header = items[0];
            byte[] layout = items[1];
            int entitycount = BitConv.FromInt32(header,0xC);
            OldEntity[] entities = new OldEntity[entitycount];
            for (int i = 0; i < entitycount; ++i)
            {
                entities[i] = OldEntity.Load(items[i+2]);
            }
            return new MapEntry(header,layout,entities,eid);
        }
    }
}
