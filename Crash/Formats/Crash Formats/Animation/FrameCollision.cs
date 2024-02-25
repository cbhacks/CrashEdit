namespace CrashEdit.Crash
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
            XOffset = xo;
            YOffset = yo;
            ZOffset = zo;
        }

        public int U { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int ZOffset { get; set; }
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int Z1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int Z2 { get; set; }
    }
}
