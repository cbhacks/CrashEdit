using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class PackingException : Exception,ISerializable
    {
        public PackingException() : base("The data to be saved was too large to fit into its parent container.")
        {
        }

        public PackingException(string message) : base(message)
        {
        }

        public PackingException(string message,Exception inner) : base(message,inner)
        {
        }

        protected PackingException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }
    }
}
