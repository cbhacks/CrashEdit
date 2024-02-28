namespace CrashEdit.Crash
{
    public sealed class SLSTSource
    {
        public static SLSTSource Load(byte[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Length < 4)
            {
                ErrorManager.SignalError("SLSTSource: Data is too short");
            }
            short count = BitConv.FromInt16(data, 0);
            short type = BitConv.FromInt16(data, 2);
            if (count < 0)
            {
                ErrorManager.SignalError("SLSTSource: Value count is negative");
            }
            if (data.Length < 4 + 2 * count)
            {
                ErrorManager.SignalError("SLSTSource: Data is too short");
            }
            if (type != 0)
            {
                ErrorManager.SignalError("SLSTSource: Type is wrong");
            }
            SLSTPolygonID[] polygons = new SLSTPolygonID[count];
            for (int i = 0; i < count; i++)
            {
                polygons[i] = new SLSTPolygonID(BitConv.FromInt16(data, 4 + i * 2));
            }
            return new SLSTSource(polygons);
        }

        private List<SLSTPolygonID> polygons;

        public SLSTSource(IEnumerable<SLSTPolygonID> polygons)
        {
            ArgumentNullException.ThrowIfNull(polygons);
            this.polygons = new List<SLSTPolygonID>(polygons);
        }

        public IList<SLSTPolygonID> Polygons => polygons;

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
