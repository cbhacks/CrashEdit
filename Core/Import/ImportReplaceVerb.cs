namespace CrashEdit
{

    public sealed class ImportReplaceVerb : DirectVerb
    {

        public override string Text => "Replace from file...";

        public override string ImageKey => "Fire";

        public override bool ApplicableForSubject(Controller subj)
        {
            ArgumentNullException.ThrowIfNull(subj);

            if (subj.ParentGroup?.CanReplace == true)
            {
                return Importer.AllImporters
                    .Where(x => subj.ParentGroup.ResourceType.IsAssignableFrom(x.ResourceType))
                    .Any();
            }
            else
            {
                return false;
            }
        }

        public override void Execute(IUserInterface ui)
        {
            ArgumentNullException.ThrowIfNull(ui);
            if (Subject == null)
                throw new InvalidOperationException();
            if (Subject.ParentGroup == null)
                throw new InvalidOperationException();

            var importers = Importer.AllImporters
                .Where(x => Subject.ParentGroup.ResourceType.IsAssignableFrom(x.ResourceType))
                .ToList();
            if (importers.Count == 0)
                throw new InvalidOperationException();

            var importer = importers[0];
            // TODO - multiple choices?

            // Get the input filename from the user.
            if (!ui.ShowImportDialog(out var filename, importer.FileFilters))
            {
                return;
            }

            // Read the data from the file.
            var buf = File.ReadAllBytes(filename);

            // Create a resource from the raw data.
            if (!importer.Import(ui, buf, out var res))
            {
                return;
            }

            // Replace the resource with the new one.
            Subject.ParentGroup.Replace(Subject, res!);
        }

    }

}
