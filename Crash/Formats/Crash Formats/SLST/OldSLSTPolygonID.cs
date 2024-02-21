namespace Crash
{
    public struct OldSLSTPolygonID
    {
        short poly;

        public OldSLSTPolygonID(short poly)
        {
            this.poly = poly;
        }

        public OldSLSTPolygonID(int id, int world, bool copy)
        {
            poly = unchecked((short)(id & 0xFFF | (world << 12) | ((copy ? 1 : 0) << 15)));
        }

        public int Poly => poly;
        public int ID => poly & 0xFFF;
        public int World => (poly >> 12) & 0x7;
        public int Copy => (poly >> 15) & 0x1;
    }
}
