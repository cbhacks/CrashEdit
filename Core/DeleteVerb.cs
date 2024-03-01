namespace CrashEdit
{
    public class DeleteVerb : DirectVerb
    {
        public override string Text => "Delete";

        public override string ImageKey => "Erase";

        public override bool ApplicableForSubject(Controller subj)
        {
            ArgumentNullException.ThrowIfNull(subj);

            return subj.ParentGroup?.CanRemove ?? false;
        }

        public override void Execute(IUserInterface ui)
        {
            ArgumentNullException.ThrowIfNull(ui);
            if (Subject == null)
                throw new InvalidOperationException();
            if (Subject.ParentGroup == null)
                throw new InvalidOperationException();

            Subject.ParentGroup.Remove(Subject);
        }

    }

}
