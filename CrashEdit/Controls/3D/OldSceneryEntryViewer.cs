using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public class OldSceneryEntryViewer : BaseSceneryEntryViewer<OldSceneryEntry>
    {
        private List<OldSLSTPolygonID>? sortlist;

        public OldSceneryEntryViewer(NSF nsf, int world) : base(nsf, world) { }

        public OldSceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf, worlds) { }

        protected override void SetWorldOffset(OldSceneryEntry world)
        {
            if (world.IsSky)
                world_offset = -render.Projection.Trans * GameScales.WorldC1;
            else
                world_offset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
        }

        protected void SetSortList(IEnumerable<OldSLSTPolygonID> sortlist)
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
                    foreach (OldSceneryVertex vertex in world.Vertices)
                    {
                        yield return new Position(world.XOffset + vertex.X, world.YOffset + vertex.Y, world.ZOffset + vertex.Z) / GameScales.WorldC1;
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
            vaoWorld.TestRealloc(all_worlds.Sum(x => x?.Polygons.Count ?? 0) * 3);

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
                    OldSceneryEntry lastworld = null;
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
                        RenderPolygon(world, poly_id.ID);
                    }
                }

                RenderPasses();
            }
        }

        protected override void RenderWorld(OldSceneryEntry world)
        {
            SetWorldOffset(world);
            for (int i = 0; i < world.Polygons.Count; ++i)
            {
                RenderPolygon(world, i);
            }
        }

        private void RenderPolygon(OldSceneryEntry world, int index)
        {
            var polygon = world.Polygons[index];
            OldModelStruct str = world.Structs[polygon.ModelStruct];
            ref var a = ref vaoWorld.Verts[vaoWorld.CurVert + 0];
            ref var b = ref vaoWorld.Verts[vaoWorld.CurVert + 1];
            ref var c = ref vaoWorld.Verts[vaoWorld.CurVert + 2];
            if (str is OldSceneryTexture tex)
            {
                a.st = new(tex.U3, tex.V3);
                b.st = new(tex.U2, tex.V2);
                c.st = new(tex.U1, tex.V1);

                a.tex = new VertexTexInfo(tpages[world.GetTPAG(polygon.Page)], color: tex.ColorMode, blend: tex.BlendMode, clutx: tex.ClutX, cluty: tex.ClutY);

                blend_mask |= VertexTexInfo.GetBlendMode(tex.BlendMode);
            }
            else
            {
                a.tex = new VertexTexInfo();
            }
            b.tex = a.tex;
            c.tex = a.tex;
            RenderVertex(world.Vertices[polygon.VertexA]);
            RenderVertex(world.Vertices[polygon.VertexB]);
            RenderVertex(world.Vertices[polygon.VertexC]);
        }

        private void RenderVertex(in OldSceneryVertex vert)
        {
            ref var v = ref vaoWorld.Verts[vaoWorld.CurVert];
            v.trans = (new Vector3(vert.X, vert.Y, vert.Z) + world_offset) / GameScales.WorldC1;
            v.rgba = new(vert.Red, vert.Green, vert.Blue, 255);
            vaoWorld.CurVert++;
        }
    }
}
