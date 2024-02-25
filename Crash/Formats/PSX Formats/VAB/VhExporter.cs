
using System;

namespace CrashEdit.Crash {

    public sealed class VhExporter : Exporter<VH> {

        public override string Text => "PlayStation VH";

        public override string[] FileFilters => new string[] {
            "PlayStation VAB headers (*.vh)|*.vh;*.VH"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, VH res) {
            if (ui == null)
                throw new ArgumentNullException();
            if (res == null)
                throw new ArgumentNullException();

            buf = new ReadOnlySpan<byte>(res.Save());
            return true;
        }

    }

}
