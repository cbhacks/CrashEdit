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
                dpdLang.Items.Add(Properties.Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(lang)));
            dpdLang.SelectedItem = Properties.Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(Properties.Settings.Default.Language));
            dpdLang.SelectedIndexChanged += new EventHandler(dpdLang_SelectedIndexChanged);
            numW.Value = Properties.Settings.Default.DefaultFormW;
            numH.Value = Properties.Settings.Default.DefaultFormH;
        }

        private void dpdLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = Languages[dpdLang.SelectedIndex];
            Properties.Settings.Default.Save();
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            ((OldMainForm)TopLevelControl).ResetConfig();
        }

        private void numW_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultFormW = (int)numW.Value;
            Properties.Settings.Default.Save();
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DefaultFormH = (int)numH.Value;
            Properties.Settings.Default.Save();
        }
    }
}
