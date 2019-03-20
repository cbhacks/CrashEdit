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
            int vertexcount = BitConv.FromInt32(items[0],16);
            SceneryVertex[] vertices = new SceneryVertex [vertexcount];
            for (int i = 0;i < vertexcount;i++)
            {
                byte[] xydata = new byte [4];
                byte[] zdata = new byte [2];
                Array.Copy(items[1],(vertexcount - 1 - i) * 4,xydata,0,xydata.Length);
                Array.Copy(items[1],vertexcount * 4 + i * 2,zdata,0,zdata.Length);
                vertices[i] = SceneryVertex.Load(xydata,zdata);
            }
            int trianglecount = BitConv.FromInt32(items[0],20);
            SceneryTriangle[] triangles = new SceneryTriangle [trianglecount];
            for (int i = 0;i < trianglecount;i++)
            {
                byte[] adata = new byte [4];
                byte[] bdata = new byte [2];
                Array.Copy(items[2],(trianglecount - 1 - i) * 4,adata,0,adata.Length);
                Array.Copy(items[2],trianglecount * 4 + i * 2,bdata,0,bdata.Length);
                triangles[i] = SceneryTriangle.Load(adata,bdata);
            }
            int quadcount = BitConv.FromInt32(items[0],24);
            SceneryQuad[] quads = new SceneryQuad [quadcount];
            for (int i = 0;i < quadcount;i++)
            {
                byte[] quaddata = new byte [8];
                Array.Copy(items[3],i * 8,quaddata,0,quaddata.Length);
                quads[i] = SceneryQuad.Load(quaddata);
            }
            int texturecount = BitConv.FromInt32(items[0],28);
            //if (items[3].Length != texturecount * 12)
            //{
            //    ErrorManager.SignalError("ModelEntry: Texture count mismatch");
            //}
            ModelTexture[] textures = new ModelTexture[texturecount];
            for (int i = 0; i < texturecount; i++)
            {
                byte[] texturedata = new byte[12];
                Array.Copy(items[4], i * 12, texturedata, 0, texturedata.Length);
                textures[i] = ModelTexture.Load(texturedata);
            }
            int colorcount = BitConv.FromInt32(items[0],32);
            SceneryColor[] colors = new SceneryColor [colorcount];
            for (int i = 0;i < colorcount;i++)
            {
                byte red = items[5][i * 4];
                byte green = items[5][i * 4 + 1];
                byte blue = items[5][i * 4 + 2];
                byte extra = items[5][i * 4 + 3];
                colors[i] = new SceneryColor(red,green,blue,extra);
            }
            int animatedtexturecount = BitConv.FromInt32(items[0],36);
            //if (items[4].Length != animatedtexturecount * 4)
            //{
            //    ErrorManager.SignalError("ModelEntry: Animated texture count mismatch");
            //}
            ModelAnimatedTexture[] animatedtextures = new ModelAnimatedTexture[animatedtexturecount];
            for (int i = 0; i < animatedtexturecount; i++)
            {
                byte[] animatedtexturedata = new byte[4];
                Array.Copy(items[4], i * 4, animatedtexturedata, 0, animatedtexturedata.Length);
                animatedtextures[i] = ModelAnimatedTexture.Load(animatedtexturedata);
            }
            return new SceneryEntry(items[0],vertices,triangles,quads,textures,colors,animatedtextures,eid,size);
        }
    }
}
