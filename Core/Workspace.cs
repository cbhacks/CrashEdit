#nullable enable

using System;

namespace CrashEdit {

    public abstract class Workspace {
    }

    public interface IWorkspaceHost {

        Controller RootController { get; }

        Controller? ActiveController { get; set; }

        Predicate<Controller>? SearchPredicate { get; }

    }

}
