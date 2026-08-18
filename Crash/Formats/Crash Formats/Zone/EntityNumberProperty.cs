namespace CrashEdit.Crash
{
    public sealed class EntityNumberProperty : EntityBasicProperty<EntityNumber>
    {
        public EntityNumberProperty()
        {
        }

        public EntityNumberProperty(IEnumerable<EntityPropertyRow<EntityNumber>> rows) : base(rows)
        {
        }

        public override EntityPropertyType Type => EntityPropertyType.Number;
        public override byte ElementSize => 4;

        protected override void SaveElement(byte[] data, EntityNumber value)
        {
            BitConv.ToInt32(data, 0, value.Value);
        }
    }
}
