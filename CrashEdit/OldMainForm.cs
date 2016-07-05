using Crash;
using Crash.UI;
using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class OldMainForm : Form
    {
        private static ImageList imglist;
        private static GameVersion version;

        static OldMainForm()
        {
            imglist = new ImageList();
            version = new GameVersion();
            try
            {
                imglist.Images.Add("default",OldResources.FileImage);
                imglist.Images.Add("tb_open",OldResources.OpenImage);
                imglist.Images.Add("tb_save",OldResources.SaveImage);
                imglist.Images.Add("tb_patchnsd",OldResources.SaveImage);
                imglist.Images.Add("tb_close",OldResources.FolderImage);
                imglist.Images.Add("tb_find",OldResources.BinocularsImage);
                imglist.Images.Add("tb_findnext",OldResources.BinocularsNextImage);
            }
            catch
            {
                imglist.Images.Clear();
            }
        }

        private ToolStrip tsToolbar;
        private ToolStripButton tbbOpen;
        private ToolStripButton tbbSave;
        private ToolStripButton tbbPatchNSD;
        private ToolStripButton tbbClose;
        private ToolStripSeparator tbsSeparator;
        private ToolStripButton tbbFind;
        private ToolStripButton tbbFindNext;
        private TabControl tbcTabs;
        private GameVersionForm dlgGameVersion;

        public OldMainForm()
        {
            tbbOpen = new ToolStripButton();
            tbbOpen.Text = "Open";
            tbbOpen.ImageKey = "tb_open";
            tbbOpen.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbOpen.Click += new EventHandler(tbbOpen_Click);

            tbbSave = new ToolStripButton();
            tbbSave.Text = "Save";
            tbbSave.ImageKey = "tb_save";
            tbbSave.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbSave.Click += new EventHandler(tbbSave_Click);

            tbbPatchNSD = new ToolStripButton();
            tbbPatchNSD.Text = "Patch NSD";
            tbbPatchNSD.ImageKey = "tb_patchnsd";
            tbbPatchNSD.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbPatchNSD.Click += new EventHandler(tbbPatchNSD_Click);

            tbbClose = new ToolStripButton();
            tbbClose.Text = "Close";
            tbbClose.ImageKey = "tb_close";
            tbbClose.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbClose.Click += new EventHandler(tbbClose_Click);

            tbsSeparator = new ToolStripSeparator();

            tbbFind = new ToolStripButton();
            tbbFind.Text = "Find";
            tbbFind.ImageKey = "tb_find";
            tbbFind.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbFind.Click += new EventHandler(tbbFind_Click);

            tbbFindNext = new ToolStripButton();
            tbbFindNext.Text = "Find Next";
            tbbFindNext.ImageKey = "tb_findnext";
            tbbFindNext.TextImageRelation = TextImageRelation.ImageAboveText;
            tbbFindNext.Click += new EventHandler(tbbFindNext_Click);

            tsToolbar = new ToolStrip();
            tsToolbar.Dock = DockStyle.Top;
            tsToolbar.ImageList = imglist;
            tsToolbar.Items.Add(tbbOpen);
            tsToolbar.Items.Add(tbbSave);
            tsToolbar.Items.Add(tbbPatchNSD);
            tsToolbar.Items.Add(tbbClose);
            tsToolbar.Items.Add(tbsSeparator);
            tsToolbar.Items.Add(tbbFind);
            tsToolbar.Items.Add(tbbFindNext);

            tbcTabs = new TabControl();
            tbcTabs.Dock = DockStyle.Fill;

            dlgGameVersion = new GameVersionForm();

            Width = 640;
            Height = 480;
            Text = "CrashEdit";
            Controls.Add(tbcTabs);
            Controls.Add(tsToolbar);
        }

        void tbbOpen_Click(object sender,EventArgs e)
        {
            OpenNSF();
        }

        void tbbSave_Click(object sender,EventArgs e)
        {
            SaveNSF();
        }

        void tbbPatchNSD_Click(object sender,EventArgs e)
        {
            PatchNSD();
        }

        void tbbClose_Click(object sender,EventArgs e)
        {
            CloseNSF();
        }

        void tbbFind_Click(object sender,EventArgs e)
        {
            Find();
        }

        void tbbFindNext_Click(object sender,EventArgs e)
        {
            FindNext();
        }

        public void OpenNSF()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = FileFilters.NSF + "|" + FileFilters.Any;
                dialog.Multiselect = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in dialog.FileNames)
                    {
                        OpenNSF(filename);
                    }
                }
            }
        }

        public void OpenNSF(string filename)
        {
            try
            {
                byte[] nsfdata = File.ReadAllBytes(filename);
                if (dlgGameVersion.ShowDialog() == DialogResult.OK)
                {
                    NSF nsf = NSF.LoadAndProcess(nsfdata,dlgGameVersion.SelectedVersion);
                    OpenNSF(filename,nsf,dlgGameVersion.SelectedVersion);
                }
            }
            catch (LoadAbortedException)
            {
            }
        }

        public void OpenNSF(string filename,NSF nsf,GameVersion gameversion)
        {
            NSFBox nsfbox = new NSFBox(nsf,gameversion);
            nsfbox.Dock = DockStyle.Fill;

            TabPage nsftab = new TabPage(filename);
            nsftab.Tag = nsfbox;
            nsftab.Controls.Add(nsfbox);

            tbcTabs.TabPages.Add(nsftab);
            tbcTabs.SelectedTab = nsftab;
        }

        public void SaveNSF()
        {
            if (tbcTabs.SelectedTab != null)
            {
                string filename = tbcTabs.SelectedTab.Text;
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                SaveNSF(filename,nsfbox.NSF);
            }
        }

        public void SaveNSF(string filename,NSF nsf)
        {
            try
            {
                byte[] nsfdata = nsf.Save();
                if (MessageBox.Show("Are you sure you want to overwrite this file?","Save Confirmation Prompt",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.WriteAllBytes(filename,nsfdata);
                }
            }
            catch (PackingException ex)
            {
                MessageBox.Show(string.Format("A packing error occurred. The chunk containing entry '{0}' has over 64 KB of data.",Entry.EIDToEName(ex.EID)),"Save",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show("An IO error occurred.\n\n" + ex.Message,"Save",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("An unauthorized access error occurred.\n\n" + ex.Message,"Save",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void PatchNSD()
        {
            if (tbcTabs.SelectedTab != null)
            {
                string filename = tbcTabs.SelectedTab.Text;
                if (filename.EndsWith("F"))
                {
                    filename = filename.Remove(filename.Length - 1);
                    filename += "D";
                }
                else if (filename.EndsWith("f"))
                {
                    filename = filename.Remove(filename.Length - 1);
                    filename += "d";
                }
                else
                {
                    string message = string.Format("Can't figure out NSD filename.\n\nFOO.NSF -> FOO.NSD\n\n{0} -> ???",filename);
                    MessageBox.Show(message,"Patch NSD",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                PatchNSD(filename,nsfbox.NSF);
            }
        }

        public void PatchNSD(string filename,NSF nsf)
        {
            if (dlgGameVersion.SelectedVersion == GameVersion.Crash1 || dlgGameVersion.SelectedVersion == GameVersion.Crash2 || dlgGameVersion.SelectedVersion == GameVersion.Crash3)
            {
                try
                {
                    byte[] data = File.ReadAllBytes(filename);
                    NSD nsd = NSD.Load(data);
                    nsd.ChunkCount = nsf.Chunks.Count;
                    Dictionary<int, int> newindex = new Dictionary<int, int>();
                    for (int i = 0; i < nsf.Chunks.Count; i++)
                    {
                        if (nsf.Chunks[i] is IEntry)
                        {
                            IEntry entry = (IEntry)nsf.Chunks[i];
                            newindex.Add(entry.EID, i * 2 + 1);
                        }
                        if (nsf.Chunks[i] is EntryChunk)
                        {
                            foreach (Entry entry in ((EntryChunk)nsf.Chunks[i]).Entries)
                            {
                                newindex.Add(entry.EID, i * 2 + 1);
                            }
                        }
                    }
                    foreach (NSDLink link in nsd.Index)
                    {
                        if (newindex.ContainsKey(link.EntryID))
                        {
                            link.ChunkID = newindex[link.EntryID];
                            newindex.Remove(link.EntryID);
                        }
                    }
                    if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.WriteAllBytes(filename, nsd.Save());
                    }
                }
                catch (LoadAbortedException)
                {
                }
            }
            else
                throw new NotImplementedException("beta crash 1 nsd patch");
        }

        public void CloseNSF()
        {
            TabPage tab = tbcTabs.SelectedTab;
            if (tab != null)
            {
                tbcTabs.TabPages.Remove(tab);
                tab.Dispose();
            }
        }

        public void Find()
        {
            if (tbcTabs.SelectedTab != null)
            {
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                using (InputWindow inputwindow = new InputWindow())
                {
                    if (inputwindow.ShowDialog() == DialogResult.OK)
                    {
                        nsfbox.Find(inputwindow.Input);
                    }
                }
            }
        }

        public void FindNext()
        {
            if (tbcTabs.SelectedTab != null)
            {
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                nsfbox.FindNext();
            }
        }
    }
}
