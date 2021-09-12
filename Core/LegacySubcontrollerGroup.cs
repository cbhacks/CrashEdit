#nullable enable

using System;
using System.Linq;
using System.Collections.Generic;

namespace CrashEdit {

    public sealed class LegacySubcontrollerGroup : SubcontrollerGroup {

        public LegacySubcontrollerGroup(Controller owner) : base(owner) {
            if (owner.Legacy == null)
                throw new Exception();

            Sync();
        }

        public override int Order => int.MaxValue;

        public override void Sync() {
            var missingMembers = new HashSet<Controller>(Members);
            Members.Clear();
            Members.AddRange(Owner.Legacy!.LegacySubcontrollers.Select(x => x.Modern));

            foreach (var subctlr in Members) {
                subctlr.Sync();
                missingMembers.Remove(subctlr);
            }

            foreach (var subctlr in missingMembers) {
                subctlr.Kill();
            }
        }

        public override string MakeTextForMember(Controller ctlr) => "?";

    }

}
