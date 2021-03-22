#nullable enable

using System;
using System.Linq;
using System.Collections.Generic;

namespace CrashEdit {

    public sealed class Controller {

        public Controller(object resource, SubcontrollerGroup? parentGroup) {
            if (resource == null)
                throw new ArgumentNullException();

            Resource = resource;
            ParentGroup = parentGroup;
        }

        public Controller(LegacyController legacy) : this(legacy.Resource, legacy.Parent?.Modern?.LegacyGroup) {
            if (legacy == null)
                throw new ArgumentNullException();

            Legacy = legacy;
            LegacyGroup = new LegacySubcontrollerGroup(this);
            SubcontrollerGroups.Add(LegacyGroup);
        }

        public object Resource { get; }

        public Controller? Parent => ParentGroup?.Owner;

        public SubcontrollerGroup? ParentGroup { get; }

        public List<SubcontrollerGroup> SubcontrollerGroups { get; } =
            new List<SubcontrollerGroup>();

        public string Text =>
            Legacy?.NodeText ??
            Resource.GetType().ToString();

        public string ImageKey =>
            Legacy?.NodeImage ??
            "";

        public LegacyController? Legacy { get; }

        public LegacySubcontrollerGroup? LegacyGroup { get; }

        public void Sync() {
            foreach (var subctlrGroup in SubcontrollerGroups) {
                subctlrGroup.Sync();
            }
        }

    }

    public abstract class SubcontrollerGroup {

        public SubcontrollerGroup(Controller owner) {
            if (owner == null)
                throw new ArgumentNullException();

            Owner = owner;
        }

        public Controller Owner { get; }

        public List<Controller> Members { get; } =
            new List<Controller>();

        public abstract void Sync();

    }

    public sealed class LegacySubcontrollerGroup : SubcontrollerGroup {

        public LegacySubcontrollerGroup(Controller owner) : base(owner) {
            if (owner.Legacy == null)
                throw new Exception();

            Sync();
        }

        public override void Sync() {
            Members.Clear();
            Members.AddRange(Owner.Legacy!.LegacySubcontrollers.Select(x => x.Modern));

            foreach (var subctlr in Members) {
                subctlr.Sync();
            }
        }

    }

}
