namespace Crash
{
    public struct ProtoEntityPosition
    {
        public ProtoEntityPosition(sbyte global,sbyte x,sbyte y,sbyte z)
        {
            Global = global;
            X = x;
            Y = y;
            Z = z;
        }

        public sbyte Global { get; }
        public sbyte X { get; }
        public sbyte Y { get; }
        public sbyte Z { get; }
    }
}
