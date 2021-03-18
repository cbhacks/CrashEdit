#nullable enable

using System;

namespace CrashEdit {

    public abstract class Verb : ICloneable {

        public abstract string Text { get; }

        public abstract void Execute();

        public virtual object Clone() {
            return MemberwiseClone();
        }

    }

    public interface IVerbExecutor {

        void ExecuteVerb(Verb verb);

    }

}
