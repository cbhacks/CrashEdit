namespace Crash
{
    public struct FrameCollision
    {
        public FrameCollision(int u, int xo, int yo, int zo, int x1, int y1, int z1, int x2, int y2, int z2)
        {
            U = u;
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
            XO = xo;
            YO = yo;
            ZO = zo;
        }

        public int U { get; }
        public int XO { get; }
        public int YO { get; }
        public int ZO { get; }
        public int X1 { get; }
        public int Y1 { get; }
        public int Z1 { get; }
        public int X2 { get; }
        public int Y2 { get; }
        public int Z2 { get; }
    }
}
