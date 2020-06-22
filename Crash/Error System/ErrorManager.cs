using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Crash
{
    public static class ErrorManager
    {
        private static int skipdepth;
        private static WeakReference ignoredSubjectRef; // target of current Ignore All

        private static Stack<object> subjects = new Stack<object>();

        static ErrorManager()
        {
            skipdepth = 0;
            ignoredSubjectRef = new WeakReference(null);
        }

        private static void SignalErrorWithArgs(ErrorSignalEventArgs e)
        {
            if (subjects.Count > 0)
            {
                e.Subject = subjects.Peek();
                if (e.CanIgnore && e.Subject == ignoredSubjectRef.Target)
                {
                    return;
                }
            }

            e.CanSkip = skipdepth > 0;
            Signal?.Invoke(null, e);
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
                case ErrorResponse.IgnoreAll:
                    ignoredSubjectRef = new WeakReference(e.Subject);
                    break;
            }
        }

        public static event ErrorSignalEventHandler Signal;

        public static void SignalError(string message)
        {
            ErrorSignalEventArgs e = new ErrorSignalEventArgs(message);
            SignalErrorWithArgs(e);
            throw new InvalidOperationException();
        }

        public static void SignalIgnorableError(string message)
        {
            ErrorSignalEventArgs e = new ErrorSignalEventArgs(message);
            e.CanIgnore = true;
            SignalErrorWithArgs(e);
        }

        public static void EnterSkipRegion()
        {
            ++skipdepth;
        }

        public static void ExitSkipRegion()
        {
            --skipdepth;
        }

        public static void EnterSubject(object obj)
        {
            subjects.Push(obj);
        }

        public static void ExitSubject()
        {
            subjects.Pop();
        }
    }
}
