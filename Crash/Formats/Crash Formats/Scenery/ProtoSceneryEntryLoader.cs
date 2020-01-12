using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1Beta1995)]
    public sealed class ProtoSceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                ErrorManager.SignalError("ProtoSceneryEntry: Wrong number of items");
            }
            int polygoncount = BitConv.FromInt32(items[0],0xC);
            int vertexcount = BitConv.FromInt32(items[0],0x10);
            int structcount = BitConv.FromInt32(items[0],0x14);
            ProtoSceneryPolygon[] polygons = new ProtoSceneryPolygon[polygoncount];
            for (int i = 0;i < polygons.Length;i++)
            {
                byte[] polygondata = new byte [12];
                Array.Copy(items[1],i * 12,polygondata,0,12);
                polygons[i] = ProtoSceneryPolygon.Load(polygondata);
            }
            ProtoSceneryVertex[] vertices = new ProtoSceneryVertex[vertexcount];
            for (int i = 0;i < vertices.Length;i++)
            {
                byte[] vertexdata = new byte [6];
                Array.Copy(items[2],i * 6,vertexdata,0,6);
                vertices[i] = ProtoSceneryVertex.Load(vertexdata);
            }
            OldModelStruct[] structs = new OldModelStruct[structcount];
            for (int i = 0; i < structs.Length; i++)
            {
                structs[i] = ConvertPolyItem(items[0],0x40+(i*4)); // advance 4 bytes for each parse; note that structs can overlap
            }
            short? pad = null;
            if (vertices.Length*6 + 2 == items[2].Length)
                pad = BitConv.FromInt16(items[2],vertices.Length*6);
            return new ProtoSceneryEntry(items[0],polygons,vertices,structs,pad,eid);
        }

        internal static OldModelStruct ConvertPolyItem(byte[] item, int offset)
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
