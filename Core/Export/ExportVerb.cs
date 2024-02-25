
using System;
using System.Linq;
using System.IO;

namespace CrashEdit {

    public sealed class ExportVerb : DirectVerb {

        public override string Text => "Export to file...";

        public override string ImageKey => "Floppy";

        public override bool ApplicableForSubject(Controller subj) {
            if (subj == null)
                throw new ArgumentNullException();

            var type = subj.Resource.GetType();
            return Exporter.AllExporters
                .Where(x => x.ResourceType.IsAssignableFrom(type))
                .Any();
        }

        public override void Execute(IUserInterface ui) {
            if (ui == null)
                throw new ArgumentNullException();
            if (Subject == null)
                throw new InvalidOperationException();

            var type = Subject.Resource.GetType();
            var exporters = Exporter.AllExporters
                .Where(x => x.ResourceType.IsAssignableFrom(type))
                .ToList();
            if (exporters.Count == 0)
                throw new InvalidOperationException();

            Exporter exporter;
            if (exporters.Count == 1) {
                exporter = exporters[0];
            } else {
                var choices = exporters.Select(
                    x => new UserChoice {
                        Text = x.Text,
                        ImageKey = "Floppy",
                        Tag = x,
                    });

                var choice = ui.ShowChoiceDialog("Choose an export format.", choices);
                if (choice == null) {
                    return;
                }
                exporter = (Exporter)choice.Tag!;
            }

            // Convert the resource to raw data for export.
            if (!exporter.Export(ui, out var buf, Subject.Resource)) {
                return;
            }

            // Get the output filename from the user.
            if (!ui.ShowExportDialog(out var filename, exporter.FileFilters)) {
                return;
            }

            // Write the data to the file.
            File.WriteAllBytes(filename, buf.ToArray());
        }

    }

}
