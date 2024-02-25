namespace CrashEdit.Crash
{
    public sealed class ProtoZoneEntry : Entry
    {
        public ProtoZoneEntry(byte[] header, byte[] layout, IEnumerable<OldCamera> cameras, IEnumerable<ProtoEntity> entities, int eid)
            : base(eid)
        {
            Header = header;
            Layout = layout;
            Cameras.AddRange(cameras);
            Entities.AddRange(entities);
        }

        public override string Title => $"Proto Zone ({EName})";
        public override string ImageKey => "ThingViolet";

        public override int Type => 7;

        [SubresourceSlot]
        public byte[] Header { get; set; }

        [SubresourceSlot]
        public byte[] Layout { get; set; }

        [SubresourceList]
        public List<OldCamera> Cameras { get; } = new List<OldCamera>();

        [SubresourceList]
        public List<ProtoEntity> Entities { get; } = new List<ProtoEntity>();

        public int HeaderCount
        {
            get => BitConv.FromInt32(Header, 0x204);
            set => BitConv.ToInt32(Header, 0x204, value);
        }

        public int CameraCount
        {
            get => BitConv.FromInt32(Header, 0x208);
            set => BitConv.ToInt32(Header, 0x208, value);
        }

        public int EntityCount
        {
            get => BitConv.FromInt32(Header, 0x20C);
            set => BitConv.ToInt32(Header, 0x20C, value);
        }

        public int ZoneCount
        {
            get => BitConv.FromInt32(Header, 0x210);
            set => BitConv.ToInt32(Header, 0x210, value);
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[2 + Entities.Count + Cameras.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0; i < Cameras.Count; i++)
            {
                items[2 + i] = Cameras[i].Save();
            }
            for (int i = 0; i < Entities.Count; i++)
            {
                items[2 + Cameras.Count + i] = Entities[i].Save();
            }
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
