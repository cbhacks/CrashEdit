namespace CrashEdit.Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ChunkTypeAttribute : Attribute
    {
        public ChunkTypeAttribute(short type)
        {
            Type = type;
        }

        public short Type { get; }
    }
}
