namespace Crash
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public sealed class EntryTypeAttribute : System.Attribute
    {
        private int type;

        public EntryTypeAttribute(int type)
        {
            this.type = type;
        }

        public int Type
        {
            get { return type; }
        }
    }
}
