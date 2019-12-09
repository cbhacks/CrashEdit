using Crash;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public class SceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<SceneryEntry> entries;
        private int displaylist;
        private bool textures_enabled = true;

        private int[][] entrytextures;

        public SceneryEntryViewer(SceneryEntry entry,TextureChunk[] texturechunks)
        {
            entries = new List<SceneryEntry>();
            entries.Add(entry);
            entrytextures = new int[1][];
            //ConvertTexturesToGL(texturechunks, entry.Textures, entry.Info, 0x2C);
            entrytextures[0] = textures;
            displaylist = -1;
        }

        public SceneryEntryViewer(IEnumerable<SceneryEntry> entries,TextureChunk[][] texturechunks)
        {
            this.entries = new List<SceneryEntry>(entries);
            entrytextures = new int[this.entries.Count][];
            for (int i = 0; i < entrytextures.Length; ++i)
            {
                //ConvertTexturesToGL(texturechunks[i], this.entries[i].Textures, this.entries[i].Info, 0x2C);
                entrytextures[i] = textures;
            }
            displaylist = -1;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (SceneryEntry entry in entries)
                {
                    foreach (SceneryVertex vertex in entry.Vertices)
                    {
                        yield return new Position(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
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

        protected override void RenderObjects()
        {
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist,ListMode.CompileAndExecute);
                for (int e = 0; e < entries.Count; ++e)
                {
                    SceneryEntry entry = entries[e];
                    if (entry != null)
                    {
                        //if (textures_enabled)
                        //{
                        //    GL.Begin(PrimitiveType.Triangles);
                        //    foreach (SceneryTriangle triangle in entry.Triangles)
                        //    {
                        //        if (triangle.VertexA < entry.Vertices.Count && triangle.VertexB < entry.Vertices.Count && triangle.VertexC < entry.Vertices.Count)
                        //        {
                        //            RenderVertex(entry, entry.Vertices[triangle.VertexA]);
                        //            RenderVertex(entry, entry.Vertices[triangle.VertexB]);
                        //            RenderVertex(entry, entry.Vertices[triangle.VertexC]);
                        //        }
                        //    }
                        //    GL.End();
                        //    float[] uvs = new float[8];
                        //    for (int i = 0; i < entry.Quads.Count; ++i)
                        //    {
                        //        var q = entry.Quads[i];
                        //        if (q.VertexA >= entry.Vertices.Count || q.VertexB >= entry.Vertices.Count || q.VertexC >= entry.Vertices.Count || q.VertexD >= entry.Vertices.Count) continue;
                        //        if (q.Texture != 0 && false)
                        //        {
                        //            //GL.Enable(EnableCap.Texture2D);
                        //            int tex = q.Texture - 1;
                        //            if (!q.Animated)
                        //                GL.BindTexture(TextureTarget.Texture2D, entrytextures[e][tex]);
                        //            //else
                        //            //{
                        //            //    var anim = model.AnimatedTextures[tex];
                        //            //    if (!anim.IsLOD && !anim.Leap && anim.Offset != 0)
                        //            //    {
                        //            //        tex = anim.Offset - 1 + (textureframe & anim.Mask);
                        //            //        GL.BindTexture(TextureTarget.Texture2D, entrytextures[e][tex]);
                        //            //    }
                        //            //    else
                        //            //        GL.BindTexture(TextureTarget.Texture2D, 0);
                        //            //}
                        //            uvs[0] = entry.Textures[tex].X1;
                        //            uvs[1] = entry.Textures[tex].Y1;
                        //            uvs[2] = entry.Textures[tex].X2;
                        //            uvs[3] = entry.Textures[tex].Y2;
                        //            uvs[4] = entry.Textures[tex].X3;
                        //            uvs[5] = entry.Textures[tex].Y3;
                        //            uvs[6] = entry.Textures[tex].X4;
                        //            uvs[7] = entry.Textures[tex].Y4;
                        //        }
                        //        else
                        //            GL.BindTexture(TextureTarget.Texture2D, 0);
                        //        GL.Begin(PrimitiveType.Quads);
                        //        GL.TexCoord2(uvs[0], uvs[1]);
                        //        RenderVertex(entry, entry.Vertices[q.VertexA]);
                        //        GL.TexCoord2(uvs[2], uvs[3]);
                        //        RenderVertex(entry, entry.Vertices[q.VertexB]);
                        //        GL.TexCoord2(uvs[4], uvs[5]);
                        //        RenderVertex(entry, entry.Vertices[q.VertexC]);
                        //        GL.TexCoord2(uvs[6], uvs[7]);
                        //        RenderVertex(entry, entry.Vertices[q.VertexD]);
                        //        GL.End();
                        //    }
                        //}
                        //else
                        {
                            GL.Begin(PrimitiveType.Triangles);
                            foreach (SceneryTriangle triangle in entry.Triangles)
                            {
                                if (triangle.VertexA < entry.Vertices.Count && triangle.VertexB < entry.Vertices.Count && triangle.VertexC < entry.Vertices.Count)
                                {
                                    RenderVertex(entry, entry.Vertices[triangle.VertexA]);
                                    RenderVertex(entry, entry.Vertices[triangle.VertexB]);
                                    RenderVertex(entry, entry.Vertices[triangle.VertexC]);
                                }
                            }
                            GL.End();
                            GL.Begin(PrimitiveType.Quads);
                            foreach (SceneryQuad quad in entry.Quads)
                            {
                                if (quad.VertexA < entry.Vertices.Count && quad.VertexB < entry.Vertices.Count && quad.VertexC < entry.Vertices.Count && quad.VertexD < entry.Vertices.Count)
                                {
                                    RenderVertex(entry, entry.Vertices[quad.VertexA]);
                                    RenderVertex(entry, entry.Vertices[quad.VertexB]);
                                    RenderVertex(entry, entry.Vertices[quad.VertexC]);
                                    RenderVertex(entry, entry.Vertices[quad.VertexD]);
                                }
                            }
                            GL.End();
                        }
                    }
                }
                GL.EndList();
            }
            else
            {
                GL.CallList(displaylist);
            }
        }

        private void RenderVertex(SceneryEntry entry,SceneryVertex vertex)
        {
            if (vertex.Color < entry.Colors.Count)
            {
                SceneryColor color = entry.Colors[vertex.Color];
                GL.Color3(color.Red,color.Green,color.Blue);
            }
            else
            {
                throw new System.Exception("Vertex color outside color range.");
                //GL.Color3(Color.Fuchsia);
            }
            GL.Vertex3(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
        }

        protected override void Dispose(bool disposing)
        {
            if (entrytextures != null)
            {
                for (int i = 0; i < entrytextures.Length; ++i)
                {
                    if (entrytextures[i] == null) continue;
                    GL.DeleteTextures(entrytextures[i].Length, entrytextures[i]);
                }
            }
            base.Dispose(disposing);
        }
    }
}
