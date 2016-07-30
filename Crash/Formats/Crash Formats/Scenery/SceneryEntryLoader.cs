using System;

namespace Crash
{
    [EntryType(3,GameVersion.Crash2)]
    public sealed class SceneryEntryLoader : EntryLoader
    {
        public override Entry Load(byte[][] items,int eid,int size)
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
            // TODO :: Get trianglecount from info
            int trianglecount = items[2].Length / 6;
            // TODO :: Check triangle list size
            SceneryTriangle[] triangles = new SceneryTriangle [trianglecount];
            for (int i = 0;i < trianglecount;i++)
            {
                byte[] adata = new byte [4];
                byte[] bdata = new byte [2];
                Array.Copy(items[2],(trianglecount - 1 - i) * 4,adata,0,adata.Length);
                Array.Copy(items[2],trianglecount * 4 + i * 2,bdata,0,bdata.Length);
                triangles[i] = SceneryTriangle.Load(adata,bdata);
            }
            // TODO :: Get quadcount from info
            int quadcount = items[3].Length / 8;
            // TODO :: Check quad list size
            SceneryQuad[] quads = new SceneryQuad [quadcount];
            for (int i = 0;i < quadcount;i++)
            {
                byte[] quaddata = new byte [8];
                Array.Copy(items[3],i * 8,quaddata,0,quaddata.Length);
                quads[i] = SceneryQuad.Load(quaddata);
            }
            // TODO :: Get colorcount from info
            int colorcount = items[5].Length / 4;
            // TODO :: Check color list size
            SceneryColor[] colors = new SceneryColor [colorcount];
            for (int i = 0;i < colorcount;i++)
            {
                byte red = items[5][i * 4];
                byte green = items[5][i * 4 + 1];
                byte blue = items[5][i * 4 + 2];
                byte extra = items[5][i * 4 + 3];
                colors[i] = new SceneryColor(red,green,blue,extra);
            }
            SceneryColorList[] colorlist = new SceneryColorList[1];
            for (int i = 0; i < 1; i++)
            {
                colorlist[i] = SceneryColorList.Load(items[5]);
            }
            return new SceneryEntry(items[0],vertices,triangles,quads,items[4],colors,colorlist,items[6],eid,size);
        }
    }
}
