#nullable enable

using System;
using System.Windows.Forms;

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

    public sealed class FindPreviousCommand : Command {

        public FindPreviousCommand(ICommandHost host) : base(host) {}

        public override string Text => "Find Previous";

        public override string ImageKey => "RecordPrevious";

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

            // Regress until a match is found. The initial selection is not eligible.
            while (w.MoveToPreviousDFS()) {
                if (WsHost.SearchPredicate(w.Cursor)) {
                    // Match found.
                    WsHost.ActiveController = w.Cursor;
                    return true;
                }
            }

            // No match.
            Host.ShowError("No results before the current selection.");
            return false;
        }

    }

}
