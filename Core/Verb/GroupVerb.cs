
using System;

namespace CrashEdit {

    public abstract class GroupVerb : Verb {

        public SubcontrollerGroup? Group { get; set; }

        public abstract bool ApplicableForGroup(SubcontrollerGroup group);

    }

}
