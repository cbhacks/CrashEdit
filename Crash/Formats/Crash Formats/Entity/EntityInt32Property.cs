namespace Crash
{
    public sealed class EntityInt32Property : EntityBasicProperty<int>
    {
        public EntityInt32Property(int[,] values) : base(values)
        {
        }

        public override byte Type
        {
            get { return 19; }
        }

        public override byte ElementSize
        {
            get { return 4; }
        }

        protected override void SaveElement(byte[] data,int value)
        {
            BitConv.ToInt32(data,0,value);
        }
    }
}
