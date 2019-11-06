using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(5)]
    public sealed class EntitySettingPropertyLoader : EntityBasicPropertyLoader<EntitySetting>
    {
        protected override byte ElementSize => 4;

        protected override EntitySetting LoadElement(byte[] data)
        {
            byte a = data[0];
            int b = BitConv.FromInt24(data,1);
            return new EntitySetting(a,b);
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntitySetting>> rows)
        {
            return new EntitySettingProperty(rows);
        }
    }
}
