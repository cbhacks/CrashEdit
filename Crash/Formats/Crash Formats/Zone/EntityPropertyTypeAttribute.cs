using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntityPropertyTypeAttribute : Attribute
    {
        public EntityPropertyTypeAttribute(byte type)
        {
            Type = type;
        }

        public byte Type { get; }
    }
}
