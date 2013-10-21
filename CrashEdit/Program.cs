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
            using (MainForm mainform = new MainForm())
            {
                foreach (string arg in args)
                {
                    mainform.OpenNSF(arg);
                }
                Application.Run(mainform);
            }
        }
    }
}
