using System.Windows.Forms;

namespace CrashEdit
{

    public abstract class LegacyController
    {

        public LegacyController(SubcontrollerGroup? parentGroup, object resource)
        {
            ArgumentNullException.ThrowIfNull(resource);

            Resource = resource;
            Modern = new Controller(this, parentGroup);
        }

        public Controller Modern { get; }

        public object Resource { get; }

        public List<LegacyVerb> LegacyVerbs { get; } = [];

        public virtual bool EditorAvailable => false;

        public virtual Control CreateEditor()
        {
            throw new NotSupportedException();
        }

        public bool NeedsNewEditor { get; set; }

        public virtual bool CanMoveTo(LegacyController dest)
        {
            return false;
        }

        public virtual void MoveTo(LegacyController dest)
        {
            throw new NotSupportedException();
        }

        public static Dictionary<Type, Type> OrphanControllerTypes = [];

        [Registrar.TypeProcessor]
        private static void ProcessOrphanControllerType(Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(OrphanLegacyControllerAttribute), false);
            foreach (OrphanLegacyControllerAttribute attr in attrs)
            {
                OrphanControllerTypes.Add(attr.ResourceType, type);
            }
        }

    }

}
