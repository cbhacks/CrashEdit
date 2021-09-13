#nullable enable

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit {

    public abstract class LegacyController {

        public LegacyController(LegacyController? parent, object resource) {
            if (resource == null)
                throw new ArgumentNullException();

            Parent = parent;
            Resource = resource;
            Modern = new Controller(this);
        }

        public LegacyController(SubcontrollerGroup? parentGroup, object resource) {
            if (resource == null)
                throw new ArgumentNullException();

            Parent = null;
            Resource = resource;
            Modern = new Controller(this, parentGroup);
        }

        public Controller Modern { get; }

        public LegacyController? Parent { get; }

        public object Resource { get; }

        public List<LegacyController> LegacySubcontrollers { get; } =
            new List<LegacyController>();

        public List<LegacyVerb> LegacyVerbs { get; } =
            new List<LegacyVerb>();

        public string NodeText { get; set; } = "";

        public string? NodeImageKey { get; set; }

        public virtual bool EditorAvailable => false;

        public virtual Control CreateEditor() {
            throw new NotSupportedException();
        }

        public bool NeedsNewEditor { get; set; }

        public virtual bool CanMoveTo(LegacyController dest) {
            return false;
        }

        public virtual LegacyController MoveTo(LegacyController dest) {
            throw new NotSupportedException();
        }

        public static Dictionary<Type, Type> OrphanControllerTypes =
            new Dictionary<Type, Type>();

        [Registrar.TypeProcessor]
        private static void ProcessOrphanControllerType(Type type) {
            var attrs = type.GetCustomAttributes(typeof(OrphanLegacyControllerAttribute), false);
            foreach (OrphanLegacyControllerAttribute attr in attrs) {
                OrphanControllerTypes.Add(attr.ResourceType, type);
            }
        }

    }

}
