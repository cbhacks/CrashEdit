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

        public string NodeImageKey { get; set; } = "";

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

        [TypeProcessor]
        private static void ProcessOrphanControllerType(Type type) {
            var attrs = type.GetCustomAttributes(typeof(OrphanLegacyControllerAttribute), false);
            foreach (OrphanLegacyControllerAttribute attr in attrs) {
                OrphanControllerTypes.Add(attr.ResourceType, type);
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OrphanLegacyControllerAttribute : Attribute {

        public OrphanLegacyControllerAttribute(Type resType) {
            if (resType == null)
                throw new ArgumentNullException();

            ResourceType = resType;
        }

        public Type ResourceType { get; }

    }

    public sealed class LegacyVerb : Verb {

        public LegacyVerb(string text, Action proc) {
            if (text == null)
                throw new ArgumentNullException();
            if (proc == null)
                throw new ArgumentNullException();

            _text = text;
            Proc = proc;
        }

        public string _text;

        public override string Text => _text;

        private Action Proc { get; }

        public override void Execute() {
            Proc();
        }

    }

    public sealed class LegacyMoveVerb : TransitiveVerb {

        public override string Text => "Move here";

        public override bool ApplicableForSource(Controller src) {
            if (src == null)
                throw new ArgumentNullException();

            return (src.Legacy != null);
        }

        public override bool ApplicableForTransit(Controller src, Controller dest) {
            if (src == null)
                throw new ArgumentNullException();
            if (dest == null)
                throw new ArgumentNullException();

            if (src.Legacy == null)
                return false;
            if (dest.Legacy == null)
                return false;

            return src.Legacy.CanMoveTo(dest.Legacy);
        }

        public override void Execute() {
            if (Source == null)
                throw new InvalidOperationException();
            if (Destination == null)
                throw new InvalidOperationException();
            if (Source.Legacy == null)
                throw new InvalidOperationException();
            if (Destination.Legacy == null)
                throw new InvalidOperationException();

            Source.Legacy.MoveTo(Destination.Legacy);
        }

    }

    public sealed class LegacyEditor : Editor {

        public override string Text => "Legacy";

        public override bool ApplicableForSubject(Controller subj) {
            if (subj == null)
                throw new ArgumentNullException();

            return (subj.Legacy?.EditorAvailable == true);
        }

        protected override Control MakeControl() {
            return new LegacyEditorControlWrapper(Subject.Legacy!);
        }

        public override void Sync() {
            ((LegacyEditorControlWrapper)Control).Sync();
        }

    }

    public sealed class LegacyEditorControlWrapper : UserControl {

        public LegacyEditorControlWrapper(LegacyController legacyCtlr) {
            if (legacyCtlr == null)
                throw new ArgumentNullException();

            LegacyController = legacyCtlr;
            legacyCtlr.NeedsNewEditor = false;
            InnerControl = legacyCtlr.CreateEditor();
            InnerControl.Dock = DockStyle.Fill;
            Controls.Add(InnerControl);
        }

        public LegacyController LegacyController { get; }

        public Control InnerControl { get; private set; }

        public void Sync() {
            if (LegacyController.NeedsNewEditor) {
                LegacyController.NeedsNewEditor = false;
                Controls.Remove(InnerControl);
                InnerControl.Dispose();
                InnerControl = LegacyController.CreateEditor();
                InnerControl.Dock = DockStyle.Fill;
                Controls.Add(InnerControl);
            }
        }

    }

}
