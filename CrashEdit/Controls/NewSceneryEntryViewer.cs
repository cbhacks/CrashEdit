using Crash;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public class NewSceneryEntryViewer : ThreeDimensionalViewer
    {
        private List<NewSceneryEntry> entries;
        private int displaylist;
        private int displaylist_translucid;
        private bool textures_enabled = true;
        private bool init = false;

        private static long textureframe; // CrashEdit would have to run for so long for this to overflow
        private static Timer texturetimer;

        private TextureChunk[][] texturechunks;
        private List<SceneryTriangle>[] dyntris;
        private List<SceneryQuad>[] dynquads;
        private List<SceneryTriangle>[] lasttris;
        private List<SceneryQuad>[] lastquads;

        static NewSceneryEntryViewer()
        {
            textureframe = 0;
            texturetimer = new Timer
            {
                Interval = 1000 / OldMainForm.GetRate(),
                Enabled = true
            };
            texturetimer.Tick += delegate (object sender, EventArgs e)
            {
                ++textureframe;
                texturetimer.Interval = 1000 / OldMainForm.GetRate();
            };
        }

        public NewSceneryEntryViewer(NewSceneryEntry entry,TextureChunk[] texturechunks)
        {
            entries = new List<NewSceneryEntry>() { entry };
            displaylist = -1;
            displaylist_translucid = -1;
            InitTextures(1);
            this.texturechunks = new TextureChunk[1][];
            dyntris = new List<SceneryTriangle>[1] { new List<SceneryTriangle>() };
            dynquads = new List<SceneryQuad>[1] { new List<SceneryQuad>() };
            lasttris = new List<SceneryTriangle>[1] { new List<SceneryTriangle>() };
            lastquads = new List<SceneryQuad>[1] { new List<SceneryQuad>() };
            this.texturechunks[0] = texturechunks;
        }

        public NewSceneryEntryViewer(IEnumerable<NewSceneryEntry> entries,TextureChunk[][] texturechunks)
        {
            this.entries = new List<NewSceneryEntry>(entries);
            displaylist = -1;
            displaylist_translucid = -1;
            InitTextures(this.entries.Count);
            this.texturechunks = texturechunks;
            dyntris = new List<SceneryTriangle>[this.entries.Count];
            dynquads = new List<SceneryQuad>[this.entries.Count];
            lasttris = new List<SceneryTriangle>[this.entries.Count];
            lastquads = new List<SceneryQuad>[this.entries.Count];
            for (int i = 0; i < this.entries.Count; ++i)
            {
                dyntris[i] = new List<SceneryTriangle>();
                dynquads[i] = new List<SceneryQuad>();
                lasttris[i] = new List<SceneryTriangle>();
                lastquads[i] = new List<SceneryQuad>();
            }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (NewSceneryEntry entry in entries)
                {
                    foreach (NewSceneryVertex vertex in entry.Vertices)
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
                    ConvertTexturesToGL(i, texturechunks[i], entries[i].Textures, entries[i].Info, 0x2C);
                }
            }

            GL.BindTexture(TextureTarget.Texture2D, 0);
            if (!textures_enabled)
                GL.Disable(EnableCap.Texture2D);
            else
                GL.Enable(EnableCap.Texture2D);
            double[] uvs = new double[8];
            if (displaylist == -1)
            {
                displaylist = GL.GenLists(1);
                GL.NewList(displaylist,ListMode.CompileAndExecute);
                for (int e = 0; e < entries.Count; ++e)
                {
                    dyntris[e].Clear();
                    dynquads[e].Clear();
                    lasttris[e].Clear();
                    lastquads[e].Clear();
                    NewSceneryEntry entry = entries[e];
                    for (int i = 0; i < entry.Triangles.Count; ++i)
                    {
                        var t = entry.Triangles[i];
                        if ((t.VertexA >= entry.Vertices.Count || t.VertexB >= entry.Vertices.Count || t.VertexC >= entry.Vertices.Count) || (t.VertexA == t.VertexB && t.VertexB == t.VertexC && t.VertexC == t.VertexA)) continue;
                        if (t.Texture != 0 || t.Animated)
                        {
                            bool untex = false;
                            int tex = t.Texture - 1;
                            if (t.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                if (anim.Offset == 0)
                                    untex = true;
                                else if (anim.IsLOD)
                                {
                                    tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                                }
                                else
                                {
                                    if (anim.Leap)
                                    {
                                        ++tex;
                                        anim = entry.AnimatedTextures[tex];
                                        tex = anim.Offset - 1 + anim.LOD0;
                                    }
                                    if (entry.Textures[tex].BlendMode == 1)
                                        lasttris[e].Add(t);
                                    else
                                        dyntris[e].Add(t);
                                    continue;
                                }
                            }
                            if (untex)
                            {
                                UnbindTexture();
                            }
                            else
                            {
                                if (entry.Textures[tex].BlendMode == 1)
                                {
                                    lasttris[e].Add(t);
                                    continue;
                                }
                                else if (entry.Textures[tex].BlendMode == 0)
                                {
                                    continue;
                                }
                                BindTexture(e, tex);
                                uvs[0] = entry.Textures[tex].X2;
                                uvs[1] = entry.Textures[tex].Y2;
                                uvs[2] = entry.Textures[tex].X1;
                                uvs[3] = entry.Textures[tex].Y1;
                                uvs[4] = entry.Textures[tex].X3;
                                uvs[5] = entry.Textures[tex].Y3;
                                //SetBlendMode(entry.Textures[tex].BlendMode);
                            }
                        }
                        else
                            UnbindTexture();
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(uvs[0], uvs[1]);
                        RenderVertex(entry, entry.Vertices[t.VertexA]);
                        GL.TexCoord2(uvs[2], uvs[3]);
                        RenderVertex(entry, entry.Vertices[t.VertexB]);
                        GL.TexCoord2(uvs[4], uvs[5]);
                        RenderVertex(entry, entry.Vertices[t.VertexC]);
                        GL.End();
                    }
                    for (int i = 0; i < entry.Quads.Count; ++i)
                    {
                        var q = entry.Quads[i];
                        if ((q.VertexA >= entry.Vertices.Count || q.VertexB >= entry.Vertices.Count || q.VertexC >= entry.Vertices.Count || q.VertexD >= entry.Vertices.Count) || (q.VertexA == q.VertexB && q.VertexB == q.VertexC && q.VertexC == q.VertexD && q.VertexD == q.VertexA)) continue;
                        if (q.Texture != 0 || q.Animated)
                        {
                            bool untex = false;
                            int tex = q.Texture - 1;
                            if (q.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                if (anim.Offset == 0)
                                    untex = true;
                                else if (anim.IsLOD)
                                {
                                    tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                                }
                                else
                                {
                                    if (anim.Leap)
                                    {
                                        ++tex;
                                        anim = entry.AnimatedTextures[tex];
                                        // tex = anim.Offset - 1 + anim.LOD0;
                                        tex = anim.Offset - 1 + (anim.Data >> 29 & 0x3);
                                    }
                                    if (entry.Textures[tex].BlendMode == 1)
                                        lastquads[e].Add(q);
                                    else
                                        dynquads[e].Add(q);
                                    continue;
                                }
                            }
                            if (untex)
                            {
                                UnbindTexture();
                            }
                            else
                            {
                                if (entry.Textures[tex].BlendMode == 1)
                                {
                                    lastquads[e].Add(q);
                                    continue;
                                }
                                else if (entry.Textures[tex].BlendMode == 0)
                                {
                                    continue;
                                }
                                BindTexture(e, tex);
                                uvs[0] = entry.Textures[tex].X2;
                                uvs[1] = entry.Textures[tex].Y2;
                                uvs[2] = entry.Textures[tex].X1;
                                uvs[3] = entry.Textures[tex].Y1;
                                uvs[4] = entry.Textures[tex].X3;
                                uvs[5] = entry.Textures[tex].Y3;
                                uvs[6] = entry.Textures[tex].X4;
                                uvs[7] = entry.Textures[tex].Y4;
                                //SetBlendMode(entry.Textures[tex].BlendMode);
                            }
                        }
                        else
                            UnbindTexture();
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
                }
                GL.EndList();
                UnbindTexture();
                SetBlendMode(3);
            }
            else
            {
                GL.CallList(displaylist);
            }
            for (int i = 0; i < dyntris.Length; ++i)
            {
                NewSceneryEntry entry = entries[i];
                List<SceneryTriangle> fakes = new List<SceneryTriangle>();
                foreach (SceneryTriangle tri in dyntris[i])
                {
                    ModelExtendedTexture anim = entry.AnimatedTextures[tri.Texture];
                    int tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                    if (anim.Leap)
                    {
                        ++tex;
                        if (!entry.AnimatedTextures[tex].IsLOD) System.Diagnostics.Debugger.Break();
                        tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                    }
                    if (entry.Textures[tex].BlendMode == 1)
                    {
                        fakes.Add(tri);
                        continue;
                    }
                    BindTexture(i,tex);
                    SetBlendMode(entry.Textures[tex].BlendMode);
                    GL.Begin(PrimitiveType.Triangles);
                    GL.TexCoord2(entry.Textures[tex].X2, entry.Textures[tex].Y2);
                    RenderVertex(entry, entry.Vertices[tri.VertexA]);
                    GL.TexCoord2(entry.Textures[tex].X1, entry.Textures[tex].Y1);
                    RenderVertex(entry, entry.Vertices[tri.VertexB]);
                    GL.TexCoord2(entry.Textures[tex].X3, entry.Textures[tex].Y3);
                    RenderVertex(entry, entry.Vertices[tri.VertexC]);
                    GL.End();
                }
                foreach (var fake in fakes)
                {
                    dyntris[i].Remove(fake);
                    lasttris[i].Add(fake);
                }
            }
            for (int i = 0; i < dynquads.Length; ++i)
            {
                NewSceneryEntry entry = entries[i];
                List<SceneryQuad> fakes = new List<SceneryQuad>();
                foreach (SceneryQuad quad in dynquads[i])
                {
                    ModelExtendedTexture anim = entry.AnimatedTextures[quad.Texture];
                    int tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                    if (anim.Leap)
                    {
                        ++tex;
                        if (!entry.AnimatedTextures[tex].IsLOD) System.Diagnostics.Debugger.Break();
                        tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                    }
                    if (entry.Textures[tex].BlendMode == 1)
                    {
                        fakes.Add(quad);
                        continue;
                    }
                    BindTexture(i,tex);
                    SetBlendMode(entry.Textures[tex].BlendMode);
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
                foreach (var fake in fakes)
                {
                    dynquads[i].Remove(fake);
                    lastquads[i].Add(fake);
                }
            }
            SetBlendMode(1);
            GL.DepthMask(false);
            for (int i = 0; i < lasttris.Length; ++i)
            {
                NewSceneryEntry entry = entries[i];
                foreach (SceneryTriangle t in lasttris[i])
                {
                    bool untex = false;
                    int tex = t.Texture - 1;
                    if (t.Animated)
                    {
                        ++tex;
                        var anim = entry.AnimatedTextures[tex];
                        if (anim.Offset == 0)
                            untex = true;
                        else if (anim.IsLOD)
                        {
                            tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                        }
                        else
                        {
                            tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                            if (anim.Leap)
                            {
                                ++tex;
                                if (!entry.AnimatedTextures[tex].IsLOD) System.Diagnostics.Debugger.Break();
                                tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                            }
                        }
                    }
                    if (untex)
                    {
                        UnbindTexture();
                    }
                    else
                    {
                        BindTexture(i,tex);
                        uvs[0] = entry.Textures[tex].X2;
                        uvs[1] = entry.Textures[tex].Y2;
                        uvs[2] = entry.Textures[tex].X1;
                        uvs[3] = entry.Textures[tex].Y1;
                        uvs[4] = entry.Textures[tex].X3;
                        uvs[5] = entry.Textures[tex].Y3;
                    }
                    GL.Begin(PrimitiveType.Triangles);
                    GL.TexCoord2(entry.Textures[tex].X2,entry.Textures[tex].Y2);
                    RenderVertex(entry,entry.Vertices[t.VertexA],0x7F);
                    GL.TexCoord2(entry.Textures[tex].X1,entry.Textures[tex].Y1);
                    RenderVertex(entry,entry.Vertices[t.VertexB],0x7F);
                    GL.TexCoord2(entry.Textures[tex].X3,entry.Textures[tex].Y3);
                    RenderVertex(entry,entry.Vertices[t.VertexC],0x7F);
                    GL.End();
                }
            }
            for (int i = 0; i < lastquads.Length; ++i)
            {
                NewSceneryEntry entry = entries[i];
                foreach (SceneryQuad q in lastquads[i])
                {
                    bool untex = false;
                    int tex = q.Texture - 1;
                    if (q.Animated)
                    {
                        ++tex;
                        var anim = entry.AnimatedTextures[tex];
                        if (anim.Offset == 0)
                            untex = true;
                        else if (anim.IsLOD)
                        {
                            tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                        }
                        else
                        {
                            tex = anim.Offset - 1 + (int)((textureframe / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                            if (anim.Leap)
                            {
                                ++tex;
                                if (!entry.AnimatedTextures[tex].IsLOD) System.Diagnostics.Debugger.Break();
                                tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                            }
                        }
                    }
                    if (untex)
                    {
                        UnbindTexture();
                    }
                    else
                    {
                        BindTexture(i,tex);
                        uvs[0] = entry.Textures[tex].X2;
                        uvs[1] = entry.Textures[tex].Y2;
                        uvs[2] = entry.Textures[tex].X1;
                        uvs[3] = entry.Textures[tex].Y1;
                        uvs[4] = entry.Textures[tex].X3;
                        uvs[5] = entry.Textures[tex].Y3;
                        uvs[6] = entry.Textures[tex].X4;
                        uvs[7] = entry.Textures[tex].Y4;
                    }
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(entry.Textures[tex].X2,entry.Textures[tex].Y2);
                    RenderVertex(entry,entry.Vertices[q.VertexA],0x7F);
                    GL.TexCoord2(entry.Textures[tex].X1,entry.Textures[tex].Y1);
                    RenderVertex(entry,entry.Vertices[q.VertexB],0x7F);
                    GL.TexCoord2(entry.Textures[tex].X3,entry.Textures[tex].Y3);
                    RenderVertex(entry,entry.Vertices[q.VertexC],0x7F);
                    GL.TexCoord2(entry.Textures[tex].X4,entry.Textures[tex].Y4);
                    RenderVertex(entry,entry.Vertices[q.VertexD],0x7F);
                    GL.End();
                }
            }
            GL.DepthMask(true);
            SetBlendMode(0);
            if (displaylist_translucid == -1)
            {
                displaylist_translucid = GL.GenLists(1);
                GL.NewList(displaylist_translucid, ListMode.CompileAndExecute);
                for (int e = 0; e < entries.Count; ++e)
                {
                    NewSceneryEntry entry = entries[e];
                    for (int i = 0; i < entry.Triangles.Count; ++i)
                    {
                        var t = entry.Triangles[i];
                        if ((t.VertexA >= entry.Vertices.Count || t.VertexB >= entry.Vertices.Count || t.VertexC >= entry.Vertices.Count) || (t.VertexA == t.VertexB && t.VertexB == t.VertexC && t.VertexC == t.VertexA)) continue;
                        if (t.Texture != 0 || t.Animated)
                        {
                            int tex = t.Texture - 1;
                            if (t.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                if (anim.Offset == 0)
                                    continue;
                                else if (anim.IsLOD)
                                {
                                    tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            if (entry.Textures[tex].BlendMode != 0)
                            {
                                continue;
                            }
                            BindTexture(e, tex);
                            uvs[0] = entry.Textures[tex].X2;
                            uvs[1] = entry.Textures[tex].Y2;
                            uvs[2] = entry.Textures[tex].X1;
                            uvs[3] = entry.Textures[tex].Y1;
                            uvs[4] = entry.Textures[tex].X3;
                            uvs[5] = entry.Textures[tex].Y3;
                        }
                        else
                            continue;
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(uvs[0], uvs[1]);
                        RenderVertex(entry, entry.Vertices[t.VertexA]);
                        GL.TexCoord2(uvs[2], uvs[3]);
                        RenderVertex(entry, entry.Vertices[t.VertexB]);
                        GL.TexCoord2(uvs[4], uvs[5]);
                        RenderVertex(entry, entry.Vertices[t.VertexC]);
                        GL.End();
                    }
                    for (int i = 0; i < entry.Quads.Count; ++i)
                    {
                        var q = entry.Quads[i];
                        if ((q.VertexA >= entry.Vertices.Count || q.VertexB >= entry.Vertices.Count || q.VertexC >= entry.Vertices.Count || q.VertexD >= entry.Vertices.Count) || (q.VertexA == q.VertexB && q.VertexB == q.VertexC && q.VertexC == q.VertexD && q.VertexD == q.VertexA)) continue;
                        if (q.Texture != 0 || q.Animated)
                        {
                            int tex = q.Texture - 1;
                            if (q.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                if (anim.Offset == 0)
                                    continue;
                                else if (anim.IsLOD)
                                {
                                    tex = anim.Offset - 1 + anim.LOD0; // we render the closest LOD for now
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            if (entry.Textures[tex].BlendMode != 0)
                            {
                                continue;
                            }
                            BindTexture(e, tex);
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
                            continue;
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
                }
                UnbindTexture();
                GL.EndList();
            }
            else
            {
                GL.CallList(displaylist_translucid);
            }
            SetBlendMode(3);
            UnbindTexture();
            if (!textures_enabled)
                GL.Enable(EnableCap.Texture2D);
        }

        private void RenderVertex(NewSceneryEntry entry,NewSceneryVertex vertex,byte alpha = 0xFF)
        {
            SceneryColor color = entry.Colors[vertex.Color];
            GL.Color4(color.Red,color.Green,color.Blue,alpha);
            GL.Vertex3(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
        }
    }
}
