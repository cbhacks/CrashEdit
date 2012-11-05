namespace Crash
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public sealed class ChunkTypeAttribute : System.Attribute
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
