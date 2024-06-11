namespace CrashEdit.Crash
{
    public class GOOLProtoFrame : GOOLProtoFrameGroup
    {
        public long PackedValue;

        public int R => (int)((PackedValue) & 0xff);
        public int G => (int)((PackedValue >> 8) & 0xff);
        public int B => (int)((PackedValue >> 16) & 0xff);
        public int ClutX => (int)((PackedValue >> 24) & 0xf);
        public int Unk1 => (int)((PackedValue >> 28) & 0x1);
        public int BlendMode => (int)((PackedValue >> 29) & 0x3);
        public int Textured => (int)((PackedValue >> 31) & 0x1);
        public int Y => (int)((PackedValue >> 32) & 0x1f);
        public int Unk2 => (int)((PackedValue >> 37) & 0x1);
        public int ClutY => (int)((PackedValue >> 38) & 0x7f);
        public int X => (int)((PackedValue >> 45) & 0x1f);
        public int Segment => (int)((PackedValue >> 50) & 0x3);
        public int ColorMode => (int)((PackedValue >> 52) & 0x3);
        public int UV => (int)((PackedValue >> 54) & 0x3ff);

        public GOOLProtoFrame(long packed)
        {
            PackedValue = packed;
        }

        public override byte[] Save()
        {
            byte[] data = new byte[8];
            BitConv.ToInt64(data, 0, PackedValue);
            return data;
        }
    }
}
