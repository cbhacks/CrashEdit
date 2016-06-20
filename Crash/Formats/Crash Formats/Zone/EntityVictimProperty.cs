using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityVictimProperty : EntityBasicProperty<EntityVictim>
    {
        public EntityVictimProperty()
        {
        }

        public EntityVictimProperty(IEnumerable<EntityPropertyRow<EntityVictim>> rows) : base(rows)
        {
        }

        public override byte Type
        {
            get { return 18; }
        }

        public override byte ElementSize
        {
            get { return 2; }
        }

        protected override void SaveElement(byte[] data,EntityVictim value)
        {
            BitConv.ToInt16(data,0,value.VictimID);
        }
    }
}
