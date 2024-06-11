namespace CrashEdit.Crash
{
    public class SpriteTexture(int packed1, int packed2)
    {
        public int PackedValue1 = packed1;
        public int PackedValue2 = packed2;

        public int R => (PackedValue1) & 0xff;
        public int G => (PackedValue1 >> 8) & 0xff;
        public int B => (PackedValue1 >> 16) & 0xff;
        public int ClutX => (PackedValue1 >> 24) & 0xf;
        public int Unk1 => (PackedValue1 >> 28) & 0x1;
        public int BlendMode => (PackedValue1 >> 29) & 0x3;
        public int Textured => (PackedValue1 >> 31) & 0x1;
        public int Y => (PackedValue2) & 0x1f;
        public int Unk2 => (PackedValue2 >> 5) & 0x1;
        public int ClutY => (PackedValue2 >> 6) & 0x7f;
        public int X => (PackedValue2 >> 13) & 0x1f;
        public int Segment => (PackedValue2 >> 18) & 0x3;
        public int ColorMode => (PackedValue2 >> 20) & 0x3;
        public int UV => (PackedValue2 >> 22) & 0x3ff;
    }
}
