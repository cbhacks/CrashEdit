namespace CrashEdit.Crash
{
    public class ImageTexture(int packed1, int packed2, short x1, short y1, short x2, short y2) : SpriteTexture(packed1, packed2)
    {
        public short X1 { get; set; } = x1;
        public short Y1 { get; set; } = y1;
        public short X2 { get; set; } = x2;
        public short Y2 { get; set; } = y2;
    }
}
