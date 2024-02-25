#nullable enable

using System;

namespace CrashEdit {

    public interface ICommandHost : IUserInterface {

        IWorkspaceHost? ActiveWorkspaceHost { get; }

        public event EventHandler? ResyncSuggested;

    }
}
