
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CrashEdit {

    public sealed class SubcontrollerListGroup : SubcontrollerGroup {

        public SubcontrollerListGroup(Controller owner, PropertyInfo property, SubresourceListAttribute attr) : base(owner) {
            if (property == null)
                throw new ArgumentNullException();

            var iEnumerableTypes = property.PropertyType.GetInterfaces()
                .Concat(new Type[] {property.PropertyType})
                .Where(x => x.IsGenericType)
                .Where(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .ToList();
            if (iEnumerableTypes.Count != 1)
                throw new ArgumentException();

            Property = property;
            Attribute = attr;
            IEnumerableType = iEnumerableTypes[0];
            ResourceType = IEnumerableType.GenericTypeArguments[0];

            ICollectionType = typeof(ICollection<>).MakeGenericType(new Type[] {ResourceType});
            if (ICollectionType.IsAssignableFrom(property.PropertyType)) {
                ICollectionProperty_Count = ICollectionType.GetProperty("Count");
                ICollectionProperty_IsReadOnly = ICollectionType.GetProperty("IsReadOnly");
                ICollectionMethod_Add = ICollectionType.GetMethod("Add");
                ICollectionMethod_Clear = ICollectionType.GetMethod("Clear");
                ICollectionMethod_Contains = ICollectionType.GetMethod("Contains");
                ICollectionMethod_Remove = ICollectionType.GetMethod("Remove");
            } else {
                ICollectionType = null;
            }

            IListType = typeof(IList<>).MakeGenericType(new Type[] {ResourceType});
            if (IListType.IsAssignableFrom(property.PropertyType)) {
                IListProperty_Item = IListType.GetProperty("Item");
                IListMethod_IndexOf = IListType.GetMethod("IndexOf");
                IListMethod_Insert = IListType.GetMethod("Insert");
                IListMethod_RemoveAt = IListType.GetMethod("RemoveAt");
            } else {
                IListType = null;
            }

            Sync();
        }

        public PropertyInfo Property { get; }

        public SubresourceListAttribute Attribute { get; }

        // T, where the list property's type implements IEnumerable<T>.
        public override Type ResourceType { get; }

        // Reflection info for IEnumerable<ElementType>.
        public Type IEnumerableType { get; }

        // Reflection info for ICollection<ElementType>.
        public bool IsCollection => ICollectionType != null;
        public Type? ICollectionType { get; }
        public PropertyInfo? ICollectionProperty_Count { get; }
        public PropertyInfo? ICollectionProperty_IsReadOnly { get; }
        public MethodInfo? ICollectionMethod_Add { get; }
        public MethodInfo? ICollectionMethod_Clear { get; }
        public MethodInfo? ICollectionMethod_Contains { get; }
        public MethodInfo? ICollectionMethod_Remove { get; }

        // Reflection info for IList<ElementType>.
        public bool IsList => IListType != null;
        public Type? IListType { get; }
        public PropertyInfo? IListProperty_Item { get; }
        public MethodInfo? IListMethod_IndexOf { get; }
        public MethodInfo? IListMethod_Insert { get; }
        public MethodInfo? IListMethod_RemoveAt { get; }

        public override int Order => Attribute.Order;

        public override string Text => Property.Name;

        public bool CanModifyElements {
            get {
                if (!IsCollection)
                    return false;

                object? value = Property.GetValue(Owner.Resource);
                if (value == null)
                    return false;

                return !(bool)ICollectionProperty_IsReadOnly!.GetValue(value);
            }
        }

        public bool CanModifyStructure => CanModifyElements && !Property.PropertyType.IsArray;

        public override bool CanAdd => CanModifyStructure;

        public override void Add(object res) {
            if (res == null)
                throw new ArgumentNullException();
            if (!CanAdd)
                throw new InvalidOperationException();

            ICollectionMethod_Add!.Invoke(Property.GetValue(Owner.Resource), new object[] {res});
        }

        public override bool CanRemove => CanModifyStructure;

        public override void Remove(Controller subctlr) {
            if (subctlr == null)
                throw new ArgumentNullException();
            if (!CanRemove)
                throw new InvalidOperationException();
            int index = Members.IndexOf(subctlr);
            if (index == -1)
                throw new Exception();

            if (IsList) {
                // If implementing IList<T>, remove by index.
                IListMethod_RemoveAt!.Invoke(
                    Property.GetValue(Owner.Resource),
                    new object[] { index });
            } else {
                // Otherwise, remove by object identity.
                ICollectionMethod_Remove!.Invoke(
                    Property.GetValue(Owner.Resource),
                    new object[] { subctlr.Resource });
            }
        }

        public override bool CanReplace => CanModifyElements;

        public override void Replace(Controller subctlr, object newRes) {
            if (subctlr == null)
                throw new ArgumentNullException();
            if (newRes == null)
                throw new ArgumentNullException();
            if (!CanReplace)
                throw new InvalidOperationException();
            int index = Members.IndexOf(subctlr);
            if (index == -1)
                throw new Exception();

            IListProperty_Item!.SetValue(
                Property.GetValue(Owner.Resource),
                newRes,
                new object[] { index });
        }

        public override void Sync() {
            var value = (IEnumerable?)Property.GetValue(Owner.Resource);

            // If the property itself is null, just give up.
            if (value == null) {
                foreach (var subctlr in Members) {
                    subctlr.Kill();
                }
                Members.Clear();
                return;
            }

            var oldMembers = new Dictionary<object, Controller>();
            foreach (var subctlr in Members) {
                oldMembers.Add(subctlr.Resource, subctlr);
            }

            // Build the new member list.
            Members.Clear();
            foreach (object res in value) {
                if (res == null) {
                    // FIXME - phony null resource maybe?
                    throw new Exception();
                } else if (oldMembers.TryGetValue(res, out var subctlr)) {
                    Members.Add(subctlr);
                    oldMembers.Remove(res);
                    subctlr.Sync();
                } else {
                    Members.Add(Controller.Make(res, this));
                }
            }

            // Clean up the old controllers for members which are no longer present.
            foreach (var subctlr in oldMembers.Values) {
                subctlr.Kill();
            }
        }

        public override string MakeTextForMember(Controller ctlr) =>
            $"{Property.Name}[{Members.IndexOf(ctlr)}]";

    }

}
