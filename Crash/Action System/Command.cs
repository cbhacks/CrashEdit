using System;

namespace Crash
{
    public abstract class Command
    {
        private bool hasrun;

        public Command()
        {
            this.hasrun = false;
        }

        public Command Run()
        {
            if (hasrun)
                throw new InvalidOperationException();
            hasrun = true;
            return RunImpl();
        }

        protected abstract Command RunImpl();
    }
}
