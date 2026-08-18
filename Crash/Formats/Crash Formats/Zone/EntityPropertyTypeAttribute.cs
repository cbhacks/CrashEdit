namespace CrashEdit.Crash
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class EntityPropertyTypeAttribute(EntityPropertyType type) : Attribute
    {
        public EntityPropertyType Type { get; } = type;
    }
}
