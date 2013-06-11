using System.Diagnostics;

namespace Crash
{
    public static class ErrorManager
    {
        private static int skipdepth;

        static ErrorManager()
        {
            skipdepth = 0;
        }

        private static void SignalErrorWithArgs(ErrorSignalEventArgs e)
        {
            e.CanSkip = (skipdepth > 0);
            if (Signal != null)
            {
                Signal(null,e);
            }
            switch (e.Response)
            {
                case ErrorResponse.Break:
                    Debugger.Break();
                    e.Response = ErrorResponse.Abort;
                    SignalErrorWithArgs(e);
                    break;
                case ErrorResponse.Abort:
                    throw new LoadAbortedException();
                case ErrorResponse.Skip:
                    throw new LoadSkippedException();
                case ErrorResponse.Ignore:
                    break;
            }
        }

        public static event ErrorSignalEventHandler Signal;

        public static void SignalError(string message)
        {
            ErrorSignalEventArgs e = new ErrorSignalEventArgs(message);
            SignalErrorWithArgs(e);
        }

        public static void SignalIgnorableError(string message)
        {
            ErrorSignalEventArgs e = new ErrorSignalEventArgs(message);
            e.CanIgnore = true;
            SignalErrorWithArgs(e);
        }

        public static void EnterSkipRegion()
        {
            skipdepth++;
        }

        public static void ExitSkipRegion()
        {
            skipdepth--;
        }
    }
}
