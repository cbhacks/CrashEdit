using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(1)]
    public sealed class EntityUInt8PropertyLoader : EntityBasicPropertyLoader<byte>
    {
        protected override byte ElementSize => 1;

        protected override byte LoadElement(byte[] data)
        {
            return data[0];
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<byte>> rows)
        {
            return new EntityUInt8Property(rows);
        }
    }
}
