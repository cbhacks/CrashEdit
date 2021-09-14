#nullable enable

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

            var exporter = exporters[0];
            // TODO - multiple choices?

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
