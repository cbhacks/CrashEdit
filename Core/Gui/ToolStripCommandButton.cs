#nullable enable

using System;
using System.Windows.Forms;

namespace CrashEdit {

    public class ToolStripCommandButton : ToolStripButton {

        private Command? _command;

        public Command? Command {
            get { return _command; }
            set {
                if (_command == value)
                    return;

                if (_command != null) {
                    _command.Host.ResyncSuggested -= Command_Host_ResyncSuggested;
                }
                if (value != null) {
                    value.Host.ResyncSuggested += Command_Host_ResyncSuggested;
                }

                _command = value;
                Sync();
            }
        }

        public void Sync() {
            if (Command == null) {
                Enabled = false;
                Text = null;
                ImageKey = null;
                return;
            }

            Text = Command.Text;
            ImageKey = Command.ImageKey;

            Enabled = Command.Ready;
        }

        protected override void OnClick(EventArgs e) {
            if (Command != null && Command.Ready) {
                Command.Execute();
            }

            base.OnClick(e);
        }

        private void Command_Host_ResyncSuggested(object sender, EventArgs e) {
            Sync();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_command != null) {
                    _command.Host.ResyncSuggested -= Command_Host_ResyncSuggested;
                    _command = null;
                }
            }
            base.Dispose(disposing);
        }

    }

}
