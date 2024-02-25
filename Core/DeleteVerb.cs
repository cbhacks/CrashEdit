#nullable enable

using System;

namespace CrashEdit {

    public sealed class DeleteVerb : DirectVerb {

        public override string Text => "Delete";

        public override string ImageKey => "Erase";

        public override bool ApplicableForSubject(Controller subj) {
            if (subj == null)
                throw new ArgumentNullException();

            return subj.ParentGroup?.CanRemove ?? false;
        }

        public override void Execute(IUserInterface ui) {
            if (ui == null)
                throw new ArgumentNullException();
            if (Subject == null)
                throw new InvalidOperationException();
            if (Subject.ParentGroup == null)
                throw new InvalidOperationException();

            Subject.ParentGroup.Remove(Subject);
        }

    }

}
