using System.Collections.Generic;

namespace CrashEdit.Crash
{
    public sealed class NewZoneEntry : Entry
    {
        public NewZoneEntry(byte[] header, byte[] layout,IEnumerable<Entity> entities,int eid) : base(eid)
        {
            Header = header;
            Layout = layout;
            Entities.AddRange(entities);
        }

        public override string Title => $"Zone ({EName})";
        public override string ImageKey => "ThingViolet";

        public override int Type => 7;

        [SubresourceSlot]
        public byte[] Header { get; set; }

        [SubresourceSlot]
        public byte[] Layout { get; set; }

        [SubresourceList]
        public List<Entity> Entities { get; } = new List<Entity>();

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
            byte[][] items = new byte [2 + Entities.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0;i < Entities.Count;i++)
            {
                items[2 + i] = Entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
