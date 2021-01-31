using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace CrashEdit
{
    internal static class Program
    {
        public static SortedDictionary<string,string> C3AnimLinks = new SortedDictionary<string,string>(new ENameComparer());
        public static void SaveC3AnimLinks()
        {
            using (XmlWriter writer = XmlWriter.Create("CrashEdit.exe.animmodel.config", new XmlWriterSettings() { Indent = true, IndentChars = "\t" }))
            {
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
        }

        public static void LoadC3AnimLinks()
        {
            C3AnimLinks.Clear();
            if (!System.IO.File.Exists("CrashEdit.exe.animmodel.config")) return;
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

        [STAThread]
        internal static void Main(string[] args)
        {
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
            catch {
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
                Application.Run(mainform);
            }
        }
    }
}
