using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntityPropertyTypeAttribute : Attribute
    {
        private byte type;

        public EntityPropertyTypeAttribute(byte type)
        {
            this.type = type;
        }

        public byte Type
        {
            get { return type; }
        }
    }
}
