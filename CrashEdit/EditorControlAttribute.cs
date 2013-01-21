using System;

namespace CrashEdit
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public sealed class EditorControlAttribute : Attribute
    {
        private Type type;

        public EditorControlAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
