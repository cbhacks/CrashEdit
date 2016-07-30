namespace Crash
{
    public struct ProtoEntityPosition
    {
        private sbyte global;
        private sbyte x;
        private sbyte y;
        private sbyte z;

        public ProtoEntityPosition(sbyte global,sbyte x,sbyte y,sbyte z)
        {
            this.global = global;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public sbyte Global
        {
            get { return global; }
        }

        public sbyte X
        {
            get { return x; }
        }

        public sbyte Y
        {
            get { return y; }
        }

        public sbyte Z
        {
            get { return z; }
        }
    }
}
