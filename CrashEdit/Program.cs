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
        public static void SaveC3AnimLinks()
        {
            using (XmlWriter writer = XmlWriter.Create("CrashEdit.exe.animmodel.config", new XmlWriterSettings() { Indent = true, IndentChars = "\t" }))
            {
                writer.WriteStartElement("animmodels");
                foreach (var kvp in C3AnimLinks)
                {
                    writer.WriteStartElement("animmodel");
                    writer.WriteAttributeString("anim", Entry.EIDToEName(kvp.Key));
                    writer.WriteAttributeString("model", Entry.EIDToEName(kvp.Value));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        public static void LoadC3AnimLinks()
        {
            XmlReader r = null;
            try
            {
                r = XmlReader.Create("CrashEdit.exe.animmodel.config");
            }
            catch (System.IO.FileNotFoundException) { }
            C3AnimLinks.Clear();
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
            r.Close();
            r.Dispose();
        }

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
