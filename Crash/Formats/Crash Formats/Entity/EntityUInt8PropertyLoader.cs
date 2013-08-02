namespace Crash
{
    [EntityPropertyType(1)]
    public sealed class EntityUInt8PropertyLoader : EntityBasicPropertyLoader<byte>
    {
        protected override byte ElementSize
        {
            get { return 1; }
        }

        protected override byte LoadElement(byte[] data)
        {
            return data[0];
        }

        protected override EntityProperty Load(byte[,] values)
        {
            return new EntityUInt8Property(values);
        }
    }
}
