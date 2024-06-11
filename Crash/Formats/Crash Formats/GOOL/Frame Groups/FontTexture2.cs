namespace CrashEdit.Crash
{
    public class FontTexture2(int packed1, int packed2, int packed3, int packed4, short width, short height) : SpriteTexture2(packed1, packed2, packed3, packed4)
    {
        public short Width { get; set; } = width;
        public short Height { get; set; } = height;
    }
}
