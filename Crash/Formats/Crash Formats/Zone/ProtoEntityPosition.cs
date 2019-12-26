namespace Crash
{
    public struct ProtoEntityPosition
    {
        public ProtoEntityPosition(sbyte x,sbyte y,sbyte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public sbyte X { get; }
        public sbyte Y { get; }
        public sbyte Z { get; }
    }
}
