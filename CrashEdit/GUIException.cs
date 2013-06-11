using System;
using System.Runtime.Serialization;

namespace CrashEdit
{
    [Serializable]
    public class GUIException : Exception
    {
        public GUIException(string message) : base(message)
        {
        }

        public GUIException(string message,Exception inner) : base(message,inner)
        {
        }

        protected GUIException(SerializationInfo info,StreamingContext context) : base(info,context)
        {
        }
    }
}
