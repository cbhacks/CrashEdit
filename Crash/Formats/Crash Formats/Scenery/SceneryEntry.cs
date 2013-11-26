using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class SceneryEntry : Entry
    {
        private byte[] info;
        private List<SceneryVertex> vertices;
        private List<SceneryTriangle> triangles;
        private List<SceneryQuad> quads;
        private byte[] item4;
        private List<SceneryColor> colors;
        private byte[] item6;

        public SceneryEntry(byte[] info,IEnumerable<SceneryVertex> vertices,IEnumerable<SceneryTriangle> triangles,IEnumerable<SceneryQuad> quads,byte[] item4,IEnumerable<SceneryColor> colors,byte[] item6,int eid) : base(eid)
        {
            this.info = info;
            this.vertices = new List<SceneryVertex>(vertices);
            this.triangles = new List<SceneryTriangle>(triangles);
            this.quads = new List<SceneryQuad>(quads);
            this.item4 = item4;
            this.colors = new List<SceneryColor>(colors);
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

        public IList<SceneryTriangle> Triangles
        {
            get { return triangles; }
        }

        public IList<SceneryQuad> Quads
        {
            get { return quads; }
        }

        public byte[] Item4
        {
            get { return item4; }
        }

        public IList<SceneryColor> Colors
        {
            get { return colors; }
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
            items[2] = new byte [triangles.Count * 6];
            for (int i = 0;i < triangles.Count;i++)
            {
                triangles[i].SaveA().CopyTo(items[2],(triangles.Count - 1 - i) * 4);
                triangles[i].SaveB().CopyTo(items[2],triangles.Count * 4 + i * 2);
            }
            items[3] = new byte [quads.Count * 8];
            for (int i = 0;i < quads.Count;i++)
            {
                quads[i].Save().CopyTo(items[3],i * 8);
            }
            items[4] = item4;
            items[5] = new byte [colors.Count * 4];
            for (int i = 0;i < colors.Count;i++)
            {
                items[5][i * 4 + 0] = colors[i].Red;
                items[5][i * 4 + 1] = colors[i].Green;
                items[5][i * 4 + 2] = colors[i].Blue;
                items[5][i * 4 + 3] = colors[i].Extra;
            }
            items[6] = item6;
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
