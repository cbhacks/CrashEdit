using Crash;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public class ProtoSceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<ProtoSceneryEntry> entries;
        private int displaylist;
        private bool textures_enabled = true;
        private bool init = false;

        private TextureChunk[][] texturechunks;
        public ProtoSceneryEntryViewer(ProtoSceneryEntry entry,TextureChunk[] texturechunks)
        {
            entries = new List<ProtoSceneryEntry> { entry };
            displaylist = -1;
            InitTextures(1);
            this.texturechunks = new TextureChunk[1][];
            this.texturechunks[0] = texturechunks;
        }

        public ProtoSceneryEntryViewer(IEnumerable<ProtoSceneryEntry> entries,TextureChunk[][] texturechunks)
        {
            this.entries = new List<ProtoSceneryEntry>(entries);
            displaylist = -1;
            InitTextures(this.entries.Count);
            this.texturechunks = texturechunks;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (ProtoSceneryEntry entry in entries)
                {
                    foreach (ProtoSceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(entry.XOffset + vertex.X,entry.YOffset + vertex.Y,entry.ZOffset + vertex.Z);
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.T:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.T:
                    textures_enabled = !textures_enabled;
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 2.0f);
        }

        protected override void RenderObjects()
        {
            if (!init)
            {
                init = true;
                for (int i = 0; i < entries.Count; ++i)
                {
                    ConvertTexturesToGL(i,texturechunks[i],entries[i].Polygons,entries[i].Structs);
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            if (!textures_enabled)
                GL.Disable(EnableCap.Texture2D);
            else
                GL.Enable(EnableCap.Texture2D);
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist,ListMode.CompileAndExecute);
                for (int e = 0; e < entries.Count; e++)
                {
                    ProtoSceneryEntry entry = entries[e];
                    if (entry != null)
                    {
                        for (int p = 0; p < entry.Polygons.Count; p++)
                        {
                            ProtoSceneryPolygon polygon = entry.Polygons[p];
                            if (polygon.VertexA >= entry.Vertices.Count || polygon.VertexB >= entry.Vertices.Count || polygon.VertexC >= entry.Vertices.Count) continue;
                            OldModelStruct modelstruct = entry.Structs[polygon.Texture];
                            if (modelstruct is OldSceneryTexture tex)
                            { 
                                BindTexture(e,p);
                                GL.Color3(tex.R,tex.G,tex.B);
                                GL.Begin(PrimitiveType.Triangles);
                                GL.TexCoord2(tex.X3,tex.Y3);
                                RenderVertex(entry,entry.Vertices[polygon.VertexA]);
                                GL.TexCoord2(tex.X2,tex.Y2);
                                RenderVertex(entry,entry.Vertices[polygon.VertexB]);
                                GL.TexCoord2(tex.X1,tex.Y1);
                                RenderVertex(entry,entry.Vertices[polygon.VertexC]);
                                GL.End();
                            }
                            else
                            {
                                UnbindTexture();
                                OldSceneryColor col = (OldSceneryColor)modelstruct;
                                GL.Color3(col.R,col.G,col.B);
                                GL.Begin(PrimitiveType.Triangles);
                                RenderVertex(entry,entry.Vertices[polygon.VertexA]);
                                RenderVertex(entry,entry.Vertices[polygon.VertexB]);
                                RenderVertex(entry,entry.Vertices[polygon.VertexC]);
                                GL.End();
                            }
                        }
                    }
                }
                GL.EndList();
            }
            else
            {
                GL.CallList(displaylist);
            }
            GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
        }

        private void RenderVertex(ProtoSceneryEntry entry, ProtoSceneryVertex vertex)
        {
            GL.Vertex3(entry.XOffset + vertex.X, entry.YOffset + vertex.Y, entry.ZOffset + vertex.Z);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
