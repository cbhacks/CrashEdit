using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ConfigEditor : UserControl
    {
        public static Dictionary<string,string> Languages = new Dictionary<string,string>()
        {
            { "English","en-US" },
            { "Japanese","ja-JA" }
        };

        public ConfigEditor()
        {
            InitializeComponent();
            foreach (string lang in Languages.Keys)
                dpdLang.Items.Add(lang);
            dpdLang.SelectedItem = Properties.Settings.Default.Language;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Language = (string)dpdLang.SelectedItem;
            Properties.Settings.Default.Save();
            MessageBox.Show("Effects will apply on program restart.", "Configuration save successful", MessageBoxButtons.OK);
        }
    }
}
