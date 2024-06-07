using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public class ProtoSceneryEntryViewer : BaseSceneryEntryViewer<ProtoSceneryEntry>
    {
        private List<OldSLSTPolygonID>? sortlist;

        public ProtoSceneryEntryViewer(NSF nsf, int world) : base(nsf, world) { }

        public ProtoSceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf, worlds) { }

        protected override void SetWorldOffset(ProtoSceneryEntry world)
        {
            world_offset = new Vector3(world.XOffset, world.YOffset, world.ZOffset);
        }

        protected void SetSortList(IEnumerable<OldSLSTPolygonID> sortlist)
        {
            if (sortlist != null)
                this.sortlist = new(sortlist);
            else
                this.sortlist = null;
        }

        protected override bool HasSky => false;

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

        protected override void CollectTPAGs()
        {
            foreach (var world in GetWorlds())
            {
                for (int i = 0, m = world.TPAGCount; i < m; ++i)
                {
                    tpages.AddTexturePage(world.GetTPAG(i));
                }
            }
        }

        protected override void RenderWorlds(bool sky)
        {
            // collect valid worlds
            var all_worlds = GetWorlds();
            _vao.TestRealloc(all_worlds.Sum(x => x?.Polygons.Count ?? 0) * 3);

            if (sortlist == null)
            {
                // render stuff
                foreach (var world in all_worlds)
                {
                    if (world == null)
                        continue;
                    RenderWorld(world);
                }
            }
            else
            {
                ProtoSceneryEntry lastworld = null;
                foreach (var poly_id in sortlist)
                {
                    if (poly_id.World >= all_worlds.Count)
                        continue;
                    var world = all_worlds[poly_id.World];
                    if (world == null)
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

        protected override void RenderWorld(ProtoSceneryEntry world)
        {
            SetWorldOffset(world);
            for (int i = 0; i < world.Polygons.Count; ++i)
            {
                RenderPolygon(world, i);
            }
        }

        private void RenderPolygon(ProtoSceneryEntry world, int index)
        {
            var polygon = world.Polygons[index];
            OldModelStruct str = world.Structs[polygon.Texture];
            ref var a = ref _vao.Verts[_vao.CurVert + 0];
            ref var b = ref _vao.Verts[_vao.CurVert + 1];
            ref var c = ref _vao.Verts[_vao.CurVert + 2];
            if (str is OldSceneryTexture tex)
            {
                a.rgba = new(tex.R, tex.G, tex.B, 255);
                a.st = new(tex.U3, tex.V3);
                b.st = new(tex.U2, tex.V2);
                c.st = new(tex.U1, tex.V1);

                a.tex = new VertexTexInfo(tpages[world.GetTPAG(polygon.Page)], color: tex.ColorMode, blend: tex.BlendMode, clutx: tex.ClutX, cluty: tex.ClutY);

                _vao.BlendModes |= VertexTexInfo.GetBlendMode(tex.BlendMode);
            }
            else
            {
                OldSceneryColor col = (OldSceneryColor)str;
                a.rgba = new(col.R, col.G, col.B, 255);
                a.tex = new VertexTexInfo();
            }
            b.rgba = a.rgba;
            c.rgba = a.rgba;
            b.tex = a.tex;
            c.tex = a.tex;
            RenderVertex(world.Vertices[polygon.VertexA]);
            RenderVertex(world.Vertices[polygon.VertexB]);
            RenderVertex(world.Vertices[polygon.VertexC]);
        }

        private void RenderVertex(in ProtoSceneryVertex vert)
        {
            _vao.Verts[_vao.CurVert].trans = (new Vector3(vert.X, vert.Y, vert.Z) + world_offset) / GameScales.WorldC1;
            _vao.CurVert++;
        }
    }
}
