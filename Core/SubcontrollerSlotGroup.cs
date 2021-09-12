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
