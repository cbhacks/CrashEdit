using System;
using System.Runtime.Serialization;

namespace Crash
{
    [Serializable]
    public class LoadAbortedException : Exception
    {
        public LoadAbortedException() : base("Loading was aborted while processing this object.")
        {
        }

        public LoadAbortedException(string message) : base(message)
        {
        }

        public LoadAbortedException(string message,Exception inner) : base(message,inner)
        {
        }

        protected LoadAbortedException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }
    }
}
