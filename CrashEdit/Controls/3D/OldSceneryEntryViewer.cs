using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public class OldSceneryEntryViewer : GLViewer
    {
        private List<int> worlds;

        private MultiPassVAO vaoWorld;
        private Vector3[] buf_vtx;
        private Rgba[] buf_col;
        private Vector2[] buf_uv;
        private TexInfoUnpacked[] buf_tex;
        private int buf_idx;

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

            vaoWorld = new(BlendMode.All, shaderContext, "crash1", PrimitiveType.Triangles);
            vaoWorld.ForEachVAO((VAO vao) =>
            {
                vao.ArtType = VAO.ArtTypeEnum.Crash1World;
                vao.UserScaleScalar = GameScales.WorldC1;
            });
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

        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        protected override void Render()
        {
            base.Render();
            long dbgTimeSetupTPAGs = 0;
            long dbgTimeAllocBufs = 0;
            long dbgTimeMakeVerts = 0;
            long dbgTimePushVerts = 0;
            long dbgTimeRender = 0;
            watch.Restart();
            vaoWorld.DiscardVerts();

            // setup textures
            var tex_eids = CollectTPAGs();
            SetupTPAGs(tex_eids);
            dbgTimeSetupTPAGs = watch.StopAndElapsed();
            watch.Restart();

            int nb = 0;
            foreach (var world in GetWorlds())
            {
                nb += world.Polygons.Count * 3;
            }
            if (buf_vtx == null || buf_vtx.Length < nb)
                buf_vtx = new Vector3[nb];
            if (buf_col == null || buf_col.Length < nb)
                buf_col = new Rgba[nb];
            if (buf_uv == null || buf_uv.Length < nb)
                buf_uv = new Vector2[nb];
            if (buf_tex == null || buf_tex.Length < nb)
                buf_tex = new TexInfoUnpacked[nb];
            dbgTimeAllocBufs = watch.StopAndElapsed();
            watch.Restart();

            // render stuff
            buf_idx = 0;
            foreach (var world in GetWorlds())
            {
                RenderWorld(world, tex_eids);
            }
            dbgTimeMakeVerts = watch.StopAndElapsed();
            watch.Restart();

            for (int i = 0; i < buf_idx; ++i)
            {
                vaoWorld.PushAttrib(buf_tex[i / 3 * 3 + 2].GetBlendMode() | BlendMode.Solid, trans: buf_vtx[i], rgba: buf_col[i], st: buf_uv[i], tex: buf_tex[i]);
            }
            dbgTimePushVerts = watch.StopAndElapsed();
            watch.Restart();

            // render passes
            vaoWorld.Render(render);
            dbgTimeRender = watch.StopAndElapsed();
            watch.Restart();
            watch.Stop();
            if (true)
            {
                vaoWorld.ForEachVAO((VAO vao) =>
                {
                    Console.WriteLine($"WGEO vao blend {vao.BlendMask} has {vao.VertCount} verts");
                });
                double t_to_ms = 1 / (System.Diagnostics.Stopwatch.Frequency / 1000.0);
                Console.WriteLine($"WGEO renderer dbgTimeSetupTPAGs: {dbgTimeSetupTPAGs * t_to_ms}ms");
                Console.WriteLine($"WGEO renderer dbgTimeAllocBufs: {dbgTimeAllocBufs * t_to_ms}ms");
                Console.WriteLine($"WGEO renderer dbgTimeMakeVerts: {dbgTimeMakeVerts * t_to_ms}ms");
                Console.WriteLine($"WGEO renderer dbgTimePushVerts: {dbgTimePushVerts * t_to_ms}ms");
                Console.WriteLine($"WGEO renderer dbgTimeRender: {dbgTimeRender * t_to_ms}ms");
            }
        }

        protected void RenderWorld(OldSceneryEntry world, Dictionary<int, int> tex_eids)
        {
            foreach (OldSceneryPolygon polygon in world.Polygons)
            {
                OldModelStruct str = world.Structs[polygon.ModelStruct];
                if (str is OldSceneryTexture tex)
                {
                    buf_uv[buf_idx + 0] = new(tex.U3, tex.V3);
                    buf_uv[buf_idx + 1] = new(tex.U2, tex.V2);
                    buf_uv[buf_idx + 2] = new(tex.U1, tex.V1);

                    buf_tex[buf_idx + 2] = new(true, color: tex.ColorMode, blend: tex.BlendMode,
                                                     clutx: tex.ClutX, cluty: tex.ClutY,
                                                     page: tex_eids[world.GetTPAG(polygon.Page)]);
                    RenderVertex(world, world.Vertices[polygon.VertexA]);
                    RenderVertex(world, world.Vertices[polygon.VertexB]);
                    RenderVertex(world, world.Vertices[polygon.VertexC]);
                }
                else
                {
                    buf_tex[buf_idx + 2] = new(false);
                    RenderVertex(world, world.Vertices[polygon.VertexA]);
                    RenderVertex(world, world.Vertices[polygon.VertexB]);
                    RenderVertex(world, world.Vertices[polygon.VertexC]);
                }
            }
        }

        private void RenderVertex(OldSceneryEntry world, OldSceneryVertex vert)
        {
            buf_vtx[buf_idx] = new Vector3(vert.X, vert.Y, vert.Z) + new Vector3(world.XOffset, world.YOffset, world.ZOffset);
            buf_col[buf_idx] = new(vert.Red, vert.Green, vert.Blue, 255);
            buf_idx++;
        }

        protected override void Dispose(bool disposing)
        {
            vaoWorld?.Dispose();

            base.Dispose(disposing);
        }
    }
}
