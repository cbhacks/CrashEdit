#nullable enable

using System;
using System.Reflection;

namespace CrashEdit {

    public sealed class SubcontrollerSlotGroup : SubcontrollerGroup {

        public SubcontrollerSlotGroup(Controller owner, PropertyInfo property, SubresourceSlotAttribute attr) : base(owner) {
            if (property == null)
                throw new ArgumentNullException();

            Property = property;
            Attribute = attr;
            Sync();
        }

        public PropertyInfo Property { get; }

        public SubresourceSlotAttribute Attribute { get; }

        public override int Order => Attribute.Order;

        public override string Text => Property.Name;

        public override Type ResourceType => Property.PropertyType;

        public bool CanSet => Property.SetMethod?.IsPublic ?? false;

        public bool CanSetNull => CanSet && Attribute.AllowNull;

        public override bool CanAdd => CanSet && Property.GetValue(Owner.Resource) == null;

        public override void Add(object res) {
            if (res == null)
                throw new ArgumentNullException();
            if (!CanAdd)
                throw new InvalidOperationException();

            Property.SetValue(Owner.Resource, res);
        }

        public override bool CanRemove => CanSetNull && Property.GetValue(Owner.Resource) != null;

        public override void Remove(Controller subctlr) {
            if (subctlr == null)
                throw new ArgumentNullException();
            if (!CanRemove)
                throw new InvalidOperationException();
            if (Members.Count == 0 || Members[0] != subctlr)
                throw new Exception();

            Property.SetValue(Owner.Resource, null);
        }

        public override bool CanReplace => CanSet && Property.GetValue(Owner.Resource) != null;

        public override void Replace(Controller subctlr, object newRes) {
            if (subctlr == null)
                throw new ArgumentNullException();
            if (newRes == null)
                throw new ArgumentNullException();
            if (!CanReplace)
                throw new InvalidOperationException();
            if (Members.Count == 0 || Members[0] != subctlr)
                throw new Exception();

            Property.SetValue(Owner.Resource, newRes);
        }

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

        public override string MakeTextForMember(Controller ctlr) => Property.Name;

    }

}
