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

        public ushort LightMatrixL11 { get => GetZoneMatrixElement(0); set => SetZoneMatrixElement(0, value); }
        public ushort LightMatrixL12 { get => GetZoneMatrixElement(1); set => SetZoneMatrixElement(1, value); }
        public ushort LightMatrixL13 { get => GetZoneMatrixElement(2); set => SetZoneMatrixElement(2, value); }
        public ushort LightMatrixL21 { get => GetZoneMatrixElement(3); set => SetZoneMatrixElement(3, value); }
        public ushort LightMatrixL22 { get => GetZoneMatrixElement(4); set => SetZoneMatrixElement(4, value); }
        public ushort LightMatrixL23 { get => GetZoneMatrixElement(5); set => SetZoneMatrixElement(5, value); }
        public ushort LightMatrixL31 { get => GetZoneMatrixElement(6); set => SetZoneMatrixElement(6, value); }
        public ushort LightMatrixL32 { get => GetZoneMatrixElement(7); set => SetZoneMatrixElement(7, value); }
        public ushort LightMatrixL33 { get => GetZoneMatrixElement(8); set => SetZoneMatrixElement(8, value); }
        public ushort ColorBaseR { get => GetZoneMatrixElement(9); set => SetZoneMatrixElement(9, value); }
        public ushort ColorBaseG { get => GetZoneMatrixElement(10); set => SetZoneMatrixElement(10, value); }
        public ushort ColorBaseB { get => GetZoneMatrixElement(11); set => SetZoneMatrixElement(11, value); }
        public ushort ColorMatrixLR1 { get => GetZoneMatrixElement(12); set => SetZoneMatrixElement(12, value); }
        public ushort ColorMatrixLR2 { get => GetZoneMatrixElement(13); set => SetZoneMatrixElement(13, value); }
        public ushort ColorMatrixLR3 { get => GetZoneMatrixElement(14); set => SetZoneMatrixElement(14, value); }
        public ushort ColorMatrixLG1 { get => GetZoneMatrixElement(15); set => SetZoneMatrixElement(15, value); }
        public ushort ColorMatrixLG2 { get => GetZoneMatrixElement(16); set => SetZoneMatrixElement(16, value); }
        public ushort ColorMatrixLG3 { get => GetZoneMatrixElement(17); set => SetZoneMatrixElement(17, value); }
        public ushort ColorMatrixLB1 { get => GetZoneMatrixElement(18); set => SetZoneMatrixElement(18, value); }
        public ushort ColorMatrixLB2 { get => GetZoneMatrixElement(19); set => SetZoneMatrixElement(19, value); }
        public ushort ColorMatrixLB3 { get => GetZoneMatrixElement(20); set => SetZoneMatrixElement(20, value); }
        public ushort ColorIntR { get => GetZoneMatrixElement(21); set => SetZoneMatrixElement(21, value); }
        public ushort ColorIntG { get => GetZoneMatrixElement(22); set => SetZoneMatrixElement(22, value); }
        public ushort ColorIntB { get => GetZoneMatrixElement(23); set => SetZoneMatrixElement(23, value); }

        public ushort PlayerLightMatrixL11 { get => GetZonePlayerMatrixElement(0); set => SetZonePlayerMatrixElement(0, value); }
        public ushort PlayerLightMatrixL12 { get => GetZonePlayerMatrixElement(1); set => SetZonePlayerMatrixElement(1, value); }
        public ushort PlayerLightMatrixL13 { get => GetZonePlayerMatrixElement(2); set => SetZonePlayerMatrixElement(2, value); }
        public ushort PlayerLightMatrixL21 { get => GetZonePlayerMatrixElement(3); set => SetZonePlayerMatrixElement(3, value); }
        public ushort PlayerLightMatrixL22 { get => GetZonePlayerMatrixElement(4); set => SetZonePlayerMatrixElement(4, value); }
        public ushort PlayerLightMatrixL23 { get => GetZonePlayerMatrixElement(5); set => SetZonePlayerMatrixElement(5, value); }
        public ushort PlayerLightMatrixL31 { get => GetZonePlayerMatrixElement(6); set => SetZonePlayerMatrixElement(6, value); }
        public ushort PlayerLightMatrixL32 { get => GetZonePlayerMatrixElement(7); set => SetZonePlayerMatrixElement(7, value); }
        public ushort PlayerLightMatrixL33 { get => GetZonePlayerMatrixElement(8); set => SetZonePlayerMatrixElement(8, value); }
        public ushort PlayerColorBaseR { get => GetZonePlayerMatrixElement(9); set => SetZonePlayerMatrixElement(9, value); }
        public ushort PlayerColorBaseG { get => GetZonePlayerMatrixElement(10); set => SetZonePlayerMatrixElement(10, value); }
        public ushort PlayerColorBaseB { get => GetZonePlayerMatrixElement(11); set => SetZonePlayerMatrixElement(11, value); }
        public ushort PlayerColorMatrixLR1 { get => GetZonePlayerMatrixElement(12); set => SetZonePlayerMatrixElement(12, value); }
        public ushort PlayerColorMatrixLR2 { get => GetZonePlayerMatrixElement(13); set => SetZonePlayerMatrixElement(13, value); }
        public ushort PlayerColorMatrixLR3 { get => GetZonePlayerMatrixElement(14); set => SetZonePlayerMatrixElement(14, value); }
        public ushort PlayerColorMatrixLG1 { get => GetZonePlayerMatrixElement(15); set => SetZonePlayerMatrixElement(15, value); }
        public ushort PlayerColorMatrixLG2 { get => GetZonePlayerMatrixElement(16); set => SetZonePlayerMatrixElement(16, value); }
        public ushort PlayerColorMatrixLG3 { get => GetZonePlayerMatrixElement(17); set => SetZonePlayerMatrixElement(17, value); }
        public ushort PlayerColorMatrixLB1 { get => GetZonePlayerMatrixElement(18); set => SetZonePlayerMatrixElement(18, value); }
        public ushort PlayerColorMatrixLB2 { get => GetZonePlayerMatrixElement(19); set => SetZonePlayerMatrixElement(19, value); }
        public ushort PlayerColorMatrixLB3 { get => GetZonePlayerMatrixElement(20); set => SetZonePlayerMatrixElement(20, value); }
        public ushort PlayerColorIntR { get => GetZonePlayerMatrixElement(21); set => SetZonePlayerMatrixElement(21, value); }
        public ushort PlayerColorIntG { get => GetZonePlayerMatrixElement(22); set => SetZonePlayerMatrixElement(22, value); }
        public ushort PlayerColorIntB { get => GetZonePlayerMatrixElement(23); set => SetZonePlayerMatrixElement(23, value); }

        public ushort GetZoneMatrixElement(int index)
        {
            return BitConv.FromUInt16(Header, 0x318 + index * 2);
        }

        public void SetZoneMatrixElement(int index, ushort value)
        {
            BitConv.ToInt16(Header, 0x318 + index * 2, (short)value);
        }

        public ushort GetZonePlayerMatrixElement(int index)
        {
            return BitConv.FromUInt16(Header, 0x318 + (index + 24) * 2);
        }

        public void SetZonePlayerMatrixElement(int index, ushort value)
        {
            BitConv.ToInt16(Header, 0x318 + (index + 24) * 2, (short)value);
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
