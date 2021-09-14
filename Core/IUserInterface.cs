#nullable enable

using System;

namespace CrashEdit {

    public interface IUserInterface {

        void ShowError(string msg);

        bool ShowImportDialog(out string? filename, string[] fileFilters);

        bool ShowExportDialog(out string? filename, string[] fileFilters);

    }

}
