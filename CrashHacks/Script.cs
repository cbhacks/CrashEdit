using System;
using Crash;

namespace CrashHacks
{
    public abstract class Script
    {
        public abstract string Name
        {
            get;
        }

        public virtual string Description
        {
            get { return "No description available."; }
        }

        public virtual string Author
        {
            get { return "(unspecified)"; }
        }

        public virtual string Category
        {
            get { return "other"; }
        }

        public abstract SupportLevel CheckCompatibility(GameVersion gameversion);

        public abstract void Run(object value,GameVersion gameversion);
    }
}
