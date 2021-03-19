#nullable enable

using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class LegacyController {

        public LegacyController(LegacyController? parent, object resource) {
            if (resource == null)
                throw new ArgumentNullException();

            Parent = parent;
            Resource = resource;
            Modern = new Controller(this);
        }

        public Controller Modern { get; }

        public LegacyController? Parent { get; }

        public object Resource { get; }

        public List<LegacyController> LegacySubcontrollers { get; } =
            new List<LegacyController>();

        public List<LegacyVerb> LegacyVerbs { get; } =
            new List<LegacyVerb>();

        public abstract string NodeText { get; }

        public abstract string NodeImage { get; }

        public virtual bool CanMoveTo(LegacyController dest) {
            return false;
        }

        public virtual LegacyController MoveTo(LegacyController dest) {
            throw new NotSupportedException();
        }

    }

    public sealed class LegacyVerb : Verb {

        public LegacyVerb(string text, Action proc) {
            if (text == null)
                throw new ArgumentNullException();
            if (proc == null)
                throw new ArgumentNullException();

            Text = text;
            Proc = proc;
        }

        public override string Text { get; }

        private Action Proc { get; }

        public override void Execute() {
            Proc();
        }

    }

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

        public override void Execute() {
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
