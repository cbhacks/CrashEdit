namespace Crash
{
    public interface OldModelStruct
    {
    }
    public struct OldModelColor : OldModelStruct
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }

        public static OldModelColor Load(byte[] data)
        {
            byte r = data[0];
            byte g = data[1];
            byte b = data[2];
            return new OldModelColor(r, g, b);
        }

        public OldModelColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
