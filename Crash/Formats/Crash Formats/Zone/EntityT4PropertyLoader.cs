using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(4)]
    public sealed class EntityT4PropertyLoader : EntityBasicPropertyLoader<int>
    {
        protected override byte ElementSize => 4;

        protected override int LoadElement(byte[] data)
        {
            return BitConv.FromInt32(data,0);
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<int>> rows)
        {
            return new EntityT4Property(rows);
        }
    }
}
