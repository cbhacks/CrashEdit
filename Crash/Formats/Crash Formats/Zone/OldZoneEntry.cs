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
