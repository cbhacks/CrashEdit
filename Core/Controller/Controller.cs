namespace CrashEdit
{

    public sealed class Controller
    {

        public static Controller Make(object resource, SubcontrollerGroup? parentGroup)
        {
            ArgumentNullException.ThrowIfNull(resource);

            var type = resource.GetType();
            while (type != null)
            {
                if (LegacyController.OrphanControllerTypes.TryGetValue(type, out var legacyCtlrType))
                {
                    var args = new object?[] { resource, parentGroup };
                    var legacyCtlr = (LegacyController)Activator.CreateInstance(legacyCtlrType, args);
                    return legacyCtlr.Modern;
                }
                type = type.BaseType;
            }

            return new Controller(resource, parentGroup);
        }

        private Controller(object resource, SubcontrollerGroup? parentGroup)
        {
            ArgumentNullException.ThrowIfNull(resource);

            Resource = resource;
            ParentGroup = parentGroup;

            var type = resource.GetType();
            foreach (var property in type.GetProperties())
            {
                var attr = property
                    .GetCustomAttributes(typeof(SubresourceAttribute), true)
                    .SingleOrDefault();

                if (attr == null)
                {
                    // Not a subresource.
                }
                else if (attr is SubresourceSlotAttribute slotAttr)
                {
                    // Single (possibly null) subresource.
                    SubcontrollerGroups.Add(new SubcontrollerSlotGroup(this, property, slotAttr));
                }
                else if (attr is SubresourceListAttribute listAttr)
                {
                    // List of zero or more subresources.
                    SubcontrollerGroups.Add(new SubcontrollerListGroup(this, property, listAttr));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            SubcontrollerGroups.Sort((x, y) => x.Order - y.Order);
        }

        public Controller(LegacyController legacy, SubcontrollerGroup? parentGroup) : this(legacy.Resource, parentGroup)
        {
            ArgumentNullException.ThrowIfNull(legacy);

            Legacy = legacy;
        }

        public object Resource { get; }

        public Controller? Parent => ParentGroup?.Owner;

        public Controller? Root => Parent?.Root ?? this;

        public SubcontrollerGroup? ParentGroup { get; }

        public List<SubcontrollerGroup> SubcontrollerGroups { get; } =
            new List<SubcontrollerGroup>();

        public string Text =>
            (Resource as IResource)?.Title ??
            ParentGroup?.MakeTextForMember(this) ??
            Resource.GetType().ToString();

        public string ImageKey =>
            (Resource as IResource)?.ImageKey ??
            "Arrow";

        public bool Dead { get; private set; }

        public void Kill()
        {
            if (Dead)
                throw new InvalidOperationException();

            Dead = true;
            foreach (var ctlrGroup in SubcontrollerGroups)
            {
                foreach (var ctlr in ctlrGroup.Members)
                {
                    ctlr.Kill();
                }
            }
        }

        public LegacyController? Legacy { get; }

        public void Sync()
        {
            if (Dead)
                throw new InvalidOperationException();

            if (Resource is Workspace ws)
            {
                ws.Sync();
            }
            foreach (var subctlrGroup in SubcontrollerGroups)
            {
                subctlrGroup.Sync();
            }
        }

    }

}
