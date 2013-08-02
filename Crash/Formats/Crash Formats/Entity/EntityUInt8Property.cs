namespace Crash
{
    public sealed class EntityUInt8Property : EntityBasicProperty<byte>
    {
        public EntityUInt8Property(byte[,] values) : base(values)
        {
        }

        public override byte Type
        {
            get { return 1; }
        }

        public override byte ElementSize
        {
            get { return 1; }
        }

        protected override void SaveElement(byte[] data,byte value)
        {
            data[0] = value;
        }
    }
}
