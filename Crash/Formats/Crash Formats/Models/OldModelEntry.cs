using System;
using System.Collections.Generic;

namespace Crash
{
    public sealed class OldModelEntry : Entry
    {
        private List<OldModelPolygon> polygons;

        public OldModelEntry(byte[] info,IEnumerable<OldModelPolygon> polygons,int eid) : base(eid)
        {
            if (polygons == null)
                throw new ArgumentNullException("polygons");
            Info = info ?? throw new ArgumentNullException("info");
            this.polygons = new List<OldModelPolygon>(polygons);
        }

        public override int Type => 2;
        public byte[] Info { get; }
        public IList<OldModelPolygon> Polygons => polygons;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2][];
            items[0] = Info;
            items[1] = new byte [polygons.Count * 8];
            for (int i = 0;i < polygons.Count;i++)
            {
                polygons[i].Save().CopyTo(items[1],i * 8);
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
