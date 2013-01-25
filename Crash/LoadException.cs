using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class LoadException : Exception,ISerializable
    {
        public LoadException() : base("The data to be loaded was malformed.")
        {
        }

        public LoadException(string message) : base(message)
        {
        }

        public LoadException(string message,Exception inner) : base(message,inner)
        {
        }

        protected LoadException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }
    }
}
