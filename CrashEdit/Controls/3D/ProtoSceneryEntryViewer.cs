using Crash;
using OpenTK;
using System.Collections.Generic;

namespace CrashEdit
{
    public class ProtoSceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private VAO vaoWorld => vaoListCrash1[0];
        Vector3 world_offset;
        private BlendMode blend_mask;

        public ProtoSceneryEntryViewer(NSF nsf, int world) : base(nsf)
        {
            worlds = new() { world };
        }

        public ProtoSceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf)
        {
            this.worlds = new(worlds);
        }

        protected IEnumerable<ProtoSceneryEntry> GetWorlds()
        {
            foreach (int eid in worlds)
            {
                var world = nsf.GetEntry<ProtoSceneryEntry>(eid);
                if (world != null)
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
                    foreach (ProtoSceneryVertex vertex in world.Vertices)
                    {
                        yield return new Position(world.XOffset + vertex.X, world.YOffset + vertex.Y, world.ZOffset + vertex.Z) / GameScales.WorldC1;
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

            vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);

            int nb = 0;
            foreach (var world in GetWorlds())
            {
                nb += world.Polygons.Count * 3;
            }
            vaoWorld.TestRealloc(nb);
            vaoWorld.DiscardVerts();

            // render stuff
            blend_mask = BlendMode.Solid;
            foreach (var world in GetWorlds())
            {
                RenderWorld(world, tex_eids);
            }

            // render passes
            RenderWorldPass(BlendMode.Solid);
            RenderWorldPass(BlendMode.Trans);
            RenderWorldPass(BlendMode.Subtractive);
            RenderWorldPass(BlendMode.Additive);
        }

        protected void RenderWorld(ProtoSceneryEntry world, Dictionary<int, int> tex_eids)
        {
            world_offset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            foreach (ProtoSceneryPolygon polygon in world.Polygons)
            {
                OldModelStruct str = world.Structs[polygon.Texture];
                if (str is OldSceneryTexture tex)
                {
                    vaoWorld.Verts[vaoWorld.vert_count].rgba = new(tex.R, tex.G, tex.B, 255);
                    vaoWorld.Verts[vaoWorld.vert_count + 1].rgba = vaoWorld.Verts[vaoWorld.vert_count].rgba;
                    vaoWorld.Verts[vaoWorld.vert_count + 2].rgba = vaoWorld.Verts[vaoWorld.vert_count].rgba;
                    vaoWorld.Verts[vaoWorld.vert_count + 0].st = new(tex.U3, tex.V3);
                    vaoWorld.Verts[vaoWorld.vert_count + 1].st = new(tex.U2, tex.V2);
                    vaoWorld.Verts[vaoWorld.vert_count + 2].st = new(tex.U1, tex.V1);

                    vaoWorld.Verts[vaoWorld.vert_count + 2].tex = TexInfoUnpacked.Pack(true, color: tex.ColorMode, blend: tex.BlendMode,
                                                                                            clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                            page: tex_eids[world.GetTPAG(polygon.Page)]);
                    RenderVertex(world, polygon.VertexA);
                    RenderVertex(world, polygon.VertexB);
                    RenderVertex(world, polygon.VertexC);

                    blend_mask |= TexInfoUnpacked.GetBlendMode(tex.BlendMode);
                }
                else
                {
                    OldSceneryColor col = (OldSceneryColor)str;
                    vaoWorld.Verts[vaoWorld.vert_count].rgba = new(col.R, col.G, col.B, 255);
                    vaoWorld.Verts[vaoWorld.vert_count + 1].rgba = vaoWorld.Verts[vaoWorld.vert_count].rgba;
                    vaoWorld.Verts[vaoWorld.vert_count + 2].rgba = vaoWorld.Verts[vaoWorld.vert_count].rgba;
                    vaoWorld.Verts[vaoWorld.vert_count + 2].tex = 0;
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

        private void RenderVertex(ProtoSceneryEntry world, int vert_idx)
        {
            ProtoSceneryVertex vert = world.Vertices[vert_idx];
            vaoWorld.Verts[vaoWorld.vert_count].trans = new Vector3(vert.X, vert.Y, vert.Z) + world_offset;
            vaoWorld.vert_count++;
        }
    }
}
