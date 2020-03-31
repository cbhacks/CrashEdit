using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace CrashEdit
{
    internal static class Program
    {
        public static Dictionary<int,int> C3AnimLinks = new Dictionary<int,int>();

        [STAThread]
        internal static void Main(string[] args)
        {
            try
            {
                Properties.Resources.Culture = Crash.UI.Properties.Resources.Culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Language);
            }
            catch {
                Properties.Settings.Default.Reset();
            }
            if (Properties.Settings.Default.DefaultFormW < 640)
                Properties.Settings.Default.DefaultFormW = 640;
            if (Properties.Settings.Default.DefaultFormH < 480)
                Properties.Settings.Default.DefaultFormH = 480;
            try
            {
                using (XmlReader r = XmlReader.Create("CrashEdit.exe.animmodel.config"))
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
                                    C3AnimLinks.Add(Entry.ENameToEID(anim), Entry.ENameToEID(model));
                                }
                                break;
                        }
                    }
                }
            }
            catch (System.IO.FileNotFoundException) { }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (OldMainForm mainform = new OldMainForm())
            using (ErrorReporter errorform = new ErrorReporter(mainform))
            {
                FileUtil.Owner = mainform;
                Application.Run(mainform);
            }
        }
    }
}
