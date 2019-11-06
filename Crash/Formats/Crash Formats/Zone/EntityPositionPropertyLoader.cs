using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(6)]
    public sealed class EntityPositionPropertyLoader : EntityBasicPropertyLoader<EntityPosition>
    {
        protected override byte ElementSize => 6;

        protected override EntityPosition LoadElement(byte[] data)
        {
            short x = BitConv.FromInt16(data,0);
            short y = BitConv.FromInt16(data,2);
            short z = BitConv.FromInt16(data,4);
            return new EntityPosition(x,y,z);
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntityPosition>> rows)
        {
            return new EntityPositionProperty(rows);
        }
    }
}
