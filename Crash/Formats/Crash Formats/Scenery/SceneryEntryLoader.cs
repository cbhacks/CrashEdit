using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash2)]
    [EntryType(3,GameVersion.Crash3)]
    public sealed class SceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Length != 7)
            {
                ErrorManager.SignalError("SceneryEntry: Wrong number of items");
            }
            if (items[0].Length != 76)
            {
                ErrorManager.SignalError("SceneryEntry: First item length is wrong");
            }
            // TODO :: Get vertexcount from info
            int vertexcount = items[1].Length / 6;
            // TODO :: Check vertex list size
            SceneryVertex[] vertices = new SceneryVertex [vertexcount];
            for (int i = 0;i < vertexcount;i++)
            {
                byte[] xydata = new byte [4];
                byte[] zdata = new byte [2];
                Array.Copy(items[1],(vertexcount - 1 - i) * 4,xydata,0,xydata.Length);
                Array.Copy(items[1],vertexcount * 4 + i * 2,zdata,0,zdata.Length);
                vertices[i] = SceneryVertex.Load(xydata,zdata);
            }
            // TODO :: Get polygoncount from info
            int polygoncount = items[3].Length / 8;
            // TODO :: Check polygon list size
            SceneryPolygon[] polygons = new SceneryPolygon [polygoncount];
            for (int i = 0;i < polygoncount;i++)
            {
                byte[] polygondata = new byte [8];
                Array.Copy(items[3],i * 8,polygondata,0,polygondata.Length);
                polygons[i] = SceneryPolygon.Load(polygondata);
            }
            return new SceneryEntry(items[0],vertices,items[2],polygons,items[4],items[5],items[6],eid);
        }
    }
}
