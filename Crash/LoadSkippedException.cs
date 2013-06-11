using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class LoadSkippedException : Exception
    {
        public LoadSkippedException() : base("Processing of this object was skipped.")
        {
        }

        public LoadSkippedException(string message) : base(message)
        {
        }

        public LoadSkippedException(string message,Exception inner) : base(message,inner)
        {
        }

        protected LoadSkippedException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }
    }
}
