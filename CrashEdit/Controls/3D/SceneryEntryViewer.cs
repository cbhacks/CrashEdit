using Crash;
using OpenTK;
using System.Collections.Generic;

namespace CrashEdit
{
    public class SceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private VAO vaoWorld => vaoListCrash1[0];
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
                vaoWorld.ZBufDisableWrite = i == 0;
                vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);

                int nb = 0;
                foreach (var world in worlds_to_use)
                {
                    nb += world.Triangles.Count * 3 + world.Quads.Count * 6;
                }
                vaoWorld.TestRealloc(nb);
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
            if (world.IsSky)
            {
                worldOffset = MathExt.Div(-render.Projection.RealTrans, vaoWorld.UserScale);
                if (world.IsC3)
                    worldOffset -= new Vector3(0x1800);
            }
            else
                worldOffset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            foreach (SceneryTriangle tri in world.Triangles)
            {
                if ((tri.VertexA >= world.Vertices.Count || tri.VertexB >= world.Vertices.Count || tri.VertexC >= world.Vertices.Count) ||
                    (tri.VertexA == tri.VertexB || tri.VertexB == tri.VertexC || tri.VertexC == tri.VertexA)) continue;
                var polygon_texture_info = ProcessTextureInfoC2(tri.Texture, tri.Animated, world.Textures, world.AnimatedTextures);
                if (!polygon_texture_info.Item1)
                    continue;
                int tex = 0; // completely untextured
                if (polygon_texture_info.Item2.HasValue)
                {
                    var info = polygon_texture_info.Item2.Value;
                    tex = TexInfoUnpacked.Pack(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, page: tex_eids[world.GetTPAG(info.Page)]);
                    vaoWorld.Verts[vaoWorld.VertCount + 0].st = new(info.X2, info.Y2);
                    vaoWorld.Verts[vaoWorld.VertCount + 1].st = new(info.X1, info.Y1);
                    vaoWorld.Verts[vaoWorld.VertCount + 2].st = new(info.X3, info.Y3);

                    blendMask |= TexInfoUnpacked.GetBlendMode(info.BlendMode);
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
                var polygon_texture_info = ProcessTextureInfoC2(quad.Texture, quad.Animated, world.Textures, world.AnimatedTextures);
                if (!polygon_texture_info.Item1)
                    continue;
                int tex = 0; // completely untextured
                if (polygon_texture_info.Item2.HasValue)
                {
                    var info = polygon_texture_info.Item2.Value;
                    tex = TexInfoUnpacked.Pack(true, color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY, page: tex_eids[world.GetTPAG(info.Page)]);
                    vaoWorld.Verts[vaoWorld.VertCount + 0].st = new(info.X2, info.Y2);
                    vaoWorld.Verts[vaoWorld.VertCount + 1].st = new(info.X1, info.Y1);
                    vaoWorld.Verts[vaoWorld.VertCount + 2].st = new(info.X3, info.Y3);
                    vaoWorld.Verts[vaoWorld.VertCount + 4].st = new(info.X4, info.Y4);

                    blendMask |= TexInfoUnpacked.GetBlendMode(info.BlendMode);
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
    }
}
