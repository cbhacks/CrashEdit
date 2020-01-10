using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1BetaMAR08)]
    [EntryType(3,GameVersion.Crash1BetaMAY11)]
    [EntryType(3,GameVersion.Crash1)]
    public sealed class OldSceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 4)
            {
                ErrorManager.SignalError("OldSceneryEntry: Wrong number of items");
            }
            int polygoncount = BitConv.FromInt32(items[0],0xC);
            int vertexcount = BitConv.FromInt32(items[0],0x10);
            int structcount = BitConv.FromInt32(items[0],0x14);
            if (items[1].Length != polygoncount * 8)
            {
                ErrorManager.SignalError("OldSceneryEntry: Polygon count mismatch");
            }
            if (items[2].Length != vertexcount * 8)
            {
                ErrorManager.SignalError("OldSceneryEntry: Vertex count mismatch");
            }
            if (items[0].Length - 0x40 != structcount * 4)
            {
                ErrorManager.SignalError("OldSceneryEntry: Struct count mismatch");
            }
            OldSceneryPolygon[] polygons = new OldSceneryPolygon[polygoncount];
            for (int i = 0;i < polygons.Length;i++)
            {
                byte[] polygondata = new byte [8];
                Array.Copy(items[1],i * 8,polygondata,0,8);
                polygons[i] = OldSceneryPolygon.Load(polygondata);
            }
            OldSceneryVertex[] vertices = new OldSceneryVertex[vertexcount];
            for (int i = 0;i < vertices.Length;i++)
            {
                byte[] vertexdata = new byte [8];
                Array.Copy(items[2],i * 8,vertexdata,0,8);
                vertices[i] = OldSceneryVertex.Load(vertexdata);
            }
            OldModelStruct[] structs = new OldModelStruct[structcount];
            for (int i = 0; i < structs.Length; i++)
            {
                structs[i] = ConvertPolyItem(items[0],0x40+(i*4)); // advance 4 bytes for each parse; note that structs can overlap
            }
            byte[] extradata = null;
            if (items.Length >= 4)
            {
                extradata = items[3];
            }
            return new OldSceneryEntry(items[0],polygons,vertices,structs,extradata,eid);
        }

        private static OldModelStruct ConvertPolyItem(byte[] item, int offset)
        {
            bool textured = (item[offset + 3] & 0x80) != 0;
            int size = textured ? 8 : 4;
            if ((offset + size) > item.Length) return null;
            byte[] data = new byte[size];
            Array.Copy(item,offset,data,0,size);
            if (textured)
                return OldSceneryTexture.Load(data);
            else
                return OldSceneryColor.Load(data);
        }
    }
}