using Crash;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace CrashEdit
{
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        static extern bool FreeConsole();
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string path);

        public static SortedDictionary<string, string> C3AnimLinks = new(new ENameComparer());
        public static void SaveC3AnimLinks()
        {
            using XmlWriter writer = XmlWriter.Create("CrashEdit.exe.animmodel.config", new XmlWriterSettings() { Indent = true, IndentChars = "\t" });
            writer.WriteStartElement("animmodels");
            foreach (var kvp in C3AnimLinks)
            {
                writer.WriteStartElement("animmodel");
                writer.WriteAttributeString("anim", kvp.Key);
                writer.WriteAttributeString("model", kvp.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Flush();
        }

        public static void LoadC3AnimLinks()
        {
            C3AnimLinks.Clear();
            if (!File.Exists("CrashEdit.exe.animmodel.config")) return;
            XmlReader r = XmlReader.Create("CrashEdit.exe.animmodel.config");
            try
            {
                while (r.Read())
                {
                    switch (r.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (r.Name == "animmodel")
                            {
                                string anim = r.GetAttribute("anim");
                                string model = r.GetAttribute("model");
                                C3AnimLinks.Add(anim, model);
                            }
                            break;
                    }
                }
            }
            finally
            {
                r.Close();
                r.Dispose();
            }
        }

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

            if (Properties.Settings.Default.UpgradeSettings)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeSettings = false;
                Properties.Settings.Default.Save();
            }
            try
            {
                Properties.Resources.Culture = Crash.UI.Properties.Resources.Culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Language);
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
            LoadC3AnimLinks();

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
