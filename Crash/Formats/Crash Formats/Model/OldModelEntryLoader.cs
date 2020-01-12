using System;

namespace Crash
{
    [EntryType(2,GameVersion.Crash1Beta1995)]
    [EntryType(2,GameVersion.Crash1BetaMAR08)]
    [EntryType(2,GameVersion.Crash1BetaMAY11)]
    [EntryType(2,GameVersion.Crash1)]
    public sealed class OldModelEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
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
            int structcount = BitConv.FromInt32(items[0],0x10);
            if (items[0].Length != structcount * 4 + 20)
            {
                ErrorManager.SignalError("OldModelEntry: Struct count mismatch");
            }
            OldModelStruct[] structs = new OldModelStruct[structcount];
            for (int i = 0; i < structs.Length; ++i)
            {
                structs[i] = ConvertPolyItem(items[0],20+i*4); // advance 4 bytes for each parse; note that structs can overlap
                if (structs[i] is OldModelTexture)
                {
                    structs[++i] = null;
                    structs[++i] = null;
                }
            }
            return new OldModelEntry(items[0],polygons,structs,eid);
        }

        internal static OldModelStruct ConvertPolyItem(byte[] item, int offset)
        {
            bool textured = (item[offset + 3] & 0x80) != 0;
            int size = textured ? 12 : 4;
            if ((offset + size) > item.Length) return null;
            byte[] data = new byte[size];
            Array.Copy(item,offset,data,0,size);
            if (textured)
                return OldModelTexture.Load(data);
            else
                return OldSceneryColor.Load(data);
        }
    }
}
