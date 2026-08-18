namespace CrashEdit.Crash
{
    public sealed class EntityVector32Property : EntityBasicProperty<EntityVector32>
    {
        public EntityVector32Property()
        {
        }

        public EntityVector32Property(IEnumerable<EntityPropertyRow<EntityVector32>> rows) : base(rows)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Vector32;
        public override byte ElementSize => 12;

        protected override void SaveElement(byte[] data, EntityVector32 value)
        {
            BitConv.ToInt32(data, 0, value.X);
            BitConv.ToInt32(data, 4, value.Y);
            BitConv.ToInt32(data, 8, value.Z);
        }
    }
}
