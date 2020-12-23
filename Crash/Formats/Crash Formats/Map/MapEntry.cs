using System.Collections.Generic;

namespace Crash
{
    public sealed class MapEntry : Entry
    {
        private List<OldEntity> entities;

        public MapEntry(byte[] header,byte[] layout,IEnumerable<OldEntity> entities,int eid) : base(eid)
        {
            Header = header;
            Layout = layout;
            this.entities = new List<OldEntity>(entities);
        }

        public override int Type => 17;
        public byte[] Header { get; }
        public byte[] Layout { get; }
        public IList<OldEntity> Entities => entities;

        public override UnprocessedEntry Unprocess()
        {
            BitConv.ToInt32(Header,0xC,entities.Count);
            byte[][] items = new byte[2 + entities.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0; i < entities.Count; i++)
            {
                items[2 + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
