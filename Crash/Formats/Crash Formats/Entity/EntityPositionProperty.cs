namespace Crash
{
    public sealed class EntityPositionProperty : EntityBasicProperty<EntityPosition>
    {
        public EntityPositionProperty(EntityPosition[,] values) : base(values)
        {
        }

        public override byte Type
        {
            get { return 6; }
        }

        public override byte ElementSize
        {
            get { return 6; }
        }

        protected override void SaveElement(byte[] data,EntityPosition value)
        {
            BitConv.ToInt16(data,0,value.X);
            BitConv.ToInt16(data,2,value.Y);
            BitConv.ToInt16(data,4,value.Z);
        }
    }
}
