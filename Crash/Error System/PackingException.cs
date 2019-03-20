using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class PackingException : Exception
    {
        int eid;
        int size;

        public PackingException(int eid,int size) : base("The data to be saved was too large to fit into its parent container.")
        {
            this.eid = eid;
            this.size = size;
        }

        public PackingException(int eid,int size,string message) : base(message)
        {
            this.eid = eid;
        }

        public PackingException(int eid,int size,string message,Exception inner) : base(message,inner)
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

        public int Size
        {
            get { return size; }
        }
    }
}
