#nullable enable

using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class Verb : ICloneable {

        public abstract string Text { get; }

        public abstract void Execute();

        public virtual object Clone() {
            return MemberwiseClone();
        }

        public static List<Verb> AllVerbs { get; } =
            new List<Verb>();

        [TypeProcessor]
        private static void ProcessVerbType(Type type) {
            if (!typeof(Verb).IsAssignableFrom(type))
                return;
            if (type.IsAbstract)
                return;
            if (type == typeof(LegacyVerb))
                return;

            AllVerbs.Add((Verb)Activator.CreateInstance(type));
        }

    }

    public interface IVerbExecutor {

        void ExecuteVerb(Verb verb);

        void ExecuteVerbChoice(List<Verb> verbs);

    }

    public abstract class TransitiveVerb : Verb {

        public Controller? Source { get; set; }

        public Controller? Destination { get; set; }

        public abstract bool ApplicableForSource(Controller src);

        public abstract bool ApplicableForTransit(Controller src, Controller dest);

    }

}
