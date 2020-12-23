using System.Collections.Generic;

namespace Crash
{
    [EntityPropertyType(5)]
    public sealed class EntitySettingPropertyLoader : EntityBasicPropertyLoader<EntitySetting>
    {
        protected override byte ElementSize => 4;

        protected override EntitySetting LoadElement(byte[] data)
        {
            return new EntitySetting(BitConv.FromInt32(data,0));
        }

        protected override EntityProperty Load(IEnumerable<EntityPropertyRow<EntitySetting>> rows)
        {
            return new EntitySettingProperty(rows);
        }
    }
}
