namespace Crash
{
    [EntryType(7,GameVersion.Crash1Beta1995)]
    public sealed class ProtoZoneEntryLoader : EntryLoader
    {
        private int camcount;
        
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items.Length < 2)
            {
                ErrorManager.SignalError("ProtoZoneEntry: Wrong number of items");
            }
            byte[] unknown1 = items[0];
            byte[] unknown2 = items[1];
            int camcount = BitConv.FromInt32(unknown1,520);
            OldCamera[] cameras = new OldCamera[camcount];
            for (int i = 2; i < 2 + camcount; i++)
            {
                cameras[i - 2] = OldCamera.Load(items[i]);
            }
            ProtoEntity[] entities = new ProtoEntity[items.Length - 2 - camcount];
            for (int i = 2 + camcount; i < items.Length; i++)
            {
                entities[i - 2 - camcount] = ProtoEntity.Load(items[i]);
            }
            
            return new ProtoZoneEntry(unknown1,unknown2,cameras,entities,camcount,eid,size);
        }
    }
}