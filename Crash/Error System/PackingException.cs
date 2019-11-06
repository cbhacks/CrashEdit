using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class PackingException : Exception
    {
        public PackingException(int eid) : base("The data to be saved was too large to fit into its parent container.")
        {
            EID = eid;
        }

        public PackingException(int eid,string message) : base(message)
        {
            EID = eid;
        }

        public PackingException(int eid,string message,Exception inner) : base(message,inner)
        {
            EID = eid;
        }

        protected PackingException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }

        public int EID { get; }
    }
}
