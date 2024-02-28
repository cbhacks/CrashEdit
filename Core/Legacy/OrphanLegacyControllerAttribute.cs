namespace CrashEdit
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OrphanLegacyControllerAttribute : Attribute
    {

        public OrphanLegacyControllerAttribute(Type resType)
        {
            ArgumentNullException.ThrowIfNull(resType);

            ResourceType = resType;
        }

        public Type ResourceType { get; }

    }

}
