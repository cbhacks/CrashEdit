#nullable enable

using System;

namespace CrashEdit {

    public sealed class ByteArrayExporter : Exporter<byte[]> {

        public override string Text => "Raw data";

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, byte[] res) {
            if (ui == null)
                throw new ArgumentNullException();
            if (res == null)
                throw new ArgumentNullException();

            buf = new ReadOnlySpan<byte>(res);
            return true;
        }

    }

}
