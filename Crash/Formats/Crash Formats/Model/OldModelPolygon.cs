namespace CrashEdit.Crash
{
    public struct OldModelPolygon
    {
        public static OldModelPolygon Load(byte[] data)
        {
            ArgumentNullException.ThrowIfNull(data);
            if (data.Length != 8)
                throw new ArgumentException("Value must be 8 bytes long.", nameof(data));
            short vertexa = BitConv.FromInt16(data, 0);
            short vertexb = BitConv.FromInt16(data, 2);
            short vertexc = BitConv.FromInt16(data, 4);
            short texinfo = (short)(BitConv.FromInt16(data, 6) & 0x7FFF);
            bool nolight = (BitConv.FromInt16(data, 6) & 0x8000) != 0;
            return new OldModelPolygon(vertexa, vertexb, vertexc, texinfo, nolight);
        }

        public OldModelPolygon(short vertexa, short vertexb, short vertexc, short texinfo, bool nolight)
        {
            VertexA = vertexa;
            VertexB = vertexb;
            VertexC = vertexc;
            TexInfo = texinfo;
            NoLight = nolight;
        }

        public short VertexA { get; }
        public short VertexB { get; }
        public short VertexC { get; }
        public short TexInfo { get; }
        public bool NoLight { get; }

        public byte[] Save()
        {
            byte[] data = new byte[8];
            BitConv.ToInt16(data, 0, VertexA);
            BitConv.ToInt16(data, 2, VertexB);
            BitConv.ToInt16(data, 4, VertexC);
            BitConv.ToInt16(data, 6, (short)(TexInfo | (NoLight ? 0x8000 : 0)));
            return data;
        }
    }
}
