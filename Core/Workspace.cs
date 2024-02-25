#nullable enable

using System;

namespace CrashEdit {

    public abstract class Workspace : IResource {

        public virtual string Title => "Workspace";

        public virtual string ImageKey => "Sitemap";

        public virtual void Sync() {}

    }

}
