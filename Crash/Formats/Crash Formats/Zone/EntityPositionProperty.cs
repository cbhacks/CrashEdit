using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityPositionProperty : EntityBasicProperty<EntityPosition>
    {
        public EntityPositionProperty()
        {
        }

        public EntityPositionProperty(IEnumerable<EntityPropertyRow<EntityPosition>> rows) : base(rows)
        {
        }

        public override byte Type => 6;
        public override byte ElementSize => 6;

        protected override void SaveElement(byte[] data,EntityPosition value)
        {
            BitConv.ToInt16(data,0,value.X);
            BitConv.ToInt16(data,2,value.Y);
            BitConv.ToInt16(data,4,value.Z);
        }
    }
}
