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
            ProtoSceneryPolygon[] polygons = new ProtoSceneryPolygon[BitConv.FromInt32(items[0],0xC)];
            for (int i = 0;i < polygons.Length;i++)
            {
                byte[] polygondata = new byte [12];
                Array.Copy(items[1],i * 12,polygondata,0,12);
                polygons[i] = ProtoSceneryPolygon.Load(polygondata);
            }
            ProtoSceneryVertex[] vertices = new ProtoSceneryVertex[BitConv.FromInt32(items[0],0x10)];
            for (int i = 0;i < vertices.Length;i++)
            {
                byte[] vertexdata = new byte [6];
                Array.Copy(items[2],i * 6,vertexdata,0,6);
                vertices[i] = ProtoSceneryVertex.Load(vertexdata);
            }
            short? pad = null;
            if (vertices.Length*6 + 2 == items[2].Length)
                pad = BitConv.FromInt16(items[2],vertices.Length*6);
            return new ProtoSceneryEntry(items[0],polygons,vertices,pad,eid);
        }
    }
}
