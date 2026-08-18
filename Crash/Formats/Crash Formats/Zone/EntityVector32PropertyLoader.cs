namespace CrashEdit.Crash
{
    [EntityPropertyType(EntityPropertyType.Vector32)]
    public sealed class EntityVector32PropertyLoader : EntityBasicPropertyLoader<EntityVector32>
    {
        protected override byte ElementSize => 12;

        protected override EntityVector32 LoadElement(byte[] data)
        {
            int x = BitConv.FromInt32(data, 0);
            int y = BitConv.FromInt32(data, 4);
            int z = BitConv.FromInt32(data, 8);
            return new EntityVector32(x, y, z);
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntityVector32>> rows)
        {
            return new EntityVector32Property(rows);
        }
    }
}
