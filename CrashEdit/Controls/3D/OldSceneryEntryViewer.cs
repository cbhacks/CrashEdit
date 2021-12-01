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

        private VAO vaoWorld;
        private Vector3[] buf_vtx;
        private Color4[] buf_col;
        private Vector2[] buf_uv;
        private int[] buf_tex;
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

        private IEnumerable<OldSceneryEntry> GetWorlds()
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            vaoWorld = new VAO(render.ShaderContext, "world_c1", PrimitiveType.Triangles);
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
            foreach (var world in GetWorlds())
            {
                RenderWorld(world, tex_eids);
            }
        }

        protected void RenderWorld(OldSceneryEntry world, Dictionary<int, int> tex_eids)
        {
            // alloc buffers
            int nb = world.Polygons.Count * 3;
            buf_vtx = new Vector3[nb];
            buf_col = new Color4[nb];
            buf_uv = new Vector2[nb];
            buf_tex = new int[nb]; // enable: 1, colormode: 2, blendmode: 2, clutx: 4, cluty: 7, doubleface: 1, page: X (>17 total)

            // render stuff
            RenderWorldPass(world, tex_eids, RenderPass.Solid);
            RenderWorldPass(world, tex_eids, RenderPass.Trans);
            RenderWorldPass(world, tex_eids, RenderPass.Subtractive);
            RenderWorldPass(world, tex_eids, RenderPass.Additive);
        }

        private void RenderWorldPass(OldSceneryEntry world, Dictionary<int, int> tex_eids, RenderPass pass)
        {
            buf_idx = 0;

            foreach (OldSceneryPolygon polygon in world.Polygons)
            {
                OldModelStruct str = world.Structs[polygon.ModelStruct];
                if (str is OldSceneryTexture tex)
                {
                    if (pass == RenderPass.Trans && tex.BlendMode != 0) continue;
                    if (pass == RenderPass.Additive && tex.BlendMode != 1) continue;
                    if (pass == RenderPass.Subtractive && tex.BlendMode != 2) continue;
                    buf_uv[buf_idx + 0] = new(tex.U3, tex.V3);
                    buf_uv[buf_idx + 1] = new(tex.U2, tex.V2);
                    buf_uv[buf_idx + 2] = new(tex.U1, tex.V1);
                    buf_tex[buf_idx + 2] = 1
                                        | (tex.ColorMode << 1)
                                        | (tex.BlendMode << 3)
                                        | (tex.ClutX << 5)
                                        | (tex.ClutY << 9)
                                        | (tex_eids[world.GetTPAG(polygon.Page)] << 17)
                                        ;
                    RenderVertex(world.Vertices[polygon.VertexA]);
                    RenderVertex(world.Vertices[polygon.VertexB]);
                    RenderVertex(world.Vertices[polygon.VertexC]);
                }
                else
                {
                    if (pass != RenderPass.Solid) continue;
                    buf_tex[buf_idx + 2] = 0;
                    RenderVertex(world.Vertices[polygon.VertexA]);
                    RenderVertex(world.Vertices[polygon.VertexB]);
                    RenderVertex(world.Vertices[polygon.VertexC]);
                }
            }

            if (buf_idx > 0)
            {
                // uniforms and static data
                render.Projection.UserTrans = new(world.XOffset, world.YOffset, world.ZOffset);
                render.BlendMask = pass == RenderPass.Solid;

                vaoWorld.UpdatePositions(buf_vtx, buf_idx);
                vaoWorld.UpdateColors(buf_col, buf_idx);
                vaoWorld.UpdateUVs(buf_uv, buf_idx);
                vaoWorld.UpdateAttrib(1, "tex", buf_tex, 4, 1, buf_idx);

                SetBlendForRenderPass(pass);
                vaoWorld.Render(render, vertcount: buf_idx);
            }
        }

        private void RenderVertex(OldSceneryVertex vert)
        {
            buf_vtx[buf_idx] = new(vert.X, vert.Y, vert.Z);
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
