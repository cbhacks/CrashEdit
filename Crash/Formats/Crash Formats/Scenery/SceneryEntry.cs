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
            throw new NotImplementedException();
        }
    }
}
