using System.Collections.Generic;

namespace Crash
{
    public sealed class EntityPropertyRow<T>
    {
        private short? metavalue;
        private List<T> values;

        public EntityPropertyRow()
        {
            this.metavalue = null;
            this.values = new List<T>();
        }

        public short? MetaValue
        {
            get { return metavalue; }
            set { metavalue = value; }
        }

        public IList<T> Values
        {
            get { return values; }
        }
    }
}
