using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Crash;
using Crash.UI;
using DiscUtils.Iso9660;

namespace CrashHacks
{
    public partial class MainForm : Form
    {
        private string defaultinfo;
        private GameVersion gameversion;
        private CDBuilder cdbuilder;
        private List<Script> scripts;
        
        public string SourceDir
        {
            get { return dlgOpenDirectory.SelectedPath; }
            set { dlgOpenDirectory.SelectedPath = value; }
        }

        public string TargetBIN
        {
            get { return dlgSaveISO.FileName; }
            set { dlgSaveISO.FileName = value; }
        }

        public MainForm()
        {
            InitializeComponent();
            defaultinfo = lblInfo.Text;
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract)
                    continue;
                if (!typeof(Script).IsAssignableFrom(type))
                    continue;
                Script script = (Script)Activator.CreateInstance(type);
                ListViewItem item = new ListViewItem();
                item.Text = script.Name;
                StringBuilder tooltip = new StringBuilder();
                tooltip.AppendLine(script.Description);
                tooltip.AppendLine();
                if (script.CheckCompatibility((GameVersion)(-1)) == SupportLevel.Supported)
                {
                    tooltip.AppendLine("Compatible with all games.");
                }
                else
                {
                    tooltip.AppendLine("Compatible with:");
                    if (script.CheckCompatibility(GameVersion.Crash1) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 Crash Bandicoot");
                    if (script.CheckCompatibility(GameVersion.Crash2) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 Crash Bandicoot 2: Cortex Strikes Back");
                    if (script.CheckCompatibility(GameVersion.Crash3) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 Crash Bandicoot: Warped");
                    if (script.CheckCompatibility(GameVersion.Crash1BetaMAR08) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 CB Prototype (April 8th)");
                    if (script.CheckCompatibility(GameVersion.Crash1BetaMAY11) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 CB Prototype (May 11th)");
                    if (script.CheckCompatibility(GameVersion.None) == SupportLevel.Supported)
                        tooltip.AppendLine("\u2022 Other (???)");
                }
                tooltip.AppendLine();
                tooltip.AppendFormat("Author: {0}",script.Author);
                item.ToolTipText = tooltip.ToString();
                item.Tag = script;
                item.Group = lsvScripts.Groups["other"];
                foreach (ListViewGroup group in lsvScripts.Groups)
                {
                    if (group.Name == script.Category)
                    {
                        item.Group = group;
                        break;
                    }
                }
                item.SubItems.Add("???");
                item.UseItemStyleForSubItems = false;
                lsvScripts.Items.Add(item);
            }
            SetGameVersion(GameVersion.None);
        }

        private void SetGameVersion(GameVersion gameversion)
        {
            foreach (ListViewItem item in lsvScripts.Items)
            {
                Script script = (Script)item.Tag;
                switch (script.CheckCompatibility(gameversion))
                {
                    case SupportLevel.Supported:
                        item.SubItems[1].Text = "OK";
                        item.SubItems[1].BackColor = Color.LightGreen;
                        break;
                    case SupportLevel.Untested:
                        item.SubItems[1].Text = "Untested";
                        item.SubItems[1].BackColor = Color.LightYellow;
                        break;
                    case SupportLevel.Unsupported:
                        item.SubItems[1].Text = "Incompatible";
                        item.SubItems[1].BackColor = Color.LightCoral;
                        break;
                    case SupportLevel.Experimental:
                        item.SubItems[1].Text = "Experimental";
                        item.SubItems[1].BackColor = Color.LightPink;
                        break;
                    default:
                        item.SubItems[1].Text = "???";
                        item.SubItems[1].BackColor = Color.Red;
                        break;
                }
            }
            this.gameversion = gameversion;
            lblGameVersion.Text = gameversion.ToString();
        }

        private void tbiMakeISO_Click(object sender,EventArgs e)
        {
            if (dlgOpenDirectory.ShowDialog() != DialogResult.OK)
                return;
            string path = dlgOpenDirectory.SelectedPath;
            string cnffile = Path.Combine(path,"SYSTEM.CNF");
            string exefile = Path.Combine(path,"PSX.EXE");
            if (!File.Exists(cnffile) && !File.Exists(exefile))
            {
                if (MessageBox.Show("The selected drive or folder does not appear to contain the necessary game files. The directory you choose should contain a SYSTEM.CNF or PSX.EXE file as well as subdirectories with names such as S0, S1, S2, etc.\n\nContinue anyway?","CrashHacks",MessageBoxButtons.YesNo,MessageBoxIcon.Stop) != DialogResult.Yes)
                    return;
            }
            if (dlgSaveISO.ShowDialog() != DialogResult.OK)
                return;
            cdbuilder = new CDBuilder();
            scripts = new List<Script>();
            foreach (ListViewItem item in lsvScripts.Items)
            {
                if (item.Checked)
                {
                    scripts.Add((Script)item.Tag);
                }
            }
            tbiMakeISO.Enabled = false;
            tbiChooseGameVersion.Enabled = false;
            lblGameVersion.Enabled = false;
            lsvScripts.Enabled = false;
            lsvScripts.Visible = false;
            lblInfo.Enabled = false;
            lblMessage.Text = "Running scripts...";
            uxProgress.Visible = true;
            uxProgress.ForeColor = Color.Lime;
            uxProgress.Style = ProgressBarStyle.Marquee;
            uxProgress.Value = 0;
            bgwMakeISO.RunWorkerAsync(dlgOpenDirectory.SelectedPath);
        }

        private void tbiChooseGameVersion_Click(object sender,EventArgs e)
        {
            using (GameVersionForm gameversionform = new GameVersionForm())
            {
                if (gameversionform.ShowDialog() == DialogResult.OK)
                {
                    if (gameversionform.SelectedVersion == GameVersion.Crash1)
                    {
                        if (MessageBox.Show("Retail (non-prototype) versions of Crash 1 are not properly supported. CrashHacks will output unusable NSF files for this game. Continue anyway?","CrashHacks",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) != DialogResult.Yes)
                            return;
                    }
                    SetGameVersion(gameversionform.SelectedVersion);
                    tbiMakeISO.Enabled = true;
                    lsvScripts.Visible = true;
                }
            }
        }

        private void lsvScripts_ItemSelectionChanged(object sender,ListViewItemSelectionChangedEventArgs e)
        {
            if (lsvScripts.SelectedItems.Count == 1)
                lblInfo.Text = lsvScripts.SelectedItems[0].ToolTipText;
            else
                lblInfo.Text = defaultinfo;
        }

        private void bgwMakeISO_DoWork(object sender,DoWorkEventArgs e)
        {
            ErrorManager.Signal += ErrorManager_Signal;
            DirectoryInfo dir = new DirectoryInfo((string)e.Argument);
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                bgwMakeISO_DoWork_BuildDirectory(subdir.Name,subdir);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                bgwMakeISO_DoWork_BuildFile(file.Name,file);
            }
            ErrorManager.Signal -= ErrorManager_Signal;
        }

        private void bgwMakeISO_DoWork_BuildDirectory(string path,DirectoryInfo dir)
        {
            cdbuilder.AddDirectory(path);
            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                bgwMakeISO_DoWork_BuildDirectory(path + "\\" + subdir.Name,subdir);
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                bgwMakeISO_DoWork_BuildFile(path + "\\" + file.Name,file);
            }
        }

        private void bgwMakeISO_DoWork_BuildFile(string path,FileInfo file)
        {
            if (file.Extension.ToUpper() == ".NSF")
            {
                byte[] data = File.ReadAllBytes(file.FullName);
                if (scripts.Count > 0)
                {
                    NSF nsf = NSF.LoadAndProcess(data,gameversion);
                    foreach (Script script in scripts)
                    {
                        script.Run(nsf,gameversion);
                        foreach (Chunk chunk in nsf.Chunks)
                        {
                            script.Run(chunk,gameversion);
                            if (chunk is EntryChunk entrychunk)
                            {
                                foreach (Entry entry in entrychunk.Entries)
                                {
                                    script.Run(entry,gameversion);
                                }
                            }
                        }
                    }
                    try {
                        data = nsf.Save();
                    } catch (PackingException ex) {
                        throw new Exception(string.Format("packing error on file: {0}",file.FullName),ex);
                    }
                }
                cdbuilder.AddFile(path + ";1",data);
            }
            else
            {
                cdbuilder.AddFile(path + ";1",file.FullName);
            }
        }

        private void bgwMakeISO_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                lblMessage.Text = "Writing BIN...";
                uxProgress.ForeColor = Color.Red;
                uxProgress.Style = ProgressBarStyle.Blocks;
                uxProgress.Value = 0;
                bgwMakeBIN.RunWorkerAsync(dlgSaveISO.FileName);
            }
            else
            {
                MessageBox.Show("An error occurred:\n\n" + e.Error.ToString(),"CrashHacks",MessageBoxButtons.OK,MessageBoxIcon.Error);
                FinishBuild();
            }
        }

        private void ErrorManager_Signal(object sender,ErrorSignalEventArgs e)
        {
            if (e.CanIgnore)
            {
                e.Response = ErrorResponse.Ignore;
            }
            else if (e.CanSkip)
            {
                e.Response = ErrorResponse.Skip;
            }
        }

        private void FinishBuild()
        {
            cdbuilder = null;
            scripts = null;
            tbiMakeISO.Enabled = true;
            tbiChooseGameVersion.Enabled = true;
            lblGameVersion.Enabled = true;
            lsvScripts.Enabled = true;
            lsvScripts.Visible = true;
            lblInfo.Enabled = true;
            lblMessage.Text = "";
            uxProgress.Visible = false;
        }

        private void bgwMakeBIN_DoWork(object sender,DoWorkEventArgs e)
        {
            using (FileStream output = new FileStream((string)e.Argument,FileMode.Create,FileAccess.Write))
            using (Stream input = cdbuilder.Build())
            {
                ISO2PSX.Run(input,output,bgwMakeBIN);
            }
        }

        private void bgwMakeBIN_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("An error occurred:\n\n" + e.Error.ToString(),"CrashHacks",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            FinishBuild();
        }

        private void bgwMakeBIN_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            uxProgress.Value = e.ProgressPercentage;
        }
    }
}
