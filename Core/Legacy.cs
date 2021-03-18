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

}
