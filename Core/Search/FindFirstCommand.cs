
using System;

namespace CrashEdit {

    public sealed class FindFirstCommand : Command {

        public FindFirstCommand(ICommandHost host) : base(host) {}

        public override string Text => "Find First";

        public override string ImageKey => "RecordFirst";

        public override bool Ready =>
            WsHost?.SearchPredicate != null;

        public override bool Execute() {
            if (WsHost == null)
                throw new InvalidOperationException();
            if (WsHost.SearchPredicate == null)
                throw new InvalidOperationException();

            // Start from the root controller.
            var w = new Walker();
            w.Cursor = WsHost.RootController;

            // Advance depth-first until a match is found.
            while(!WsHost.SearchPredicate(w.Cursor)) {
                if (!w.MoveToNextDFS()) {
                    // Nothing in the entire tree matches.
                    Host.ShowError("No results found.");
                    return false;
                }
            }

            // Match found.
            WsHost.ActiveController = w.Cursor;
            return true;
        }

    }

}
