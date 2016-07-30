using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1Beta1995)]
    public sealed class ProtoSceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3)
            {
                ErrorManager.SignalError("ProtoSceneryEntry: Wrong number of items");
            }
            if (items[1].Length % 12 != 0)
            {
                ErrorManager.SignalError("ProtoSceneryEntry: Second item (polygons) length is invalid");
            }
            ProtoSceneryPolygon[] polygons = new ProtoSceneryPolygon[items[1].Length / 12];
            for (int i = 0;i < polygons.Length;i++)
            {
                byte[] polygondata = new byte [12];
                Array.Copy(items[1],i * 12,polygondata,0,12);
                polygons[i] = ProtoSceneryPolygon.Load(polygondata);
            }
            for (int i = 0;i < polygons.Length;i++)
            {
                if (polygons[i].VertexA > (items[2].Length - (items[2].Length % 6)) / 6 || polygons[i].VertexB > (items[2].Length - (items[2].Length % 6)) / 6 || polygons[i].VertexC > (items[2].Length - (items[2].Length % 6)) / 6) //LMAO
                {
                    polygons[i].VertexA = 0;
                    polygons[i].VertexB = 0;
                    polygons[i].VertexC = 0;
                }
            }
            if (items[2].Length % 4 != 0)
            {
                ErrorManager.SignalError("ProtoSceneryEntry: Third item (vertices) length is invalid");
            }
            ProtoSceneryVertex[] vertices = new ProtoSceneryVertex[(items[2].Length - (items[2].Length % 6)) / 6];
            for (int i = 0;i < vertices.Length;i++)
            {
                byte[] vertexdata = new byte [6];
                Array.Copy(items[2],i * 6,vertexdata,0,6);
                vertices[i] = ProtoSceneryVertex.Load(vertexdata);
            }
            byte[] extradata = null;
            if (items.Length >= 4)
            {
                extradata = items[3];
            }
            return new ProtoSceneryEntry(items[0],polygons,vertices,extradata,eid,size);
        }
    }
}
