using System.Runtime.CompilerServices;

namespace CrashEdit
{

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SubresourceListAttribute : SubresourceAttribute
    {

        public SubresourceListAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }

        public int Order { get; }

    }

}
