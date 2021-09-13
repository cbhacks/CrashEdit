#nullable enable

using System;

namespace CrashEdit {

    public sealed class FindNextCommand : Command {

        public FindNextCommand(ICommandHost host) : base(host) {}

        public override string Text => "Find Next";

        public override string ImageKey => "RecordNext";

        public override bool Ready =>
            WsHost?.SearchPredicate != null &&
            WsHost?.ActiveController != null;

        public override bool Execute() {
            if (WsHost == null)
                throw new InvalidOperationException();
            if (WsHost.SearchPredicate == null)
                throw new InvalidOperationException();
            if (WsHost.ActiveController == null)
                throw new InvalidOperationException();

            // Start from the currently selected controller.
            var w = new Walker();
            w.Cursor = WsHost.ActiveController;

            // Advance until a match is found. The initial selection is not eligible.
            while (w.MoveToNextDFS()) {
                if (WsHost.SearchPredicate(w.Cursor)) {
                    // Match found.
                    WsHost.ActiveController = w.Cursor;
                    return true;
                }
            }

            // No match.
            Host.ShowError("No results after the current selection.");
            return false;
        }

    }

}
