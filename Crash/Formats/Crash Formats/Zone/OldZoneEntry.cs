namespace CrashEdit.Crash
{
    public sealed class OldZoneEntry : Entry
    {
        public OldZoneEntry(byte[] header, byte[] layout, IEnumerable<OldCamera> cameras, IEnumerable<OldEntity> entities, int eid)
            : base(eid)
        {
            Header = header;
            Layout = layout;
            Cameras.AddRange(cameras);
            Entities.AddRange(entities);
        }

        public override string Title => $"Old Zone ({EName})";
        public override string ImageKey => "ThingViolet";

        public override int Type => 7;

        [SubresourceSlot]
        public byte[] Header { get; set; }

        [SubresourceSlot]
        public byte[] Layout { get; set; }

        [SubresourceList]
        public List<OldCamera> Cameras { get; } = new List<OldCamera>();

        [SubresourceList]
        public List<OldEntity> Entities { get; } = new List<OldEntity>();

        public int WorldCount
        {
            get => BitConv.FromInt32(Header, 0);
            set => BitConv.ToInt32(Header, 0, value);
        }

        public int HeaderCount
        {
            get => BitConv.FromInt32(Header, 0x204);
            set => BitConv.ToInt32(Header, 0x204, value);
        }

        public int ZoneCount
        {
            get => BitConv.FromInt32(Header, 0x210);
            set => BitConv.ToInt32(Header, 0x210, value);
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

        public ushort CollisionDepthX
        {
            get => BitConv.FromUInt16(Layout, 0x1E);
            set => BitConv.ToInt16(Layout, 0x1E, (short)value);
        }

        public ushort CollisionDepthY
        {
            get => BitConv.FromUInt16(Layout, 0x20);
            set => BitConv.ToInt16(Layout, 0x20, (short)value);
        }

        public ushort CollisionDepthZ
        {
            get => BitConv.FromUInt16(Layout, 0x22);
            set => BitConv.ToInt16(Layout, 0x22, (short)value);
        }

        public override UnprocessedEntry Unprocess()
        {
            BitConv.ToInt32(Header, 0x208, Cameras.Count);
            BitConv.ToInt32(Header, 0x20C, Entities.Count);
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
