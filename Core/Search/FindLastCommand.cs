#nullable enable

using System;

namespace CrashEdit {

    public sealed class FindLastCommand : Command {

        public FindLastCommand(ICommandHost host) : base(host) {}

        public override string Text => "Find Last";

        public override string ImageKey => "RecordLast";

        public override bool Ready =>
            WsHost?.SearchPredicate != null;

        public override bool Execute() {
            if (WsHost == null)
                throw new InvalidOperationException();
            if (WsHost.SearchPredicate == null)
                throw new InvalidOperationException();

            // Start from the last (by depth-first) controller.
            var w = new Walker();
            w.Cursor = WsHost.RootController;
            while (w.MoveToLastChild()) {}

            // Regress depth-first until a match is found.
            while(!WsHost.SearchPredicate!(w.Cursor)) {
                if (!w.MoveToPreviousDFS()) {
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
