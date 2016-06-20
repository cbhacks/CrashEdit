using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(18)]
    public sealed class EntityVictimPropertyLoader : EntityBasicPropertyLoader<EntityVictim>
    {
        protected override byte ElementSize
        {
            get { return 2; }
        }

        protected override EntityVictim LoadElement(byte[] data)
        {
            short victimid = BitConv.FromInt16(data,0);
            return new EntityVictim(victimid);
        }
        
        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntityVictim>> rows)
        {
            return new EntityVictimProperty(rows);
        }
    }
}
