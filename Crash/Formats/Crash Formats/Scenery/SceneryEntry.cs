using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SceneryEntry : Entry
    {
        private byte[] info;
        private List<SceneryVertex> vertices;
        private byte[] item2;
        private List<SceneryPolygon> polygons;
        private byte[] item4;
        private byte[] item5;
        private byte[] item6;

        public SceneryEntry(byte[] info,IEnumerable<SceneryVertex> vertices,byte[] item2,IEnumerable<SceneryPolygon> polygons,byte[] item4,byte[] item5,byte[] item6,int eid) : base(eid)
        {
            this.info = info;
            this.vertices = new List<SceneryVertex>(vertices);
            this.item2 = item2;
            this.polygons = new List<SceneryPolygon>(polygons);
            this.item4 = item4;
            this.item5 = item5;
            this.item6 = item6;
        }

        public override int Type
        {
            get { return 3; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public IList<SceneryVertex> Vertices
        {
            get { return vertices; }
        }

        public byte[] Item2
        {
            get { return item2; }
        }

        public IList<SceneryPolygon> Polygons
        {
            get { return polygons; }
        }

        public byte[] Item4
        {
            get { return item4; }
        }

        public byte[] Item5
        {
            get { return item5; }
        }

        public byte[] Item6
        {
            get { return item6; }
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

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [7][];
            items[0] = info;
            items[1] = new byte [vertices.Count * 6];
            for (int i = 0;i < vertices.Count;i++)
            {
                vertices[i].SaveXY().CopyTo(items[1],(vertices.Count - 1 - i) * 4);
                vertices[i].SaveZ().CopyTo(items[1],vertices.Count * 4 + i * 2);
            }
            items[2] = item2;
            items[3] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[3],i * 8);
            }
            items[4] = item4;
            items[5] = item5;
            items[6] = item6;
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
