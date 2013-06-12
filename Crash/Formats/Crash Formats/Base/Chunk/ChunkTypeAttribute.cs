using System;

namespace Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ChunkTypeAttribute : Attribute
    {
        private short type;

        public ChunkTypeAttribute(short type)
        {
            this.type = type;
        }

        public short Type
        {
            get { return type; }
        }
    }
}
