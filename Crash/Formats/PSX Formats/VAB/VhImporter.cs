#nullable enable

using System;

namespace CrashEdit.Crash {

    public sealed class VhImporter : Importer<VH> {

        public override string[] FileFilters => new string[] {
            "PlayStation VAB headers (*.vh)|*.vh;*.VH"
        };

        public override bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out VH res) {
            if (ui == null)
                throw new ArgumentNullException();

            res = VH.Load(buf.ToArray());
            return true;
        }

    }

}
