#nullable enable

using System;

namespace CrashEdit {

    public abstract class Workspace : IResource {

        public virtual string Title => "Workspace";

        public virtual string ImageKey => "Sitemap";

    }

    public interface IWorkspaceHost {

        Controller RootController { get; }

        Controller? ActiveController { get; set; }

        Predicate<Controller>? SearchPredicate { get; }

    }

}
