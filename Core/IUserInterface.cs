
using System;
using System.Collections.Generic;

namespace CrashEdit {

    public interface IUserInterface {

        void ShowError(string msg);

        bool ShowImportDialog(out string? filename, string[] fileFilters);

        bool ShowExportDialog(out string? filename, string[] fileFilters);

        UserChoice? ShowChoiceDialog(string msg, IEnumerable<UserChoice> choices);

    }

}
