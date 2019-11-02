using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(3)]
    public sealed class EntityUInt32PropertyLoader : EntityBasicPropertyLoader<uint>
    {
        protected override byte ElementSize => 4;

        protected override uint LoadElement(byte[] data)
        {
            return (uint)BitConv.FromInt32(data,0);
        }
        
        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<uint>> rows)
        {
            return new EntityUInt32Property(rows);
        }
    }
}
