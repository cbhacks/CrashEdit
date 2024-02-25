namespace Crash
{
    public sealed class EntityPropertyRow<T>
    {
        private readonly List<T> values;

        public EntityPropertyRow()
        {
            MetaValue = null;
            values = new List<T>();
        }

        public short? MetaValue { get; set; }
        public IList<T> Values => values;
    }
}
