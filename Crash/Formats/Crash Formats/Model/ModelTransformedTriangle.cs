namespace Crash
{
    public struct ModelTransformedTriangle
    {
        public ModelTransformedTriangle(int v1,int v2,int v3,int c1,int c2,int c3,int tex,int type,int subtype,bool animated)
        {
            Vertex = new int[3] { v1, v2, v3 };
            Color = new int[3] { c1, c2, c3 };
            Tex = tex;
            Type = type;
            Subtype = subtype;
            Animated = animated;
        }

        public int[] Vertex { get; }
        public int[] Color { get; }
        public int Tex { get; }
        public int Type { get; }
        public int Subtype { get; }
        public bool Animated { get; }
    }
}
