using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash1BetaMAR08)]
    [EntryType(3,GameVersion.Crash1BetaMAY11)]
    [EntryType(3,GameVersion.Crash1)]
    public sealed class OldSceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 3 && items.Length != 4)
            {
                ErrorManager.SignalError("OldSceneryEntry: Wrong number of items");
            }
            if (items[1].Length % 8 != 0)
            {
                ErrorManager.SignalError("OldSceneryEntry: Second item (polygons) length is invalid");
            }
            OldSceneryPolygon[] polygons = new OldSceneryPolygon [items[1].Length / 8];
            for (int i = 0;i < polygons.Length;i++)
            {
                byte[] polygondata = new byte [8];
                Array.Copy(items[1],i * 8,polygondata,0,8);
                polygons[i] = OldSceneryPolygon.Load(polygondata);
            }
            if (items[2].Length % 8 != 0)
            {
                ErrorManager.SignalError("OldSceneryEntry: Third item (vertices) length is invalid");
            }
            OldSceneryVertex[] vertices = new OldSceneryVertex [items[2].Length / 8];
            for (int i = 0;i < vertices.Length;i++)
            {
                byte[] vertexdata = new byte [8];
                Array.Copy(items[2],i * 8,vertexdata,0,8);
                vertices[i] = OldSceneryVertex.Load(vertexdata);
            }
            byte[] extradata = null;
            if (items.Length >= 4)
            {
                extradata = items[3];
            }
            return new OldSceneryEntry(items[0],polygons,vertices,extradata,eid,size);
        }
    }
}
