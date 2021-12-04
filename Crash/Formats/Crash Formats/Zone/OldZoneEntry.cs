using System.Collections.Generic;

namespace Crash
{
    public sealed class OldZoneEntry : Entry
    {
        private List<OldCamera> cameras;
        private List<OldEntity> entities;

        public OldZoneEntry(byte[] header,byte[] layout,IEnumerable<OldCamera> cameras,IEnumerable<OldEntity> entities,int eid)
            : base(eid)
        {
            Header = header;
            Layout = layout;
            this.cameras = new List<OldCamera>(cameras);
            this.entities = new List<OldEntity>(entities);
        }

        public override int Type => 7;
        public byte[] Header { get; }
        public byte[] Layout { get; }
        public IList<OldCamera> Cameras => cameras;
        public IList<OldEntity> Entities => entities;

        public int WorldCount
        {
            get => BitConv.FromInt32(Header, 0);
            set => BitConv.ToInt32(Header, 0, value);
        }

        public int HeaderCount
        {
            get => BitConv.FromInt32(Header,0x204);
            set => BitConv.ToInt32(Header,0x204,value);
        }

        public int ZoneCount
        {
            get => BitConv.FromInt32(Header,0x210);
            set => BitConv.ToInt32(Header,0x210,value);
        }

        public int GetLinkedWorld(int idx) => BitConv.FromInt32(Header, 0x4 + idx * 0x40);
        public int GetLinkedZone(int idx) => BitConv.FromInt32(Header, 0x214 + idx * 0x4);

        public int X
        {
            get => BitConv.FromInt32(Layout, 0);
            set => BitConv.ToInt32(Layout, 0, value);
        }
        public int Y
        {
            get => BitConv.FromInt32(Layout, 4);
            set => BitConv.ToInt32(Layout, 4, value);
        }
        public int Z
        {
            get => BitConv.FromInt32(Layout, 8);
            set => BitConv.ToInt32(Layout, 8, value);
        }
        public int Width
        {
            get => BitConv.FromInt32(Layout, 12);
            set => BitConv.ToInt32(Layout, 12, value);
        }
        public int Height
        {
            get => BitConv.FromInt32(Layout, 16);
            set => BitConv.ToInt32(Layout, 0, value);
        }
        public int Depth
        {
            get => BitConv.FromInt32(Layout, 20);
            set => BitConv.ToInt32(Layout, 20, value);
        }

        public override UnprocessedEntry Unprocess()
        {
            BitConv.ToInt32(Header,0x208,cameras.Count);
            BitConv.ToInt32(Header,0x20C,entities.Count);
            byte[][] items = new byte[2 + entities.Count + cameras.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0; i < cameras.Count; i++)
            {
                items[2 + i] = cameras[i].Save();
            }
            for (int i = 0; i < entities.Count; i++)
            {
                items[2 + cameras.Count + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
