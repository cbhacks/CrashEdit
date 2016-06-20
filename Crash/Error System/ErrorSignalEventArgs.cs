using System;

namespace Crash
{
    public class ErrorSignalEventArgs : EventArgs
    {
        private string message;
        private ErrorResponse response;
        private bool canskip;
        private bool canignore;

        public ErrorSignalEventArgs(string message)
        {
            this.message = message;
            response = ErrorResponse.Abort;
            canskip = false;
            canignore = false;
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public ErrorResponse Response
        {
            get { return response; }
            set { response = value; }
        }

        public bool CanSkip
        {
            get { return canskip; }
            set { canskip = value; }
        }

        public bool CanIgnore
        {
            get { return canignore; }
            set { canignore = value; }
        }
    }
}
