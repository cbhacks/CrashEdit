using Crash;
using System;
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

        private static long textureframe; // CrashEdit would have to run for so long for this to overflow
        private static Timer texturetimer;

        private int[][] entrytextures;
        private List<SceneryTriangle>[] dyntris;
        private List<SceneryQuad>[] dynquads;

        static SceneryEntryViewer()
        {
            texturetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate() / 2,
                Enabled = true
            };
            texturetimer.Tick += delegate (object sender, EventArgs e)
            {
                ++textureframe;
                texturetimer.Interval = 1000 / OldMainForm.GetRate() / 2;
            };
        }

        public SceneryEntryViewer(SceneryEntry entry,TextureChunk[] texturechunks)
        {
            entries = new List<SceneryEntry>();
            entries.Add(entry);
            displaylist = -1;
            entrytextures = new int[1][];
            textureframe = 0;
            dyntris = new List<SceneryTriangle>[1] { new List<SceneryTriangle>() };
            dynquads = new List<SceneryQuad>[1] { new List<SceneryQuad>() };
            ConvertTexturesToGL(texturechunks, entry.Textures, entry.Info, 0x2C);
            entrytextures[0] = textures;
        }

        public SceneryEntryViewer(IEnumerable<SceneryEntry> entries,TextureChunk[][] texturechunks)
        {
            this.entries = new List<SceneryEntry>(entries);
            displaylist = -1;
            entrytextures = new int[this.entries.Count][];
            textureframe = 0;
            dyntris = new List<SceneryTriangle>[this.entries.Count];
            dynquads = new List<SceneryQuad>[this.entries.Count];
            for (int i = 0; i < this.entries.Count; ++i)
            {
                dyntris[i] = new List<SceneryTriangle>();
                dynquads[i] = new List<SceneryQuad>();
                ConvertTexturesToGL(texturechunks[i], this.entries[i].Textures, this.entries[i].Info, 0x2C);
                entrytextures[i] = textures;
            }
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
                    if (textures_enabled)
                        GL.Disable(EnableCap.Texture2D);
                    else
                        GL.Enable(EnableCap.Texture2D);
                    textures_enabled = !textures_enabled;
                    break;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (int)TextureEnvMode.Combine);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.CombineRgb, (int)TextureEnvModeCombine.Modulate);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 2.0f);
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
                        if (textures_enabled)
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
                            float[] uvs = new float[8];
                            for (int i = 0; i < entry.Quads.Count; ++i)
                            {
                                var q = entry.Quads[i];
                                if (q.VertexA >= entry.Vertices.Count || q.VertexB >= entry.Vertices.Count || q.VertexC >= entry.Vertices.Count || q.VertexD >= entry.Vertices.Count) continue;
                                if (q.Texture != 0 || q.Animated)
                                {
                                    int tex = q.Texture - 1;
                                    if (!q.Animated)
                                        GL.BindTexture(TextureTarget.Texture2D, entrytextures[e][tex]);
                                    else
                                    {
                                        ++tex;
                                        var anim = entry.AnimatedTextures[tex];
                                        if (anim.Offset == 0)
                                            GL.BindTexture(TextureTarget.Texture2D, 0);
                                        else if (anim.IsLOD)
                                        {
                                            tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                                            GL.BindTexture(TextureTarget.Texture2D, entrytextures[e][tex]);
                                        }
                                        else
                                        {
                                            dynquads[e].Add(q);
                                            continue;
                                        }
                                    }
                                    uvs[0] = entry.Textures[tex].X2;
                                    uvs[1] = entry.Textures[tex].Y2;
                                    uvs[2] = entry.Textures[tex].X1;
                                    uvs[3] = entry.Textures[tex].Y1;
                                    uvs[4] = entry.Textures[tex].X3;
                                    uvs[5] = entry.Textures[tex].Y3;
                                    uvs[6] = entry.Textures[tex].X4;
                                    uvs[7] = entry.Textures[tex].Y4;
                                }
                                else
                                    GL.BindTexture(TextureTarget.Texture2D, 0);
                                GL.Begin(PrimitiveType.Quads);
                                GL.TexCoord2(uvs[0], uvs[1]);
                                RenderVertex(entry, entry.Vertices[q.VertexA]);
                                GL.TexCoord2(uvs[2], uvs[3]);
                                RenderVertex(entry, entry.Vertices[q.VertexB]);
                                GL.TexCoord2(uvs[4], uvs[5]);
                                RenderVertex(entry, entry.Vertices[q.VertexC]);
                                GL.TexCoord2(uvs[6], uvs[7]);
                                RenderVertex(entry, entry.Vertices[q.VertexD]);
                                GL.End();
                            }
                            GL.BindTexture(TextureTarget.Texture2D, 0);
                        }
                        else
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
            for (int i = 0; i < dynquads.Length; ++i)
            {
                foreach (SceneryQuad quad in dynquads[i])
                {
                    SceneryEntry entry = entries[i];
                    ModelExtendedTexture anim = entry.AnimatedTextures[quad.Texture];
                    int tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                    if (anim.Leap)
                    {
                        ++tex;
                        if (!entry.AnimatedTextures[tex].IsLOD) System.Diagnostics.Debugger.Break();
                        tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                    }
                    GL.BindTexture(TextureTarget.Texture2D, entrytextures[i][tex]);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(entry.Textures[tex].X2, entry.Textures[tex].Y2);
                    RenderVertex(entry, entry.Vertices[quad.VertexA]);
                    GL.TexCoord2(entry.Textures[tex].X1, entry.Textures[tex].Y1);
                    RenderVertex(entry, entry.Vertices[quad.VertexB]);
                    GL.TexCoord2(entry.Textures[tex].X3, entry.Textures[tex].Y3);
                    RenderVertex(entry, entry.Vertices[quad.VertexC]);
                    GL.TexCoord2(entry.Textures[tex].X4, entry.Textures[tex].Y4);
                    RenderVertex(entry, entry.Vertices[quad.VertexD]);
                    GL.End();
                }
            }
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void RenderVertex(SceneryEntry entry,SceneryVertex vertex)
        {
            SceneryColor color = entry.Colors[vertex.Color];
            GL.Color3(color.Red,color.Green,color.Blue);
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
                    entrytextures[i] = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
