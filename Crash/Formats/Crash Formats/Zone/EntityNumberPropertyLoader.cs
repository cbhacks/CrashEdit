namespace CrashEdit.Crash
{
    [EntityPropertyType(EntityPropertyType.Number)]
    public sealed class EntityNumberPropertyLoader : EntityBasicPropertyLoader<EntityNumber>
    {
        protected override byte ElementSize => 4;

        protected override EntityNumber LoadElement(byte[] data)
        {
            return new EntityNumber(BitConv.FromInt32(data, 0));
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntityNumber>> rows)
        {
            return new EntityNumberProperty(rows);
        }
    }
}
