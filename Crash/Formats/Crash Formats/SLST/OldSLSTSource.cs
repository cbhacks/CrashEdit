namespace Crash
{
    public sealed class OldSLSTSource
    {
        public static OldSLSTSource Load(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length < 4)
            {
                ErrorManager.SignalError("OldSLSTSource: Data is too short");
            }
            short count = BitConv.FromInt16(data, 0);
            short type = BitConv.FromInt16(data, 2);
            if (count < 0)
            {
                ErrorManager.SignalError("OldSLSTSource: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("OldSLSTSource: Data is too short");
            }
            if (type != 0)
            {
                ErrorManager.SignalError("OldSLSTSource: Type is wrong");
            }
            OldSLSTPolygonID[] polygons = new OldSLSTPolygonID[count];
            for (int i = 0; i < count; i++)
            {
                polygons[i] = new OldSLSTPolygonID(BitConv.FromInt16(data, 4 + i * 2));
            }
            return new OldSLSTSource(polygons);
        }

        private readonly List<OldSLSTPolygonID> polygons;

        public OldSLSTSource(IEnumerable<OldSLSTPolygonID> polygons)
        {
            if (polygons == null)
                throw new ArgumentNullException(nameof(polygons));
            this.polygons = new List<OldSLSTPolygonID>(polygons);
        }

        public IList<OldSLSTPolygonID> Polygons => polygons;

        public byte[] Save()
        {
            byte[] data = new byte[4 + polygons.Count * 2];
            if (polygons.Count > short.MaxValue)
            {
                throw new Exception();
            }
            BitConv.ToInt16(data, 0, (short)polygons.Count);
            BitConv.ToInt16(data, 2, 0);
            for (int i = 0; i < polygons.Count; i++)
            {
                BitConv.ToInt16(data, 4 + i * 2, (short)polygons[i].Poly);
            }
            return data;
        }
    }
}
