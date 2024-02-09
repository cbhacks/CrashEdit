using Crash;
using OpenTK;
using System.Collections.Generic;

namespace CrashEdit
{
    public class OldSceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private VAO vaoWorld => vaoListCrash1[0];
        Vector3 world_offset;
        private BlendMode blend_mask;

        public OldSceneryEntryViewer(NSF nsf, int world) : base(nsf)
        {
            worlds = new() { world };
        }

        public OldSceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf)
        {
            this.worlds = new(worlds);
        }

        protected IEnumerable<OldSceneryEntry> GetWorlds()
        {
            foreach (int eid in worlds)
            {
                var world = nsf.GetEntry<OldSceneryEntry>(eid);
                if (world != null)
                {
                    yield return world;
                }
            }
        }

        protected IEnumerable<OldSceneryEntry> GetWorlds(bool want_sky)
        {
            foreach (OldSceneryEntry world in GetWorlds())
            {
                if (world.IsSky == want_sky)
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
                    foreach (OldSceneryVertex vertex in world.Vertices)
                    {
                        yield return new Position(world.XOffset + vertex.X, world.YOffset + vertex.Y, world.ZOffset + vertex.Z) / GameScales.WorldC1;
                    }
                }
            }
        }

        private Dictionary<int, short> CollectTPAGs()
        {
            // collect tpag eids
            Dictionary<int, short> tex_eids = new();
            foreach (var world in GetWorlds())
            {
                for (int i = 0, m = world.TPAGCount; i < m; ++i)
                {
                    int tpag_eid = world.GetTPAG(i);
                    if (!tex_eids.ContainsKey(tpag_eid))
                        tex_eids[tpag_eid] = (short)tex_eids.Count;
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
                IEnumerable<OldSceneryEntry> worlds_to_use = GetWorlds(i == 0);
                vaoWorld.ZBufDisableWrite = i == 0;
                vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);

                int nb = 0;
                foreach (var world in worlds_to_use)
                {
                    nb += world.Polygons.Count * 3;
                }
                vaoWorld.TestRealloc(nb);
                vaoWorld.DiscardVerts();

                // render stuff
                blend_mask = BlendMode.Solid;
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

        protected void RenderWorld(OldSceneryEntry world, Dictionary<int, short> tex_eids)
        {
            if (world.IsSky)
                world_offset = MathExt.Div(-render.Projection.RealTrans, vaoWorld.UserScale);
            else
                world_offset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            foreach (OldSceneryPolygon polygon in world.Polygons)
            {
                OldModelStruct str = world.Structs[polygon.ModelStruct];
                if (str is OldSceneryTexture tex)
                {
                    vaoWorld.Verts[vaoWorld.vert_count + 0].st = new(tex.U3, tex.V3);
                    vaoWorld.Verts[vaoWorld.vert_count + 1].st = new(tex.U2, tex.V2);
                    vaoWorld.Verts[vaoWorld.vert_count + 2].st = new(tex.U1, tex.V1);

                    vaoWorld.Verts[vaoWorld.vert_count + 0].tex = new VertexTexInfo(tex_eids[world.GetTPAG(polygon.Page)], color: tex.ColorMode, blend: tex.BlendMode,
                                                                                          clutx: tex.ClutX, cluty: tex.ClutY);
                    vaoWorld.Verts[vaoWorld.vert_count + 1].tex = vaoWorld.Verts[vaoWorld.vert_count + 0].tex;
                    vaoWorld.Verts[vaoWorld.vert_count + 2].tex = vaoWorld.Verts[vaoWorld.vert_count + 0].tex;
                    RenderVertex(world, polygon.VertexA);
                    RenderVertex(world, polygon.VertexB);
                    RenderVertex(world, polygon.VertexC);

                    blend_mask |= VertexTexInfo.GetBlendMode(tex.BlendMode);
                }
                else
                {
                    vaoWorld.Verts[vaoWorld.vert_count + 0].tex = new VertexTexInfo();
                    vaoWorld.Verts[vaoWorld.vert_count + 1].tex = new VertexTexInfo();
                    vaoWorld.Verts[vaoWorld.vert_count + 2].tex = new VertexTexInfo();
                    RenderVertex(world, polygon.VertexA);
                    RenderVertex(world, polygon.VertexB);
                    RenderVertex(world, polygon.VertexC);
                }
            }
        }

        private void RenderWorldPass(BlendMode pass)
        {
            if ((pass & blend_mask) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoWorld.BlendMask = BlendModeIndex(pass);
                vaoWorld.Render(render);
            }
        }

        private void RenderVertex(OldSceneryEntry world, int vert_idx)
        {
            OldSceneryVertex vert = world.Vertices[vert_idx];
            vaoWorld.Verts[vaoWorld.vert_count].trans = new Vector3(vert.X, vert.Y, vert.Z) + world_offset;
            vaoWorld.Verts[vaoWorld.vert_count].rgba = new(vert.Red, vert.Green, vert.Blue, 255);
            vaoWorld.vert_count++;
        }
    }
}
