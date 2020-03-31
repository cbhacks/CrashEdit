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
        private bool textures_enabled = true;
        private bool init = false;

        private TextureChunk[][] texturechunks;
        private List<SceneryTriangle>[] dyntris;
        private List<SceneryQuad>[] dynquads;
        private List<SceneryTriangle>[] lasttris;
        private List<SceneryQuad>[] lastquads;

        public NewSceneryEntryViewer(NewSceneryEntry entry,TextureChunk[] texturechunks)
        {
            entries = new List<NewSceneryEntry>() { entry };
            displaylist = -1;
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
                    ConvertTexturesToGL(i, texturechunks[i], entries[i].Textures);
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
                    foreach (var tri in entry.Triangles)
                    {
                        if ((tri.VertexA >= entry.Vertices.Count || tri.VertexB >= entry.Vertices.Count || tri.VertexC >= entry.Vertices.Count) || (tri.VertexA == tri.VertexB || tri.VertexB == tri.VertexC || tri.VertexC == tri.VertexA)) continue;
                        if (tri.Texture != 0 || tri.Animated)
                        {
                            bool untex = false;
                            int tex = tri.Texture - 1;
                            if (tri.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                tex = anim.Offset - 1;
                                if (anim.Offset == 0)
                                    untex = true;
                                else if (anim.IsLOD)
                                {
                                    tex += anim.LOD0; // we render the closest LOD for now
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
                                        lasttris[e].Add(tri);
                                    else
                                        dyntris[e].Add(tri);
                                    continue;
                                }
                            }
                            if (untex)
                            {
                                UnbindTexture();
                            }
                            else
                            {
                                var t = entry.Textures[tex];
                                if (t.BlendMode == 1)
                                {
                                    lasttris[e].Add(tri);
                                    continue;
                                }
                                BindTexture(e, tex);
                                uvs[0] = t.X2;
                                uvs[1] = t.Y2;
                                uvs[2] = t.X1;
                                uvs[3] = t.Y1;
                                uvs[4] = t.X3;
                                uvs[5] = t.Y3;
                                //SetBlendMode(t.BlendMode);
                            }
                        }
                        else
                            UnbindTexture();
                        GL.Begin(PrimitiveType.Triangles);
                        GL.TexCoord2(uvs[0],uvs[1]);
                        RenderVertex(entry,entry.Vertices[tri.VertexA]);
                        GL.TexCoord2(uvs[2],uvs[3]);
                        RenderVertex(entry,entry.Vertices[tri.VertexB]);
                        GL.TexCoord2(uvs[4],uvs[5]);
                        RenderVertex(entry,entry.Vertices[tri.VertexC]);
                        GL.End();
                    }
                    foreach (var q in entry.Quads)
                    {
                        if ((q.VertexA >= entry.Vertices.Count || q.VertexB >= entry.Vertices.Count || q.VertexC >= entry.Vertices.Count || q.VertexD >= entry.Vertices.Count) || (q.VertexA == q.VertexB || q.VertexB == q.VertexC || q.VertexC == q.VertexD || q.VertexD == q.VertexA)) continue;
                        if (q.Texture != 0 || q.Animated)
                        {
                            bool untex = false;
                            int tex = q.Texture - 1;
                            if (q.Animated)
                            {
                                ++tex;
                                var anim = entry.AnimatedTextures[tex];
                                tex = anim.Offset - 1;
                                if (anim.Offset == 0)
                                    untex = true;
                                else if (anim.IsLOD)
                                {
                                    tex += anim.LOD0; // we render the closest LOD for now
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
                                var t = entry.Textures[tex];
                                if (t.BlendMode == 1)
                                {
                                    lastquads[e].Add(q);
                                    continue;
                                }
                                BindTexture(e, tex);
                                uvs[0] = t.X2;
                                uvs[1] = t.Y2;
                                uvs[2] = t.X1;
                                uvs[3] = t.Y1;
                                uvs[4] = t.X3;
                                uvs[5] = t.Y3;
                                uvs[6] = t.X4;
                                uvs[7] = t.Y4;
                                //SetBlendMode(t.BlendMode);
                            }
                        }
                        else
                            UnbindTexture();
                        GL.Begin(PrimitiveType.Quads);
                        GL.TexCoord2(uvs[0],uvs[1]);
                        RenderVertex(entry,entry.Vertices[q.VertexA]);
                        GL.TexCoord2(uvs[2],uvs[3]);
                        RenderVertex(entry,entry.Vertices[q.VertexB]);
                        GL.TexCoord2(uvs[4],uvs[5]);
                        RenderVertex(entry,entry.Vertices[q.VertexC]);
                        GL.TexCoord2(uvs[6],uvs[7]);
                        RenderVertex(entry,entry.Vertices[q.VertexD]);
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
                        tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                    }
                    var t = entry.Textures[tex];
                    if (t.BlendMode == 1)
                    {
                        fakes.Add(tri);
                        continue;
                    }
                    BindTexture(i,tex);
                    SetBlendMode(t.BlendMode);
                    GL.Begin(PrimitiveType.Triangles);
                    GL.TexCoord2(t.X2,t.Y2);
                    RenderVertex(entry,entry.Vertices[tri.VertexA]);
                    GL.TexCoord2(t.X1,t.Y1);
                    RenderVertex(entry,entry.Vertices[tri.VertexB]);
                    GL.TexCoord2(t.X3,t.Y3);
                    RenderVertex(entry,entry.Vertices[tri.VertexC]);
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
                        tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                    }
                    var t = entry.Textures[tex];
                    if (t.BlendMode == 1)
                    {
                        fakes.Add(quad);
                        continue;
                    }
                    BindTexture(i,tex);
                    SetBlendMode(t.BlendMode);
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(t.X2,t.Y2);
                    RenderVertex(entry,entry.Vertices[quad.VertexA]);
                    GL.TexCoord2(t.X1,t.Y1);
                    RenderVertex(entry,entry.Vertices[quad.VertexB]);
                    GL.TexCoord2(t.X3,t.Y3);
                    RenderVertex(entry,entry.Vertices[quad.VertexC]);
                    GL.TexCoord2(t.X4,t.Y4);
                    RenderVertex(entry,entry.Vertices[quad.VertexD]);
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
                foreach (SceneryTriangle tri in lasttris[i])
                {
                    bool untex = false;
                    int tex = tri.Texture - 1;
                    if (tri.Animated)
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
                                tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                            }
                        }
                    }
                    var t = entry.Textures[tex];
                    if (untex)
                    {
                        UnbindTexture();
                    }
                    else
                    {
                        BindTexture(i,tex);
                        uvs[0] = t.X2;
                        uvs[1] = t.Y2;
                        uvs[2] = t.X1;
                        uvs[3] = t.Y1;
                        uvs[4] = t.X3;
                        uvs[5] = t.Y3;
                    }
                    GL.Begin(PrimitiveType.Triangles);
                    GL.TexCoord2(t.X2,t.Y2);
                    RenderVertex(entry,entry.Vertices[tri.VertexA]);
                    GL.TexCoord2(t.X1,t.Y1);
                    RenderVertex(entry,entry.Vertices[tri.VertexB]);
                    GL.TexCoord2(t.X3,t.Y3);
                    RenderVertex(entry,entry.Vertices[tri.VertexC]);
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
                                tex = entry.AnimatedTextures[tex].Offset - 1 + entry.AnimatedTextures[tex].LOD0;
                            }
                        }
                    }
                    var t = entry.Textures[tex];
                    if (untex)
                    {
                        UnbindTexture();
                    }
                    else
                    {
                        BindTexture(i,tex);
                        uvs[0] = t.X2;
                        uvs[1] = t.Y2;
                        uvs[2] = t.X1;
                        uvs[3] = t.Y1;
                        uvs[4] = t.X3;
                        uvs[5] = t.Y3;
                        uvs[6] = t.X4;
                        uvs[7] = t.Y4;
                    }
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(t.X2,t.Y2);
                    RenderVertex(entry,entry.Vertices[q.VertexA]);
                    GL.TexCoord2(t.X1,t.Y1);
                    RenderVertex(entry,entry.Vertices[q.VertexB]);
                    GL.TexCoord2(t.X3,t.Y3);
                    RenderVertex(entry,entry.Vertices[q.VertexC]);
                    GL.TexCoord2(t.X4,t.Y4);
                    RenderVertex(entry,entry.Vertices[q.VertexD]);
                    GL.End();
                }
            }
            GL.DepthMask(true);
            SetBlendMode(3);
            UnbindTexture();
            if (!textures_enabled)
                GL.Enable(EnableCap.Texture2D);
        }

        private void RenderVertex(NewSceneryEntry entry,NewSceneryVertex vertex)
        {
            SceneryColor color = entry.Colors[vertex.Color];
            GL.Color3(color.Red,color.Green,color.Blue);
            GL.Vertex3(entry.XOffset + vertex.X * 16,entry.YOffset + vertex.Y * 16,entry.ZOffset + vertex.Z * 16);
        }
    }
}
