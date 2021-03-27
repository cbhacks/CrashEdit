#nullable enable

using System;

namespace CrashEdit.Crash {

    public sealed class ChunkExporter : Exporter<Chunk> {

        public override string[] FileFilters => new string[] {
            "nschunk files (*.nschunk)|*.nschunk;*.NSCHUNK"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, Chunk res) {
            if (ui == null)
                throw new ArgumentNullException();
            if (res == null)
                throw new ArgumentNullException();

            buf = new ReadOnlySpan<byte>(res.Save(1));
            return true;
        }

    }

}
