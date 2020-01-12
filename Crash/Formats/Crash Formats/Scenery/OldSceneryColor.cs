namespace Crash
{
    public struct OldSceneryColor : OldModelStruct
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static OldSceneryColor Load(byte[] data)
        {
            byte r = data[0];
            byte g = data[1];
            byte b = data[2];
            return new OldSceneryColor(r, g, b);
        }

        public OldSceneryColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
