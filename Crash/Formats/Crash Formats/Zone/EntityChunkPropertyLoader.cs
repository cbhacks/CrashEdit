namespace CrashEdit.Crash
{
    [EntityPropertyType(EntityPropertyType.Chunk)]
    public sealed class EntityChunkPropertyLoader : EntityBasicPropertyLoader<int>
    {
        protected override byte ElementSize => 4;

        protected override int LoadElement(byte[] data)
        {
            return BitConv.FromInt32(data, 0);
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<int>> rows)
        {
            return new EntityChunkProperty(rows);
        }
    }
}
