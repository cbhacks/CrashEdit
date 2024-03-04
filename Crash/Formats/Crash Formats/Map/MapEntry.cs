namespace CrashEdit.Crash
{
    public sealed class MapEntry : Entry
    {
        public MapEntry(byte[] header, byte[] layout, IEnumerable<OldEntity> entities, int eid) : base(eid)
        {
            Header = header;
            Layout = layout;
            Entities.AddRange(entities);
        }

        public override string Title => $"Map ({EName})";
        public override string ImageKey => "ThingOrange";

        public override int Type => 17;

        [SubresourceSlot]
        public byte[] Header { get; set; }

        [SubresourceSlot]
        public byte[] Layout { get; set; }

        [SubresourceList]
        public List<OldEntity> Entities { get; } = new List<OldEntity>();

        public override UnprocessedEntry Unprocess()
        {
            BitConv.ToInt32(Header, 0xC, Entities.Count);
            byte[][] items = new byte[2 + Entities.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0; i < Entities.Count; i++)
            {
                items[2 + i] = Entities[i].Save();
            }
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
