namespace CrashEdit.Crash
{
    public class SpriteTexture2(int packed1, int packed2, int packed3, int packed4)
    {
        public int PackedValue1 = packed1;
        public int PackedValue2 = packed2;
        public int PackedValue3 = packed3;
        public int PackedValue4 = packed4;

        public int R => (PackedValue1) & 0xff;
        public int G => (PackedValue1 >> 8) & 0xff;
        public int B => (PackedValue1 >> 16) & 0xff;
        public int Unk1 => (PackedValue1 >> 24) & 0x1;
        public int Blend => (PackedValue1 >> 25) & 0x1;
        public int PrimType => (PackedValue1 >> 26) & 0x3f;
        public int U1 => (PackedValue2) & 0xff;
        public int V1 => (PackedValue2 >> 8) & 0xff;
        public int ClutX => (PackedValue2 >> 16) & 0xf;
        public int Unk2 => (PackedValue2 >> 20) & 0x3;
        public int ClutY => (PackedValue2 >> 22) & 0x7f;
        public int Unk3 => (PackedValue2 >> 29) & 0x7;
        public int U2 => (PackedValue3) & 0xff;
        public int V2 => (PackedValue3 >> 8) & 0xff;
        public int Segment => (PackedValue3 >> 16) & 0x3;
        public int Unk4 => (PackedValue3 >> 18) & 0x7;
        public int Additive => (PackedValue3 >> 21) & 0x1;
        public int Unk5 => (PackedValue3 >> 22) & 0x1;
        public int ColorMode => (PackedValue3 >> 23) & 0x3;
        public int Unk6 => (PackedValue3 >> 25) & 0x7f;
        public int U3 => (PackedValue4) & 0xff;
        public int V3 => (PackedValue4 >> 8) & 0xff;
        public int U4 => (PackedValue4 >> 16) & 0xff;
        public int V4 => (PackedValue4 >> 24) & 0xff;
    }
}
