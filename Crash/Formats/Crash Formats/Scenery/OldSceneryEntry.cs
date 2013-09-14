using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldSceneryEntry : Entry
    {
        private byte[] info;
        private List<OldSceneryPolygon> polygons;
        private List<OldSceneryVertex> vertices;

        public OldSceneryEntry(byte[] info,IEnumerable<OldSceneryPolygon> polygons,IEnumerable<OldSceneryVertex> vertices,int eid) : base(eid)
        {
            this.info = info;
            this.polygons = new List<OldSceneryPolygon>(polygons);
            this.vertices = new List<OldSceneryVertex>(vertices);
        }

        public override int Type
        {
            get { return 3; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public int XOffset
        {
            get { return BitConv.FromInt32(info,0); }
        }

        public int YOffset
        {
            get { return BitConv.FromInt32(info,4); }
        }

        public int ZOffset
        {
            get { return BitConv.FromInt32(info,8); }
        }

        public IList<OldSceneryPolygon> Polygons
        {
            get { return polygons; }
        }

        public IList<OldSceneryVertex> Vertices
        {
            get { return vertices; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [3][];
            items[0] = info;
            items[1] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 8);
            }
            items[2] = new byte [vertices.Count * 8];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].Save().CopyTo(items[2],i * 8);
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
