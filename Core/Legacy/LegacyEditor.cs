#nullable enable

using System;
using System.Windows.Forms;

namespace CrashEdit {

    public sealed class LegacyEditor : Editor {

        public override string Text =>
            (Control as LegacyEditorControlWrapper)?.InnerControl?.GetType()?.Name ?? "Legacy";

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

}
