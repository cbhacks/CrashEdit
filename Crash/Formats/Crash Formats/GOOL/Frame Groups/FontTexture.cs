namespace CrashEdit.Crash
{
    public class FontTexture(int packed1, int packed2, short width, short height) : SpriteTexture(packed1, packed2)
    {
        public short Width { get; set; } = width;
        public short Height { get; set; } = height;
    }
}
