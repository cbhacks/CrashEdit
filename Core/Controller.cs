#nullable enable

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CrashEdit {

    public sealed class Controller {

        public static Controller Make(object resource, SubcontrollerGroup? parentGroup) {
            if (resource == null)
                throw new ArgumentNullException();
            if (parentGroup is LegacySubcontrollerGroup)
                throw new ArgumentException();

            var type = resource.GetType();
            while (type != null) {
                if (LegacyController.OrphanControllerTypes.TryGetValue(type, out var legacyCtlrType)) {
                    var args = new object?[] {resource, parentGroup};
                    var legacyCtlr = (LegacyController)Activator.CreateInstance(legacyCtlrType, args);
                    return legacyCtlr.Modern;
                }
                type = type.BaseType;
            }

            return new Controller(resource, parentGroup);
        }

        private Controller(object resource, SubcontrollerGroup? parentGroup) {
            if (resource == null)
                throw new ArgumentNullException();

            Resource = resource;
            ParentGroup = parentGroup;

            var type = resource.GetType();
            foreach (var property in type.GetProperties()) {
                var attr = property
                    .GetCustomAttributes(typeof(SubresourceAttribute), true)
                    .SingleOrDefault();

                if (attr == null) {
                    // Not a subresource.
                } else if (attr is SubresourceSlotAttribute slotAttr) {
                    // Single (possibly null) subresource.
                    SubcontrollerGroups.Add(new SlotSubcontrollerGroup(this, property, slotAttr));
                } else {
                    throw new NotImplementedException();
                }
            }

            SubcontrollerGroups.Sort((x, y) => x.Order - y.Order);
        }

        public Controller(LegacyController legacy) : this(legacy.Resource, legacy.Parent?.Modern?.LegacyGroup) {
            if (legacy == null)
                throw new ArgumentNullException();
            if (legacy.Parent == null)
                throw new ArgumentException();

            Legacy = legacy;
            LegacyGroup = new LegacySubcontrollerGroup(this);
            SubcontrollerGroups.Add(LegacyGroup);
        }

        public Controller(LegacyController legacy, SubcontrollerGroup? parentGroup) : this(legacy.Resource, parentGroup) {
            if (legacy == null)
                throw new ArgumentNullException();
            if (legacy.Parent != null)
                throw new ArgumentException();

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
            (Resource as IResource)?.Title ??
            Resource.GetType().ToString();

        public string ImageKey =>
            Legacy?.NodeImageKey ??
            (Resource as IResource)?.ImageKey ??
            "Arrow";

        public bool Dead { get; private set; }

        public void Kill() {
            if (Dead)
                throw new InvalidOperationException();

            Dead = true;
            foreach (var ctlrGroup in SubcontrollerGroups) {
                foreach (var ctlr in ctlrGroup.Members) {
                    ctlr.Kill();
                }
            }
        }

        public LegacyController? Legacy { get; }

        public LegacySubcontrollerGroup? LegacyGroup { get; }

        public void Sync() {
            if (Dead)
                throw new InvalidOperationException();

            foreach (var subctlrGroup in SubcontrollerGroups) {
                subctlrGroup.Sync();
            }
        }

    }

    public abstract class SubresourceAttribute : Attribute {}

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

    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SubresourceSlotAttribute : SubresourceAttribute {

        public SubresourceSlotAttribute([CallerLineNumber] int order = 0) {
            Order = order;
        }

        public int Order { get; }

    }

    public sealed class SlotSubcontrollerGroup : SubcontrollerGroup {

        public SlotSubcontrollerGroup(Controller owner, PropertyInfo property, SubresourceSlotAttribute attr) : base(owner) {
            if (property == null)
                throw new ArgumentNullException();

            Property = property;
            Attribute = attr;
            Sync();
        }

        public PropertyInfo Property { get; }

        public SubresourceSlotAttribute Attribute { get; }

        public override int Order => Attribute.Order;

        public override void Sync() {
            object value = Property.GetValue(Owner.Resource);

            if (value == null) {
                if (Members.Count != 0) {
                    Members[0].Kill();
                    Members.Clear();
                }
            } else if (Members.Count == 0) {
                Members.Add(Controller.Make(value, this));
            } else if (Members[0].Resource != value) {
                Members[0].Kill();
                Members[0] = Controller.Make(value, this);
            } else {
                Members[0].Sync();
            }
        }

    }

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

    }

}
