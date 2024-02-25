
using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class Importer {
        // FIXME: use [NotNullWhen(true)] in new .NET

        public abstract Type ResourceType { get; }

        public abstract bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out object? res);

        public virtual string[] FileFilters => new string[0];

        public static List<Importer> AllImporters { get; } =
            new List<Importer>();

        [Registrar.TypeProcessor]
        private static void ProcessImporterType(Type type) {
            if (!typeof(Importer).IsAssignableFrom(type))
                return;
            if (type.IsAbstract)
                return;

            AllImporters.Add((Importer)Activator.CreateInstance(type));
        }

    }

    public abstract class Importer<T> : Importer {
        // FIXME: use [NotNullWhen(true)] in new .NET

        public sealed override Type ResourceType => typeof(T);

        public sealed override bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out object? res) {
            if (ui == null)
                throw new ArgumentNullException();

            if (Import(ui, buf, out T t)) {
                res = t;
                return true;
            } else {
                res = null;
                return false;
            }
        }

        public abstract bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out T res);

    }

}
