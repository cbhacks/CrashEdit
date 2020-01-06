using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ConfigEditor : UserControl
    {
        public static readonly List<string> Languages = new List<string> { "en-US", "ja-JP" };

        public ConfigEditor()
        {
            InitializeComponent();
            foreach (string lang in Languages)
                dpdLang.Items.Add(Crash.UI.Properties.Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(lang)));
            dpdLang.SelectedItem = Crash.UI.Properties.Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            dpdLang.SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = Languages[dpdLang.SelectedIndex];
            Properties.Settings.Default.Save();
        }
    }
}
