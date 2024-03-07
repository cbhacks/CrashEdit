using CrashEdit.Crash;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public class SceneryEntryViewer : BaseSceneryEntryViewer<SceneryEntry>
    {
        private List<SLSTPolygonID>? sortlist;

        public SceneryEntryViewer(NSF nsf, int world) : base(nsf, world) { }

        public SceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf, worlds) { }

        protected override void SetWorldOffset(SceneryEntry world)
        {
            if (world.IsSky)
            {
                world_offset = -render.Projection.Trans * GameScales.WorldC1;
                if (world.IsC3)
                    world_offset -= new Vector3(0x2000);
            }
            else
                world_offset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
        }

        protected void SetSortList(IEnumerable<SLSTPolygonID> sortlist)
        {
            if (sortlist != null)
                this.sortlist = new(sortlist);
            else
                this.sortlist = null;
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

        protected override void CollectTPAGs()
        {
            tpages.Clear();
            foreach (var world in GetWorlds())
            {
                for (int i = 0, m = world.TPAGCount; i < m; ++i)
                {
                    int tpag_eid = world.GetTPAG(i);
                    if (!tpages.ContainsKey(tpag_eid))
                        tpages[tpag_eid] = (short)tpages.Count;
                }
            }
        }

        protected override void Render()
        {
            base.Render();

            // collect valid worlds
            var all_worlds = GetWorlds();
            vaoWorld.TestRealloc(all_worlds.Sum(x => (x?.Triangles.Count ?? 0) + (x?.Quads.Count ?? 0) * 2) * 3);

            // render skies first, then other things
            for (int i = 0; i < 2; ++i)
            {
                bool sky = i == 0;
                vaoWorld.ZBufDisableWrite = sky;

                vaoWorld.DiscardVerts();

                // render stuff
                blend_mask = BlendMode.Solid;
                if (sortlist == null)
                {
                    foreach (var world in all_worlds)
                    {
                        if (world == null || world.IsSky != sky)
                            continue;
                        RenderWorld(world);
                    }
                }
                else
                {
                    SceneryEntry lastworld = null;
                    foreach (var poly_id in sortlist)
                    {
                        if (poly_id.World >= all_worlds.Count)
                            continue;
                        var world = all_worlds[poly_id.World];
                        if (world == null || world.IsSky != sky)
                            continue;
                        if (world != lastworld)
                        {
                            SetWorldOffset(world);
                            lastworld = world;
                        }
                        if (poly_id.State == 0)
                            RenderTriangle(world, poly_id.ID);
                        else
                            RenderQuad(world, poly_id.ID, poly_id.State);
                    }
                }

                RenderPasses();
            }
        }

        protected override void RenderWorld(SceneryEntry world)
        {
            SetWorldOffset(world);
            for (int i = 0; i < world.Triangles.Count; ++i)
            {
                RenderTriangle(world, i);
            }
            for (int i = 0; i < world.Quads.Count; ++i)
            {
                RenderQuad(world, i, 3);
            }
        }

        private void RenderTriangle(SceneryEntry world, int index)
        {
            var tri = world.Triangles[index];
            if (tri.VertexA >= world.Vertices.Count || tri.VertexB >= world.Vertices.Count || tri.VertexC >= world.Vertices.Count)
                return;
            if (!ProcessTextureInfoC2(tri.Texture, tri.Animated, world.Textures, world.AnimatedTextures, out var polygon_texture_info))
                return;
            ref var a = ref vaoWorld.Verts[vaoWorld.CurVert + 0];
            ref var b = ref vaoWorld.Verts[vaoWorld.CurVert + 1];
            ref var c = ref vaoWorld.Verts[vaoWorld.CurVert + 2];
            VertexTexInfo tex = new(); // completely untextured
            if (polygon_texture_info != null)
            {
                var info = polygon_texture_info;
                tex = new(tpages[world.GetTPAG(info.Page)], color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY);
                a.st = new(info.X2, info.Y2);
                b.st = new(info.X1, info.Y1);
                c.st = new(info.X3, info.Y3);

                blend_mask |= VertexTexInfo.GetBlendMode(info.BlendMode);
            }
            a.tex = tex;
            b.tex = tex;
            c.tex = tex;

            RenderVertex(world, tri.VertexA);
            RenderVertex(world, tri.VertexB);
            RenderVertex(world, tri.VertexC);
        }

        private void RenderQuad(SceneryEntry world, int index, int state)
        {
            var quad = world.Quads[index];
            if (quad.VertexA >= world.Vertices.Count || quad.VertexB >= world.Vertices.Count || quad.VertexC >= world.Vertices.Count || quad.VertexD >= world.Vertices.Count)
                return;
            if (!ProcessTextureInfoC2(quad.Texture, quad.Animated, world.Textures, world.AnimatedTextures, out var polygon_texture_info))
                return;
            ref var a = ref vaoWorld.Verts[vaoWorld.CurVert + 0];
            ref var b = ref vaoWorld.Verts[vaoWorld.CurVert + 1];
            ref var c = ref vaoWorld.Verts[vaoWorld.CurVert + 2];
            ref var d = ref vaoWorld.Verts[vaoWorld.CurVert + 3];
            ref var e = ref vaoWorld.Verts[vaoWorld.CurVert + 4];
            ref var f = ref vaoWorld.Verts[vaoWorld.CurVert + 5];
            VertexTexInfo tex = new(); // completely untextured
            if (polygon_texture_info != null)
            {
                var info = polygon_texture_info;
                tex = new(tpages[world.GetTPAG(info.Page)], color: info.ColorMode, blend: info.BlendMode, clutx: info.ClutX, cluty: info.ClutY);
                a.st = new(info.X2, info.Y2);
                b.st = new(info.X1, info.Y1);
                c.st = new(info.X3, info.Y3);
                d.st = new(info.X2, info.Y2);
                e.st = new(info.X4, info.Y4);
                f.st = new(info.X3, info.Y3);

                blend_mask |= VertexTexInfo.GetBlendMode(info.BlendMode);
            }
            a.tex = tex;
            b.tex = tex;
            c.tex = tex;
            d.tex = tex;
            e.tex = tex;
            f.tex = tex;

            if (state == 1)
            {
                RenderVertex(world, quad.VertexA);
                RenderVertex(world, quad.VertexB);
                RenderVertex(world, quad.VertexC);
            }
            else if (state == 2)
            {
                b = d;
                RenderVertex(world, quad.VertexA);
                RenderVertex(world, quad.VertexD);
                RenderVertex(world, quad.VertexC);
            }
            else if (state == 3)
            {
                RenderVertex(world, quad.VertexA);
                RenderVertex(world, quad.VertexB);
                RenderVertex(world, quad.VertexC);
                vaoWorld.CopyAttrib(vaoWorld.CurVert - 3); // copy A
                RenderVertex(world, quad.VertexD);
                vaoWorld.CopyAttrib(vaoWorld.CurVert - 3); // copy C
            }
        }

        private void RenderVertex(SceneryEntry world, int index)
        {
            SceneryVertex vert = world.Vertices[index];
            SceneryColor color = world.Colors[vert.Color];
            vaoWorld.Verts[vaoWorld.CurVert].trans = (new Vector3(vert.X, vert.Y, vert.Z) * 16 + world_offset) / GameScales.WorldC1;
            vaoWorld.Verts[vaoWorld.CurVert].rgba = new(color.Red, color.Green, color.Blue, 255);
            vaoWorld.CurVert++;
        }
    }
}
