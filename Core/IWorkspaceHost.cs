#nullable enable

using System;

namespace CrashEdit {

    public interface IWorkspaceHost {

        Controller RootController { get; }

        Controller? ActiveController { get; set; }

        Predicate<Controller>? SearchPredicate { get; }

    }

}
