namespace Crash
{
    public struct GOOLChar
    {
        public GOOLChar(GOOLSpriteFrame sprite,short w,short h)
        {
            Sprite = sprite;
            Width = w;
            Height = h;
        }

        public GOOLSpriteFrame Sprite { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
    }
}
