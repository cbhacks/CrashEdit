using Crash;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace CrashEdit
{
    public class OldSceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private VAO vaoWorld;
        Vector3 worldOffset;
        private BlendMode blendMask;

        protected override bool UseGrid => true;

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

        protected IEnumerable<OldSceneryEntry> GetSkyWorlds()
        {
            foreach (OldSceneryEntry world in GetWorlds())
            {
                if (world.IsSky)
                {
                    yield return world;
                }
            }
        }

        protected IEnumerable<OldSceneryEntry> GetNonSkyWorlds()
        {
            foreach (OldSceneryEntry world in GetWorlds())
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
                    foreach (OldSceneryVertex vertex in world.Vertices)
                    {
                        yield return new Position(world.XOffset + vertex.X, world.YOffset + vertex.Y, world.ZOffset + vertex.Z) / GameScales.WorldC1;
                    }
                }
            }
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            vaoWorld = new(shaderContext, "crash1", PrimitiveType.Triangles);
            vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);
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
                IEnumerable<OldSceneryEntry> worlds_to_use = i == 0 ? GetSkyWorlds() : GetNonSkyWorlds();
                if (i == 0)
                {
                    GL.DepthMask(false);
                    vaoWorld.UserScale = new Vector3(1 / (GameScales.WorldC1 / render.Distance));
                } else
                {
                    vaoWorld.UserScale = new Vector3(1 / GameScales.WorldC1);
                }
                int nb = 0;
                foreach (var world in worlds_to_use)
                {
                    nb += world.Polygons.Count * 3;
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

                GL.DepthMask(true);
            }
        }

        protected void RenderWorld(OldSceneryEntry world, Dictionary<int, int> tex_eids)
        {
            worldOffset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            if (world.IsSky)
            {
                worldOffset = -render.Projection.Trans * vaoWorld.UserScale;
            }
            foreach (OldSceneryPolygon polygon in world.Polygons)
            {
                OldModelStruct str = world.Structs[polygon.ModelStruct];
                if (str is OldSceneryTexture tex)
                {
                    vaoWorld.Verts[vaoWorld.VertCount + 0].st = new(tex.U3, tex.V3);
                    vaoWorld.Verts[vaoWorld.VertCount + 1].st = new(tex.U2, tex.V2);
                    vaoWorld.Verts[vaoWorld.VertCount + 2].st = new(tex.U1, tex.V1);

                    vaoWorld.Verts[vaoWorld.VertCount + 2].tex = TexInfoUnpacked.Pack(true, color: tex.ColorMode, blend: tex.BlendMode,
                                                                                            clutx: tex.ClutX, cluty: tex.ClutY,
                                                                                            page: tex_eids[world.GetTPAG(polygon.Page)]);
                    RenderVertex(world, polygon.VertexA);
                    RenderVertex(world, polygon.VertexB);
                    RenderVertex(world, polygon.VertexC);

                    blendMask |= TexInfoUnpacked.GetBlendMode(tex.BlendMode);
                }
                else
                {
                    vaoWorld.Verts[vaoWorld.VertCount + 2].tex = 0;
                    RenderVertex(world, polygon.VertexA);
                    RenderVertex(world, polygon.VertexB);
                    RenderVertex(world, polygon.VertexC);
                }
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

        private void RenderVertex(OldSceneryEntry world, int vert_idx)
        {
            OldSceneryVertex vert = world.Vertices[vert_idx];
            vaoWorld.Verts[vaoWorld.VertCount].trans = new Vector3(vert.X, vert.Y, vert.Z) + worldOffset;
            vaoWorld.Verts[vaoWorld.VertCount].rgba = new(vert.Red, vert.Green, vert.Blue, 255);
            vaoWorld.VertCount++;
        }

        protected override void Dispose(bool disposing)
        {
            vaoWorld?.Dispose();

            base.Dispose(disposing);
        }
    }
}
