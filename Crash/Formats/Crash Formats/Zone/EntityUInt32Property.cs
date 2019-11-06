using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityUInt32Property : EntityBasicProperty<uint>
    {
        public EntityUInt32Property()
        {
        }

        public EntityUInt32Property(IEnumerable<EntityPropertyRow<uint>> rows) : base(rows)
        {
        }

        public override byte Type => 3;
        public override byte ElementSize => 4;

        protected override void SaveElement(byte[] data,uint value)
        {
            BitConv.ToInt32(data,0,(int)value);
        }
    }
}
