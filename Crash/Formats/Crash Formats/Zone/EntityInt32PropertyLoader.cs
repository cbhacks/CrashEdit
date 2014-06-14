using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(19)]
    public sealed class EntityInt32PropertyLoader : EntityBasicPropertyLoader<int>
    {
        protected override byte ElementSize
        {
            get { return 4; }
        }

        protected override int LoadElement(byte[] data)
        {
            return BitConv.FromInt32(data,0);
        }
        
        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<int>> rows)
        {
            return new EntityInt32Property(rows);
        }
    }
}
