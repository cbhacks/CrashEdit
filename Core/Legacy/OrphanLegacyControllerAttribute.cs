namespace CrashEdit
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OrphanLegacyControllerAttribute : Attribute
    {

        public OrphanLegacyControllerAttribute(Type resType)
        {
            if (resType == null)
                throw new ArgumentNullException();

            ResourceType = resType;
        }

        public Type ResourceType { get; }

    }

}
