
using System;

namespace CrashEdit.Crash {

    public sealed class SeqToMidiExporter : Exporter<SEQ> {

        public override string Text => "Standard MIDI";

        public override string[] FileFilters => new string[] {
            "Standard MIDI files (*.mid, *.midi)|*.mid;*.MID;*.midi;*.MIDI"
        };

        public override bool Export(IUserInterface ui, out ReadOnlySpan<byte> buf, SEQ res) {
            if (ui == null)
                throw new ArgumentNullException();
            if (res == null)
                throw new ArgumentNullException();

            buf = new ReadOnlySpan<byte>(res.ToMIDI());
            return true;
        }

    }

}
