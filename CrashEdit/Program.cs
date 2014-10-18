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
                Application.Run(mainform);
            }
        }
    }
}
