
using System;

namespace CrashEdit.Crash {

    public sealed class SeqImporter : Importer<SEQ> {

        public override string[] FileFilters => new string[] {
            "PlayStation SEQ files (*.seq)|*.seq;*.SEQ"
        };

        public override bool Import(IUserInterface ui, ReadOnlySpan<byte> buf, out SEQ res) {
            if (ui == null)
                throw new ArgumentNullException();

            res = SEQ.Load(buf.ToArray());
            return true;
        }

    }

}
