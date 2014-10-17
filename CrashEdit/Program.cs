using Crash;
using System;
using System.Windows.Forms;

namespace CrashEdit
{
    internal static class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            using (ErrorReporter errorform = new ErrorReporter())
            using (OldMainForm oldmainform = new OldMainForm())
            {
                foreach (string arg in args)
                {
                    oldmainform.OpenNSF(arg);
                }
                Application.Run(oldmainform);
            }
        }
    }
}
