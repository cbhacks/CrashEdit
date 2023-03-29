using Crash;
using Crash.GOOLIns;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace CrashEdit
{
    public class SceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private VAO vaoWorld;
        Vector3 worldOffset;
        private BlendMode blendMask;

        protected override bool UseGrid => true;

        public SceneryEntryViewer(NSF nsf, int world) : base(nsf)
        {
            worlds = new() { world };
        }

        public SceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf)
        {
            this.worlds = new(worlds);
        }

        protected IEnumerable<SceneryEntry> GetWorlds()
        {
            foreach (int eid in worlds)
            {
                var world = nsf.GetEntry<SceneryEntry>(eid);
                if (world != null)
                {
                    yield return world;
                }
            }
        }

        protected IEnumerable<SceneryEntry> GetSkyWorlds()
        {
            foreach (var world in GetWorlds())
            {
                if (world.IsSky)
                {
                    yield return world;
                }
            }
        }

        protected IEnumerable<SceneryEntry> GetNonSkyWorlds()
        {
            foreach (var world in GetWorlds())
            {
                if (!world.IsSky)
                {
                    yield return world;
                }
            }
        }

        protected void SetWorlds(IEnumerable<int> worlds)
        {
            this.worlds = new(worlds);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (var world in GetWorlds())
                {
                    Vector3 trans = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
                    foreach (SceneryVertex vertex in world.Vertices)
                    {
                        Vector3 v_trans = (trans + new Vector3(vertex.X, vertex.Y, vertex.Z) * 16) / GameScales.WorldC1;
                        yield return new Position(v_trans.X, v_trans.Y, v_trans.Z);
                    }
                }
            }
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            vaoWorld = new(shaderContext, "crash1", PrimitiveType.Triangles);
        }

        private Dictionary<int, int> CollectTPAGs()
        {
            // collect tpag eids
            Dictionary<int, int> tex_eids = new();
            foreach (var world in GetWorlds())
            {
                for (int i = 0, m = world.TPAGCount; i < m; ++i)
                {
                    int tpag_eid = world.GetTPAG(i);
                    if (!tex_eids.ContainsKey(tpag_eid))
                        tex_eids[tpag_eid] = tex_eids.Count;
                }
            }
            return tex_eids;
        }

        protected override void Render()
        {
            base.Render();

            // setup textures
            var tex_eids = CollectTPAGs();
            SetupTPAGs(tex_eids);

            // render skies first then other things
            for (int i = 0; i < 2; ++i)
            {
                IEnumerable<SceneryEntry> worlds_to_use = i == 0 ? GetSkyWorlds() : GetNonSkyWorlds();
                if (i == 0)
                {
                    vaoWorld.ZBufDisableWrite = true;
                    vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1 * render.Distance);
                }
                else
                {
                    vaoWorld.ZBufDisableWrite = false;
                    vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);
                }
                int nb = 0;
                foreach (var world in worlds_to_use)
                {
                    nb += world.Triangles.Count * 3 + world.Quads.Count * 6;
                }
                vaoWorld.VertCount = nb;
                vaoWorld.TestRealloc();
                vaoWorld.DiscardVerts();

                // render stuff
                blendMask = BlendMode.Solid;
                foreach (var world in worlds_to_use)
                {
                    RenderWorld(world, tex_eids);
                }

                // render passes
                RenderWorldPass(BlendMode.Solid);
                RenderWorldPass(BlendMode.Trans);
                RenderWorldPass(BlendMode.Subtractive);
                RenderWorldPass(BlendMode.Additive);
            }
        }

        protected void RenderWorld(SceneryEntry world, Dictionary<int, int> tex_eids)
        {
            worldOffset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            if (world.IsSky)
            {
                worldOffset = MathExt.Div(-render.Projection.Trans, vaoWorld.UserScale);
            }
            foreach (SceneryTriangle tri in world.Triangles)
            {
                if ((tri.VertexA >= world.Vertices.Count || tri.VertexB >= world.Vertices.Count || tri.VertexC >= world.Vertices.Count) ||
                    (tri.VertexA == tri.VertexB || tri.VertexB == tri.VertexC || tri.VertexC == tri.VertexA)) continue;
                int tex = 0; // completely untextured
                if (tri.Texture != 0 || tri.Animated)
                {
                    ModelTexture? info_temp = null;
                    int tex_id = tri.Texture - 1;
                    if (tri.Animated)
                    {
                        var anim = world.AnimatedTextures[++tex_id];
                        // check if it's an untextured polygon
                        if (anim.Offset != 0)
                        {
                            tex_id = anim.Offset - 1;
                            if (anim.IsLOD)
                            {
                                tex_id += anim.LOD0; // we only render closest LOD for now
                            }
                            else
                            {
                                tex_id += (int)((render.RealCurrentFrame / 2 / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                                if (anim.Leap)
                                {
                                    anim = world.AnimatedTextures[++tex_id];
                                    tex_id = anim.Offset - 1 + anim.LOD0;
                                }
                            }
                            if (tex_id >= world.Textures.Count)
                            {
                                continue;
                            }
                            info_temp = world.Textures[tex_id];
                        }
                    }
                    else
                    {
                        if (tex_id >= world.Textures.Count)
                        {
                            continue;
                        }
                        info_temp = world.Textures[tex_id];
                    }
                    if (info_temp.HasValue)
                    {
                        var info = info_temp.Value;
                        tex = TexInfoUnpacked.Pack(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, page: tex_eids[world.GetTPAG(info.Page)]);
                        vaoWorld.Verts[vaoWorld.VertCount + 0].st = new(info.X2, info.Y2);
                        vaoWorld.Verts[vaoWorld.VertCount + 1].st = new(info.X1, info.Y1);
                        vaoWorld.Verts[vaoWorld.VertCount + 2].st = new(info.X3, info.Y3);

                        blendMask |= TexInfoUnpacked.GetBlendMode(info.BlendMode);
                    }
                }
                vaoWorld.Verts[vaoWorld.VertCount + 2].tex = tex;

                RenderVertex(world, tri.VertexA);
                RenderVertex(world, tri.VertexB);
                RenderVertex(world, tri.VertexC);
            }
            foreach (SceneryQuad quad in world.Quads)
            {
                if ((quad.VertexA >= world.Vertices.Count || quad.VertexB >= world.Vertices.Count || quad.VertexC >= world.Vertices.Count || quad.VertexD >= world.Vertices.Count) ||
                    (quad.VertexA == quad.VertexB || quad.VertexB == quad.VertexC || quad.VertexC == quad.VertexD || quad.VertexD == quad.VertexA)) continue;
                int tex = 0; // completely untextured
                if (quad.Texture != 0 || quad.Animated)
                {
                    ModelTexture? info_temp = null;
                    int tex_id = quad.Texture - 1;
                    if (quad.Animated)
                    {
                        var anim = world.AnimatedTextures[++tex_id];
                        // check if it's an untextured polygon
                        if (anim.Offset != 0)
                        {
                            tex_id = anim.Offset - 1;
                            if (anim.IsLOD)
                            {
                                tex_id += anim.LOD0; // we only render closest LOD for now
                            }
                            else
                            {
                                tex_id += (int)((render.RealCurrentFrame / 2 / (1 + anim.Latency) + anim.Delay) & anim.Mask);
                                if (anim.Leap)
                                {
                                    anim = world.AnimatedTextures[++tex_id];
                                    tex_id = anim.Offset - 1 + anim.LOD0;
                                }
                            }
                            if (tex_id >= world.Textures.Count)
                            {
                                continue;
                            }
                            info_temp = world.Textures[tex_id];
                        }
                    }
                    else
                    {
                        if (tex_id >= world.Textures.Count)
                        {
                            continue;
                        }
                        info_temp = world.Textures[tex_id];
                    }
                    if (info_temp.HasValue)
                    {
                        var info = info_temp.Value;
                        tex = TexInfoUnpacked.Pack(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, page: tex_eids[world.GetTPAG(info.Page)]);
                        vaoWorld.Verts[vaoWorld.VertCount + 0].st = new(info.X2, info.Y2);
                        vaoWorld.Verts[vaoWorld.VertCount + 1].st = new(info.X1, info.Y1);
                        vaoWorld.Verts[vaoWorld.VertCount + 2].st = new(info.X3, info.Y3);
                        vaoWorld.Verts[vaoWorld.VertCount + 4].st = new(info.X4, info.Y4);

                        blendMask |= TexInfoUnpacked.GetBlendMode(info.BlendMode);
                    }
                }
                vaoWorld.Verts[vaoWorld.VertCount + 2].tex = tex;

                RenderVertex(world, quad.VertexA);
                RenderVertex(world, quad.VertexB);
                RenderVertex(world, quad.VertexC);
                vaoWorld.VertCount++;
                RenderVertex(world, quad.VertexD);
                vaoWorld.VertCount++;

                vaoWorld.Verts[vaoWorld.VertCount + 3 - 6] = vaoWorld.Verts[vaoWorld.VertCount + 0 - 6];
                vaoWorld.Verts[vaoWorld.VertCount + 5 - 6] = vaoWorld.Verts[vaoWorld.VertCount + 2 - 6];

            }
        }

        private void RenderWorldPass(BlendMode pass)
        {
            if ((pass & blendMask) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoWorld.BlendMask = BlendModeIndex(pass);
                vaoWorld.Render(render);
            }
        }

        private void RenderVertex(SceneryEntry world, int vert_idx)
        {
            SceneryVertex vert = world.Vertices[vert_idx];
            SceneryColor color = world.Colors[vert.Color];
            vaoWorld.Verts[vaoWorld.VertCount].trans = new Vector3(vert.X, vert.Y, vert.Z) * 16 + worldOffset;
            vaoWorld.Verts[vaoWorld.VertCount].rgba = new(color.Red, color.Green, color.Blue, 255);
            vaoWorld.VertCount++;
        }
        protected override void Dispose(bool disposing)
        {
            vaoWorld?.Dispose();

            base.Dispose(disposing);
        }
    }
}
