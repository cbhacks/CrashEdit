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

        public abstract string Text { get; }

        public abstract Type ResourceType { get; }

        public virtual bool CanAdd => false;

        public virtual void Add(object res) => throw new NotImplementedException();

        public virtual bool CanRemove => false;

        public virtual void Remove(Controller subctlr) => throw new NotImplementedException();

        public virtual bool CanReplace => false;

        public virtual void Replace(Controller subctlr, object newRes) => throw new NotImplementedException();

        public abstract void Sync();

        public abstract string MakeTextForMember(Controller ctlr);

    }

}
