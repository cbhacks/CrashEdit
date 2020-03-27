namespace Crash
{
    public struct OldSceneryColor : OldModelStruct
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public bool N { get; }

        public static OldSceneryColor Load(byte[] data)
        {
            byte r = data[0];
            byte g = data[1];
            byte b = data[2];
            bool n = (data[3] & 0x10) != 0;
            return new OldSceneryColor(r, g, b, n);
        }

        public OldSceneryColor(byte r, byte g, byte b, bool n)
        {
            R = r;
            G = g;
            B = b;
            N = n;
        }
    }
}
