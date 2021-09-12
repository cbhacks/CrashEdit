#nullable enable

using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class Verb : ICloneable {

        public abstract string Text { get; }

        public virtual string ImageKey => "";

        public abstract void Execute(IUserInterface ui);

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

}
