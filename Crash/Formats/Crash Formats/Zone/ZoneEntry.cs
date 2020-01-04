using System.Collections.Generic;

namespace Crash
{
    public sealed class ZoneEntry : Entry
    {
        private List<Entity> entities;

        public ZoneEntry(byte[] header,byte[] layout,IEnumerable<Entity> entities,int eid) : base(eid)
        {
            Header = header;
            Layout = layout;
            this.entities = new List<Entity>(entities);
        }

        public override int Type => 7;
        public byte[] Header { get; }
        public byte[] Layout { get; }
        public IList<Entity> Entities => entities;

        public int InfoCount
        {
            get => BitConv.FromInt32(Header,0x184);
            set => BitConv.ToInt32(Header,0x184,value);
        }

        public int CameraCount
        {
            get => BitConv.FromInt32(Header,0x188);
            set => BitConv.ToInt32(Header,0x188,value);
        }

        public int EntityCount
        {
            get => BitConv.FromInt32(Header,0x18C);
            set => BitConv.ToInt32(Header,0x18C,value);
        }

        public int ZoneCount
        {
            get => BitConv.FromInt32(Header,0x190);
            set => BitConv.ToInt32(Header,0x190,value);
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2 + entities.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0;i < entities.Count;i++)
            {
                items[2 + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
