namespace CrashEdit
{

    public sealed class ImportAddVerb : GroupVerb
    {

        public override string Text => "Import from file...";

        public override string ImageKey => "FolderOpen";

        public override bool ApplicableForGroup(SubcontrollerGroup group)
        {
            if (group == null)
                throw new ArgumentNullException();

            if (group.CanAdd)
            {
                return Importer.AllImporters
                    .Where(x => group.ResourceType.IsAssignableFrom(x.ResourceType))
                    .Any();
            }
            else
            {
                return false;
            }
        }

        public override void Execute(IUserInterface ui)
        {
            if (ui == null)
                throw new ArgumentNullException();
            if (Group == null)
                throw new InvalidOperationException();

            var importers = Importer.AllImporters
                .Where(x => Group.ResourceType.IsAssignableFrom(x.ResourceType))
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

            // Add the new resource.
            Group.Add(res!);
        }

    }

}
