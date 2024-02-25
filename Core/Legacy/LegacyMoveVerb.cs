
using System;

namespace CrashEdit {

    public sealed class LegacyMoveVerb : TransitiveVerb {

        public override string Text => "Move here";

        public override bool ApplicableForSource(Controller src) {
            if (src == null)
                throw new ArgumentNullException();

            return (src.Legacy != null);
        }

        public override bool ApplicableForTransit(Controller src, Controller dest) {
            if (src == null)
                throw new ArgumentNullException();
            if (dest == null)
                throw new ArgumentNullException();

            if (src.Legacy == null)
                return false;
            if (dest.Legacy == null)
                return false;

            return src.Legacy.CanMoveTo(dest.Legacy);
        }

        public override void Execute(IUserInterface ui) {
            if (ui == null)
                throw new ArgumentNullException();
            if (Source == null)
                throw new InvalidOperationException();
            if (Destination == null)
                throw new InvalidOperationException();
            if (Source.Legacy == null)
                throw new InvalidOperationException();
            if (Destination.Legacy == null)
                throw new InvalidOperationException();

            Source.Legacy.MoveTo(Destination.Legacy);
        }

    }

}
