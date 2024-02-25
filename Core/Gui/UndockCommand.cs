namespace CrashEdit
{

    public sealed class UndockCommand : Command
    {

        public UndockCommand(MainForm host) : base(host) { }

        public override string Text => "Undock";

        public override string ImageKey => "Anchor";

        public override bool Ready =>
            (WsHost as MainControl)?.ResourceBox?.CanToggleUndock == true;

        public override bool Execute()
        {
            if (!(WsHost is MainControl mainCtl))
                throw new InvalidOperationException();
            if (!mainCtl.ResourceBox.CanToggleUndock)
                throw new InvalidOperationException();

            mainCtl.ResourceBox.ToggleUndock();
            return true;
        }

    }

}
