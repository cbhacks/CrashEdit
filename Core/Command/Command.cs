namespace CrashEdit
{

    public abstract class Command
    {

        public Command(ICommandHost host)
        {
            ArgumentNullException.ThrowIfNull(host);

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
