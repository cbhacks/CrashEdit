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

        public abstract string NodeText { get; }

        public abstract string NodeImage { get; }

    }

}
