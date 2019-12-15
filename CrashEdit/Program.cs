using System;
using System.Windows.Forms;

namespace CrashEdit
{
    internal static class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            using (OldMainForm mainform = new OldMainForm())
            using (ErrorReporter errorform = new ErrorReporter(mainform))
            {
                FileUtil.Owner = mainform;
                Application.EnableVisualStyles();
                Application.Run(mainform);
            }
        }
    }
}
