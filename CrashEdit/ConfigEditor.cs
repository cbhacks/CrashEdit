using CrashEdit.Properties;
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
                dpdLang.Items.Add(Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(lang)));
            dpdLang.SelectedItem = Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(Settings.Default.Language));
            dpdLang.SelectedIndexChanged += new EventHandler(dpdLang_SelectedIndexChanged);
            numW.Value = Settings.Default.DefaultFormW;
            numH.Value = Settings.Default.DefaultFormH;
            fraLang.Text = Resources.Conifg_FraLang;
        }

        private void dpdLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.Language = Languages[dpdLang.SelectedIndex];
            Settings.Default.Save();
        }

        private void cmdReset_Click(object sender, EventArgs e)
        {
            Settings.Default.Reset();
            ((OldMainForm)TopLevelControl).ResetConfig();
        }

        private void numW_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.DefaultFormW = (int)numW.Value;
            Settings.Default.Save();
        }

        private void numH_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.DefaultFormH = (int)numH.Value;
            Settings.Default.Save();
        }
    }
}
