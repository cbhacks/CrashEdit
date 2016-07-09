using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityT4Property : EntityBasicProperty<int>
    {
        public EntityT4Property()
        {
        }

        public EntityT4Property(IEnumerable<EntityPropertyRow<int>> rows) : base(rows)
        {
        }

        public override byte Type
        {
            get { return 4; }
        }

        public override byte ElementSize
        {
            get { return 4; }
        }

        protected override void SaveElement(byte[] data,int value)
        {
            BitConv.ToInt32(data,0,value);
        }
    }
}
