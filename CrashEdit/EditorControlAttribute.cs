namespace CrashEdit
{
    [System.AttributeUsage(System.AttributeTargets.Class,AllowMultiple = true)]
    public sealed class EditorControlAttribute : System.Attribute
    {
        private System.Type type;

        public EditorControlAttribute(System.Type type)
        {
            this.type = type;
        }

        public System.Type Type
        {
            get { return type; }
        }
    }
}
