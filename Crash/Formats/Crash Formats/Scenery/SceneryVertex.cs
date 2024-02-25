namespace Crash
{
    public class SceneryVertex
    {
        public static SceneryVertex Load(byte[] xydata, byte[] zdata, bool is_c3 = false)
        {
            if (xydata == null)
                throw new ArgumentNullException(nameof(xydata));
            if (zdata == null)
                throw new ArgumentNullException(nameof(zdata));
            if (xydata.Length != 4)
                throw new ArgumentException("Value must be 4 bytes long.", nameof(xydata));
            if (zdata.Length != 2)
                throw new ArgumentException("Value must be 2 bytes long.", nameof(zdata));
            int xy = BitConv.FromInt32(xydata, 0);
            ushort z = (ushort)BitConv.FromInt16(zdata, 0);
            ushort y = (ushort)(xy >> 16);
            ushort x = (ushort)xy;
            int unknownx = x & 0xF;
            int unknowny = y & 0xF;
            int unknownz = z & 0xF;
            if (!is_c3)
                return new SceneryVertex((short)x >> 4, (short)y >> 4, (short)z >> 4, unknownx, unknowny, unknownz, is_c3);
            else
                return new SceneryVertex(x >> 4, y >> 4, z >> 4, unknownx, unknowny, unknownz, is_c3);
        }

        public SceneryVertex(int x, int y, int z, int unknownx, int unknowny, int unknownz, bool is_c3 = false)
        {
            int min = is_c3 ? 0 : -0x800;
            int max = is_c3 ? 0xFFF : 0x7FF;
            if (x < min || x > max)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < min || y > max)
                throw new ArgumentOutOfRangeException(nameof(y));
            if (z < min || z > max)
                throw new ArgumentOutOfRangeException(nameof(z));
            if (unknownx < 0 || unknownx > 0xF)
                throw new ArgumentOutOfRangeException(nameof(unknownx));
            if (unknowny < 0 || unknowny > 0xF)
                throw new ArgumentOutOfRangeException(nameof(unknowny));
            if (unknownz < 0 || unknownz > 0xF)
                throw new ArgumentOutOfRangeException(nameof(unknownz));
            X = x;
            Y = y;
            Z = z;
            UnknownX = unknownx;
            UnknownY = unknowny;
            UnknownZ = unknownz;
            IsC3 = is_c3;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int UnknownX { get; }
        public int UnknownY { get; }
        public int UnknownZ { get; }
        public int Color => (UnknownY & 0x3) << 8 | UnknownX << 4 | UnknownZ;
        public bool IsC3 { get; }

        public byte[] SaveXY()
        {
            byte[] data = new byte[4];
            int xdata = (X << 4) | UnknownX;
            int ydata = (Y << 4) | UnknownY;
            BitConv.ToInt16(data, 0, (short)xdata);
            BitConv.ToInt16(data, 2, (short)ydata);
            return data;
        }

        public byte[] SaveZ()
        {
            byte[] data = new byte[2];
            int zdata = (Z << 4) | UnknownZ;
            BitConv.ToInt16(data, 0, (short)zdata);
            return data;
        }
    }
}
