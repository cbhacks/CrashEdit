using CrashEdit.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class ConfigEditor : UserControl
    {
        public static readonly List<string> Languages = new() { "en", "ja" };

        private static readonly List<string> FontFileNames = new();
        private static readonly List<string> FontExtensions = new() { ".ttf", ".otf" };

        private void MakeFontsList()
        {
            var add_font = (string f) =>
            {
                if (FontExtensions.Contains(Path.GetExtension(f).ToLower()))
                {
                    var shortname = Path.GetFileName(f);
                    if (!dpdFont.Items.Contains(shortname))
                    {
                        dpdFont.Items.Add(shortname);
                        FontFileNames.Add(f);
                    }
                }
            };

            dpdFont.Items.Clear();
            FontFileNames.Clear();

            foreach (var f in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)))
            {
                add_font(f);
            }
            try
            {
                foreach (var f in Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Fonts")))
                {
                    add_font(f);
                }
            }
            catch (Exception ex) when (
                ex is DirectoryNotFoundException
                )
            {
            }
        }

        public ConfigEditor()
        {
            InitializeComponent();
            // note: if language data is not found, this will just grab the english name. TODO fix this
            foreach (string lang in Languages)
            {
                string name = Resources.ResourceManager.GetString("Language", new System.Globalization.CultureInfo(lang)) ?? "N/A";
                dpdLang.Items.Add($"{name} ({lang})");
                if (lang == Settings.Default.Language)
                    dpdLang.SelectedIndex = dpdLang.Items.Count - 1;
            }
            dpdLang.SelectedIndexChanged += new EventHandler(dpdLang_SelectedIndexChanged);
            MakeFontsList();
            dpdFont.SelectedIndexChanged += new EventHandler(dpdFont_SelectedIndexChanged);
            if (dpdFont.Items.Contains(Settings.Default.FontName))
                dpdFont.SelectedItem = Settings.Default.FontName;
            else if (FontFileNames.Contains(Settings.Default.FontName))
                dpdFont.SelectedIndex = FontFileNames.IndexOf(Settings.Default.FontName);
            else
                dpdFont.SelectedIndex = 0;
            numFontSize.Value = (decimal)Settings.Default.FontSize;
            numW.Value = Settings.Default.DefaultFormW;
            numH.Value = Settings.Default.DefaultFormH;
            numAnimGrid.Value = Settings.Default.AnimGridLen;
            chkNormalDisplay.Checked = Settings.Default.DisplayNormals;
            chkCollisionDisplay.Checked = Settings.Default.DisplayFrameCollision;
            chkUseAnimLinks.Checked = Settings.Default.UseAnimLinks;
            chkDeleteInvalidEntries.Checked = Settings.Default.DeleteInvalidEntries;
            chkAnimGrid.Checked = Settings.Default.DisplayAnimGrid;
            chkFont3DEnable.Checked = Settings.Default.Font3DEnable;
            chkFont3DAutoscale.Checked = Settings.Default.Font3DAutoscale;
            chkFont2DEnable.Checked = Settings.Default.Font2DEnable;
            chkViewerShowHelp.Checked = Settings.Default.ViewerShowHelp;
            cdlClearCol.Color = picClearCol.BackColor = Color.FromArgb(Settings.Default.ClearColorRGB);
            sldNodeShadeAmt.Value = (int)(Settings.Default.NodeShadeMax * 100);

            dpdLang.MaximumSize = new Size(lblLang.Width, 0);

            fraMisc.Text = Resources.Config_fraMisc;
            fraSize.Text = Resources.Config_fraSize;
            fraClearCol.Text = Resources.Config_fraClearCol;
            fraAnimGrid.Text = Resources.Config_fraAnimGrid;
            fraFont.Text = Resources.Config_fraFont;
            fraNodeShadeAmt.Text = Resources.Config_fraNodeShadeAmt;
            lblLang.Text = Resources.Config_lblLang;
            lblW.Text = Resources.Config_lblW;
            lblH.Text = Resources.Config_lblH;
            lblAnimGrid.Text = Resources.Config_lblAnimGrid;
            lblFontName.Text = Resources.Config_lblFontName;
            lblFontSize.Text = Resources.Config_lblFontSize;
            chkAnimGrid.Text = Resources.Config_chkAnimGrid;
            chkNormalDisplay.Text = Resources.Config_chkNormalDisplay;
            chkCollisionDisplay.Text = Resources.Config_chkCollisionDisplay;
            chkDeleteInvalidEntries.Text = Resources.Config_chkDeleteInvalidEntries;
            chkUseAnimLinks.Text = Resources.Config_chkUseAnimLinks;
            chkPatchNSDSavesNSF.Text = Resources.Config_chkPatchNSDSavesNSF;
            chkFont3DAutoscale.Text = Resources.Config_chkFont3DAutoscale;
            chkFont3DEnable.Text = Resources.Config_chkFont3DEnable;
            chkFont2DEnable.Text = Resources.Config_chkFont2DEnable;
            chkViewerShowHelp.Text = Resources.Config_chkViewerShowHelp;
            cmdReset.Text = Resources.Config_cmdReset;
        }

        private void dpdLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.Language = Languages[dpdLang.SelectedIndex];
            Settings.Default.Save();
        }

        private void dpdFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Default.FontName = FontFileNames[dpdFont.SelectedIndex];
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

        private void chkNormalDisplay_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayNormals = chkNormalDisplay.Checked;
            Settings.Default.Save();
        }

        private void chkCollisionDisplay_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayFrameCollision = chkCollisionDisplay.Checked;
            Settings.Default.Save();
        }

        private void chkUseAnimLinks_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.UseAnimLinks = chkUseAnimLinks.Checked;
            Settings.Default.Save();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (cdlClearCol.ShowDialog(this) == DialogResult.OK)
            {
                Settings.Default.ClearColorRGB = cdlClearCol.Color.ToArgb();
                picClearCol.BackColor = System.Drawing.Color.FromArgb(Settings.Default.ClearColorRGB);
                Settings.Default.Save();
            }
        }

        private void chkDeleteInvalidEntries_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DeleteInvalidEntries = chkDeleteInvalidEntries.Checked;
            Settings.Default.Save();
        }

        private void chkAnimGrid_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.DisplayAnimGrid = chkAnimGrid.Checked;
            Settings.Default.Save();
        }

        private void numAnimGrid_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.AnimGridLen = (int)numAnimGrid.Value;
            Settings.Default.Save();
        }

        private void chkPatchNSDSavesNSF_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.PatchNSDSavesNSF = chkPatchNSDSavesNSF.Checked;
            Settings.Default.Save();
        }

        private void numFontSize_ValueChanged(object sender, EventArgs e)
        {
            Settings.Default.FontSize = (float)numFontSize.Value;
            Settings.Default.Save();
        }

        private void chkFont3DEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Font3DEnable = chkFont3DEnable.Checked;
            Settings.Default.Save();
        }

        private void chkFont3DAutoscale_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Font3DAutoscale = chkFont3DAutoscale.Checked;
            Settings.Default.Save();
        }

        private void chkFont2DEnable_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.Font2DEnable = chkFont2DEnable.Checked;
            Settings.Default.Save();
        }

        private void chkViewerShowHelp_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.ViewerShowHelp = chkViewerShowHelp.Checked;
            Settings.Default.Save();
        }

        private void sldNodeShadeAmt_Scroll(object sender, EventArgs e)
        {
            Settings.Default.NodeShadeMax = sldNodeShadeAmt.Value / 100f;
            Settings.Default.Save();
        }
    }
}
