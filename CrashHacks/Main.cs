using System;
using System.Windows.Forms;

namespace CrashHacks
{
    internal static class MainClass
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            using (MainForm mainform = new MainForm())
            {
                if (args.Length >= 2)
                {
                    mainform.TargetBIN = args[0];
                    mainform.SourceDir = args[1];
                }
                Application.Run(mainform);
            }
        }
    }
}
