using System;

namespace Crash
{
    [EntryType(7,GameVersion.Crash1BetaMAR08)]
    [EntryType(7,GameVersion.Crash1BetaMAY11)]
    [EntryType(7,GameVersion.Crash1)]
    public sealed class OldZoneEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length < 2)
            {
                ErrorManager.SignalError("OldZoneEntry: Wrong number of items");
            }
            byte[] header = items[0];
            byte[] layout = items[1];
            int camcount = BitConv.FromInt32(header,0x208);
            OldCamera[] cameras = new OldCamera[camcount];
            for (int i = 2; i < 2 + camcount; i++)
            {
                cameras[i - 2] = OldCamera.Load(items[i]);
            }
            int entitycount = BitConv.FromInt32(header,0x20C);
            OldEntity[] entities = new OldEntity[entitycount];
            for (int i = 2 + camcount; i < 2 + camcount + entitycount; i++)
            {
                entities[i - 2 - camcount] = OldEntity.Load(items[i]);
            }
            return new OldZoneEntry(header,layout,cameras,entities,eid);
        }
    }
}