using CrashEdit.Crash;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CrashEdit.CE
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string path);

        public static GLViewerLoader TopLevelGLViewer { get; set; } = null;

        [STAThread]
        internal static void Main(string[] args)
        {

            AllocConsole();

            PlatformID pid = Environment.OSVersion.Platform;
#if __MonoCS__
            if (pid != PlatformID.Unix && pid != PlatformID.MacOSX)
#else
            if (pid != PlatformID.Unix)
#endif
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                path = Path.Combine(path, IntPtr.Size == 8 ? "Win64" : "Win32");

                if (!SetDllDirectory(path))
                    throw new System.ComponentModel.Win32Exception();
            }

            Registrar.Init();
            Registrar.RegisterAssembly(typeof(Program).Assembly);

            if (Properties.Settings.Default.UpgradeSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeSettings = false;
                Properties.Settings.Default.Save();
            }
            try
            {
                Properties.Resources.Culture = CrashUI.Properties.Resources.Culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Language);
            }
            catch
            {
                Properties.Settings.Default.Language = "en";
            }
            if (Properties.Settings.Default.DefaultFormW < 640)
                Properties.Settings.Default.DefaultFormW = 640;
            if (Properties.Settings.Default.DefaultFormH < 480)
                Properties.Settings.Default.DefaultFormH = 480;
            Properties.Settings.Default.Save();
            GOOLInstruction.ParensOnPool = Properties.Settings.Default.GOOLParensOnPool;
            EntityVisual.LoadMaps();
            EntityVisual.SaveMaps();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (OldMainForm mainform = new OldMainForm())
            using (ErrorReporter errorform = new ErrorReporter(mainform))
            {
                FileUtil.Owner = mainform;
                TopLevelGLViewer = new GLViewerLoader();
                mainform.Controls.Add(TopLevelGLViewer);
                Application.Run(mainform);
            }

            FreeConsole();
        }
    }
}
