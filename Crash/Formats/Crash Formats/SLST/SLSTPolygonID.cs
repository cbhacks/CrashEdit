namespace CrashEdit.Crash
{
    public struct SLSTPolygonID
    {
        short poly;

        public SLSTPolygonID(short poly)
        {
            this.poly = poly;
        }

        public SLSTPolygonID(int id, int state, int world)
        {
            poly = unchecked((short)(id & 0x7FF | (state << 11) | (world << 13)));
        }

        public int Poly => poly;
        public int ID => poly & 0x7FF;
        public int State => (poly >> 11) & 0x3;
        public int World => (poly >> 13) & 0x7;
    }
}
