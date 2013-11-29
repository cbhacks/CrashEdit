using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(18)]
    public sealed class EntityInt16PropertyLoader : EntityBasicPropertyLoader<short>
    {
        protected override byte ElementSize
        {
            get { return 2; }
        }

        protected override short LoadElement(byte[] data)
        {
            return BitConv.FromInt16(data,0);
        }
        
        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<short>> rows)
        {
            return new EntityInt16Property(rows);
        }
    }
}
