using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityPropertyRow<T>
    {
        private List<T> values;

        public EntityPropertyRow()
        {
            MetaValue = null;
            values = new List<T>();
        }

        public short? MetaValue { get; set; }
        public IList<T> Values => values;
    }
}
