namespace Crash
{
    public struct GOOLSpriteFrame
    {
        public GOOLSpriteFrame(int clut,int texture)
        {
            CLUT = clut;
            Texture = texture;
        }

        public int CLUT { get; set; }
        public int Texture { get; set; }
    }
}
