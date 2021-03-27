#nullable enable

using System;

namespace CrashEdit {

    public interface ICommandHost : IUserInterface {

        IWorkspaceHost? ActiveWorkspaceHost { get; }

        public event EventHandler? ResyncSuggested;

    }

    public abstract class Command {

        public Command(ICommandHost host) {
            if (host == null)
                throw new ArgumentNullException();

            Host = host;
        }

        public ICommandHost Host { get; }

        public IWorkspaceHost? WsHost => Host.ActiveWorkspaceHost;

        public virtual string Text => GetType().Name;

        public virtual string ImageKey => "";

        public virtual bool Ready => true;

        public abstract bool Execute();

    }

}
