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
            using (OldMainForm mainform = new OldMainForm())
            {
                Application.Run(mainform);
            }
        }
    }
}
