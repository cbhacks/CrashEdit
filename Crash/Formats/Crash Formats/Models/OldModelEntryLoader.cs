using System;

namespace Crash
{
    [EntryType(2,GameVersion.Crash1Beta1995)]
    [EntryType(2,GameVersion.Crash1BetaMAR08)]
    [EntryType(2,GameVersion.Crash1BetaMAY11)]
    [EntryType(2,GameVersion.Crash1)]
    public sealed class OldModelEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 2)
            {
                ErrorManager.SignalError("OldModelEntry: Wrong number of items");
            }
            int polygoncount = BitConv.FromInt32(items[0],0);
            if (items[1].Length != polygoncount * 8)
            {
                ErrorManager.SignalError("OldModelEntry: Polygon count mismatch");
            }
            OldModelPolygon[] polygons = new OldModelPolygon [polygoncount];
            for (int i = 0;i < polygoncount;i++)
            {
                byte[] polygondata = new byte [8];
                Array.Copy(items[1],i * 8,polygondata,0,polygondata.Length);
                polygons[i] = OldModelPolygon.Load(polygondata);
            }
            return new OldModelEntry(items[0],polygons,eid,size);
        }
    }
}
