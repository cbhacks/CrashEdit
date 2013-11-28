namespace Crash
{
    public sealed class EntityInt16Property : EntityBasicProperty<short>
    {
        public EntityInt16Property(short[,] values) : base(values)
        {
        }

        public override byte Type
        {
            get { return 18; }
        }

        public override byte ElementSize
        {
            get { return 2; }
        }

        protected override void SaveElement(byte[] data,short value)
        {
            BitConv.ToInt16(data,0,value);
        }
    }
}
