#nullable enable

using System;
using System.Collections.Generic;

namespace CrashEdit {

    public abstract class SubcontrollerGroup {

        public SubcontrollerGroup(Controller owner) {
            if (owner == null)
                throw new ArgumentNullException();

            Owner = owner;
        }

        public Controller Owner { get; }

        public List<Controller> Members { get; } =
            new List<Controller>();

        public abstract int Order { get; }

        public abstract void Sync();

        public abstract string MakeTextForMember(Controller ctlr);

    }

}
