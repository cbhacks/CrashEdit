using Crash;
using Application = System.Windows.Forms.Application;

namespace CrashEdit
{
    internal static class Program
    {
        [System.STAThread]
        internal static void Main(string[] args)
        {
            Registrar.RegisterAssembly("Crash");
            Registrar.RegisterAssembly("Crash.Game");
            Registrar.RegisterAssembly("Crash.Graphics");
            Registrar.RegisterAssembly("Crash.Audio");
            Registrar.RegisterAssembly("Crash.Unknown0");
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
