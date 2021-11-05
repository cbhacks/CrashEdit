#nullable enable

using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class Exporter {

        public abstract Type ResourceType { get; }

        public abstract string Text { get; }

        public abstract bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, object res);

        public virtual string[] FileFilters => new string[0];

        public static List<Exporter> AllExporters { get; } =
            new List<Exporter>();

        [Registrar.TypeProcessor]
        private static void ProcessExporterType(Type type) {
            if (!typeof(Exporter).IsAssignableFrom(type))
                return;
            if (type.IsAbstract)
                return;

            AllExporters.Add((Exporter)Activator.CreateInstance(type));
        }

    }

    public abstract class Exporter<T> : Exporter {

        public sealed override Type ResourceType => typeof(T);

        public sealed override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, object res) {
            if (ui == null)
                throw new ArgumentNullException();
            if (res == null)
                throw new ArgumentNullException();
            if (!(res is T))
                throw new ArgumentException();

            return Export(ui, out buf, (T)res);
        }

        public abstract bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, T res);

    }

}
