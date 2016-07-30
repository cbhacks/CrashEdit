using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldModelEntry : Entry
    {
        private byte[] info;
        private List<OldModelPolygon> polygons;

        public OldModelEntry(byte[] info,IEnumerable<OldModelPolygon> polygons,int eid, int size) : base(eid, size)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            this.info = info;
            this.polygons = new List<OldModelPolygon>(polygons);
        }

        public override int Type
        {
            get { return 2; }
        }

        public byte[] Info
        {
            get { return info; }
        }

        public IList<OldModelPolygon> Polygons
        {
            get { return polygons; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2][];
            items[0] = info;
            items[1] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 8);
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
