using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class PackingException : Exception
    {
        int eid;

        public PackingException(int eid) : base("The data to be saved was too large to fit into its parent container.")
        {
            this.eid = eid;
        }

        public PackingException(int eid,string message) : base(message)
        {
            this.eid = eid;
        }

        public PackingException(int eid,string message,Exception inner) : base(message,inner)
        {
            this.eid = eid;
        }

        protected PackingException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }

        public int EID
        {
            get { return eid; }
        }
    }
}
