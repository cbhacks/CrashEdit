using Crash;
using Crash.UI;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using DiscUtils.Iso9660;

namespace CrashEdit
{
    public sealed class OldMainForm : Form
    {
        private static ImageList imglist;

        static OldMainForm()
        {
            imglist = new ImageList();
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
        private ToolStripButton tbbFind;
        private ToolStripButton tbbFindNext;
        private ToolStripMenuItem tbxMakeBIN;
        private ToolStripMenuItem tbxMakeBINUSA;
        private ToolStripMenuItem tbxMakeBINEUR;
        private ToolStripMenuItem tbxMakeBINJAP;
        private ToolStripMenuItem tbxConvertVHVB;
        private ToolStripMenuItem tbxConvertVAB;
        private ToolStripDropDownButton tbbExtra;
        private TabControl tbcTabs;
        private GameVersionForm dlgGameVersion;
        private ToolStripButton tbbPAL;

        private FolderBrowserDialog dlgMakeBINDir = new FolderBrowserDialog();
        private SaveFileDialog dlgMakeBINFile = new SaveFileDialog();

        private static bool PAL = false;
        private const int RateNTSC = 30;
        private const int RatePAL = 25;

        public OldMainForm()
        {
            tbbOpen = new ToolStripButton
            {
                Text = "Open",
                ImageKey = "tb_open",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbOpen.Click += new EventHandler(tbbOpen_Click);

            tbbSave = new ToolStripButton
            {
                Text = "Save",
                ImageKey = "tb_save",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbSave.Click += new EventHandler(tbbSave_Click);

            tbbPatchNSD = new ToolStripButton
            {
                Text = "Patch NSD",
                ImageKey = "tb_patchnsd",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbPatchNSD.Click += new EventHandler(tbbPatchNSD_Click);

            tbbClose = new ToolStripButton
            {
                Text = "Close",
                ImageKey = "tb_close",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbClose.Click += new EventHandler(tbbClose_Click);

            tbbFind = new ToolStripButton
            {
                Text = "Find",
                ImageKey = "tb_find",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbFind.Click += new EventHandler(tbbFind_Click);

            tbbFindNext = new ToolStripButton
            {
                Text = "Find Next",
                ImageKey = "tb_findnext",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbFindNext.Click += new EventHandler(tbbFindNext_Click);

            tbxMakeBIN = new ToolStripMenuItem();
            tbxMakeBIN.Text = "Make BIN (no region)";
            tbxMakeBIN.Click += new EventHandler(tbxMakeBIN_Click);

            tbxMakeBINUSA = new ToolStripMenuItem();
            tbxMakeBINUSA.Text = "Make BIN (NTSC-U/C)";
            tbxMakeBINUSA.Click += new EventHandler(tbxMakeBIN_Click);

            tbxMakeBINEUR = new ToolStripMenuItem();
            tbxMakeBINEUR.Text = "Make BIN (PAL)";
            tbxMakeBINEUR.Click += new EventHandler(tbxMakeBIN_Click);

            tbxMakeBINJAP = new ToolStripMenuItem();
            tbxMakeBINJAP.Text = "Make BIN (NTSC-J)";
            tbxMakeBINJAP.Click += new EventHandler(tbxMakeBIN_Click);

            tbxConvertVHVB = new ToolStripMenuItem();
            tbxConvertVHVB.Text = "Convert VH+VB to DLS";
            tbxConvertVHVB.Click += new EventHandler(tbxConvertVHVB_Click);

            tbxConvertVAB = new ToolStripMenuItem();
            tbxConvertVAB.Text = "Convert VAB to DLS";
            tbxConvertVAB.Click += new EventHandler(tbxConvertVAB_Click);

            tbbExtra = new ToolStripDropDownButton();
            tbbExtra.Text = "Extra Features";
            tbbExtra.DropDown = new ToolStripDropDown { LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow };
            tbbExtra.DropDown.Items.Add(tbxMakeBIN);
            tbbExtra.DropDown.Items.Add(tbxMakeBINUSA);
            tbbExtra.DropDown.Items.Add(tbxMakeBINEUR);
            tbbExtra.DropDown.Items.Add(tbxMakeBINJAP);
            tbbExtra.DropDown.Items.Add("-");
            tbbExtra.DropDown.Items.Add(tbxConvertVHVB);
            tbbExtra.DropDown.Items.Add(tbxConvertVAB);

            tbbPAL = new ToolStripButton
            {
                Text = "PAL",
                TextImageRelation = TextImageRelation.ImageAboveText,
                Checked = false,
                CheckOnClick = true
            };
            tbbPAL.Click += new EventHandler(tbbPAL_Click);

            tsToolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                ImageList = imglist
            };
            tsToolbar.Items.Add(tbbOpen);
            tsToolbar.Items.Add(tbbSave);
            tsToolbar.Items.Add(tbbPatchNSD);
            tsToolbar.Items.Add(tbbClose);
            tsToolbar.Items.Add(new ToolStripSeparator());
            tsToolbar.Items.Add(tbbFind);
            tsToolbar.Items.Add(tbbFindNext);
            tsToolbar.Items.Add(new ToolStripSeparator());
            tsToolbar.Items.Add(tbbExtra);
            tsToolbar.Items.Add(tbbPAL);

            tbcTabs = new TabControl
            {
                Dock = DockStyle.Fill
            };
            tbcTabs.SelectedIndexChanged += tbcTabs_SelectedIndexChanged;

            TabPage configtab = new TabPage("CrashEdit")
            {
                Tag = null
            };
            configtab.Controls.Add(new ConfigEditor() { Dock = DockStyle.Fill });

            tbcTabs.TabPages.Add(configtab);

            tbcTabs_SelectedIndexChanged(null,null);

            dlgGameVersion = new GameVersionForm();

            Width = 747;
            Height = 560;
            Text = $"CrashEdit v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            Controls.Add(tbcTabs);
            Controls.Add(tsToolbar);

            dlgMakeBINFile.Filter = "Playstation Disc Images (*.bin)|*.bin";
        }

        private void tbcTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tab = tbcTabs.SelectedTab;
            tbbSave.Enabled =
            tbbPatchNSD.Enabled =
            tbbClose.Enabled =
            tbbFind.Enabled =
            tbbFindNext.Enabled = tab != null && tab.Tag is NSFBox;
        }

        void tbbPAL_Click(object sender, EventArgs e)
        {
            PAL = tbbPAL.Checked;
        }

        public static int GetRate()
        {
            return PAL ? RatePAL : RateNTSC;
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
                if (dialog.ShowDialog(this) == DialogResult.OK)
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
                if (dlgGameVersion.ShowDialog(this) == DialogResult.OK)
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
            NSFBox nsfbox = new NSFBox(nsf, gameversion)
            {
                Dock = DockStyle.Fill
            };

            TabPage nsftab = new TabPage(filename)
            {
                Tag = nsfbox
            };
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
                    MessageBox.Show(string.Format("Can't figure out NSD filename. Make sure NSF file ends in \"f\" (case-insensitive)!\n\nFOO.NSF -> FOO.NSD\n\n{0} -> ???", filename), "Patch NSD",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                PatchNSD(filename,nsfbox.NSFController);
            }
        }

        public void PatchNSD(string filename, NSFController nsfc)
        {
            //if (MessageBox.Show("The chunk contents in this NSF may be moved in accordance to the patched NSD. Continue anyway?", "Patch NSD", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                NSF nsf = nsfc.NSF;
                byte[] data = File.ReadAllBytes(filename);
                switch (nsfc.GameVersion)
                {
                    case GameVersion.Crash1BetaMAR08:
                        {
                            ProtoNSD nsd = ProtoNSD.Load(data);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash1:
                        {
                            OldNSD nsd = OldNSD.Load(data);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash2:
                        {
                            NSD nsd = NSD.Load(data);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash3:
                        {
                            NewNSD nsd = NewNSD.Load(data);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    default:
                        MessageBox.Show("NSD patching is not supported for this game version.", "Patch NSD", MessageBoxButtons.OK);
                        break;
                }
                nsfc.Node.TreeView.BeginUpdate();
                foreach (TreeNode node in nsfc.Node.Nodes) // nsd patching might have moved entries, recreate every single controller just in case
                {
                    if (node.Tag is EntryChunkController entrychunkcontroller)
                    {
                        TreeNode[] nodes = new TreeNode[node.Nodes.Count];
                        int i = 0;
                        foreach (TreeNode oldnode in node.Nodes)
                        {
                            nodes[i++] = oldnode;
                        }
                        for (i = 0; i < nodes.Length; ++i)
                        {
                            if (nodes[i].Tag != null)
                            {
                                if (nodes[i].Tag is Controller t)
                                {
                                    t.Dispose();
                                }
                            }
                        }
                        entrychunkcontroller.PopulateNodes();
                    }
                }
                nsfc.Node.TreeView.EndUpdate();
            }
            catch (LoadAbortedException)
            {
            }
        }

        public void PatchNSD(NewNSD nsd, NSF nsf, string path)
        {
            nsd.ChunkCount = nsf.Chunks.Count;
            var indexdata = nsf.MakeNSDIndex();
            nsd.HashKeyMap = indexdata.Item1;
            nsd.Index = indexdata.Item2;

            // patch object entity count
            nsd.EntityCount = 0;
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (!(chunk is EntryChunk))
                    continue;
                foreach (Entry entry in ((EntryChunk)chunk).Entries)
                {
                    if (entry is NewZoneEntry zone)
                        foreach (Entity ent in zone.Entities)
                            if (ent.ID != null)
                                ++nsd.EntityCount;
                }
            }

            if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllBytes(path, nsd.Save());
            }
            if (MessageBox.Show("Do you want to sort all loadlists according to the NSD?", "Loadlist autosorter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int[] eids = new int[nsd.Index.Count];
                for (int i = 0; i < eids.Length; ++i)
                    eids[i] = nsd.Index[i].EntryID;
                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (!(chunk is EntryChunk))
                        continue;
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is NewZoneEntry zone)
                        {
                            foreach (Entity ent in zone.Entities)
                            {
                                if (ent.LoadListA != null)
                                {
                                    foreach (EntityPropertyRow<int> row in ent.LoadListA.Rows)
                                    {
                                        List<int> values = (List<int>)row.Values;
                                        values.Sort(delegate (int a, int b) {
                                            return Array.IndexOf(eids,a) - Array.IndexOf(eids,b);
                                        });
                                    }
                                }
                                if (ent.LoadListB != null)
                                {
                                    foreach (EntityPropertyRow<int> row in ent.LoadListB.Rows)
                                    {
                                        List<int> values = (List<int>)row.Values;
                                        values.Sort(delegate (int a, int b) {
                                            return Array.IndexOf(eids,a) - Array.IndexOf(eids,b);
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PatchNSD(NSD nsd, NSF nsf, string path)
        {
            nsd.ChunkCount = nsf.Chunks.Count;
            var indexdata = nsf.MakeNSDIndex();
            nsd.HashKeyMap = indexdata.Item1;
            nsd.Index = indexdata.Item2;

            // patch object entity count
            nsd.EntityCount = 0;
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (!(chunk is EntryChunk))
                    continue;
                foreach (Entry entry in ((EntryChunk)chunk).Entries)
                {
                    if (entry is ZoneEntry zone)
                        foreach (Entity ent in zone.Entities)
                            if (ent.ID != null)
                                ++nsd.EntityCount;
                }
            }

            if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllBytes(path, nsd.Save());
            }
            if (MessageBox.Show("Do you want to sort all loadlists according to the NSD?", "Loadlist autosorter", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int[] eids = new int[nsd.Index.Count];
                for (int i = 0; i < eids.Length; ++i)
                    eids[i] = nsd.Index[i].EntryID;
                foreach (Chunk chunk in nsf.Chunks)
                {
                    if (!(chunk is EntryChunk))
                        continue;
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is ZoneEntry zone)
                        {
                            foreach (Entity ent in zone.Entities)
                            {
                                if (ent.LoadListA != null)
                                {
                                    foreach (EntityPropertyRow<int> row in ent.LoadListA.Rows)
                                    {
                                        List<int> values = (List<int>)row.Values;
                                        values.Sort(delegate (int a, int b) {
                                            return Array.IndexOf(eids,a) - Array.IndexOf(eids,b);
                                        });
                                    }
                                }
                                if (ent.LoadListB != null)
                                {
                                    foreach (EntityPropertyRow<int> row in ent.LoadListB.Rows)
                                    {
                                        List<int> values = (List<int>)row.Values;
                                        values.Sort(delegate (int a, int b) {
                                            return Array.IndexOf(eids,a) - Array.IndexOf(eids,b);
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PatchNSD(OldNSD nsd, NSF nsf, string path)
        {
            nsd.ChunkCount = nsf.Chunks.Count;
            var indexdata = nsf.MakeNSDIndex();
            nsd.HashKeyMap = indexdata.Item1;
            nsd.Index = indexdata.Item2;
            if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllBytes(path, nsd.Save());
            }
        }

        public void PatchNSD(ProtoNSD nsd, NSF nsf, string path)
        {
            nsd.ChunkCount = nsf.Chunks.Count;
            var indexdata = nsf.MakeNSDIndex();
            nsd.HashKeyMap = indexdata.Item1;
            nsd.Index = indexdata.Item2;
            if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                File.WriteAllBytes(path, nsd.Save());
            }
        }
        
        public void CloseNSF()
        {
            if (MessageBox.Show("Are you sure you want to close the NSF file?", "Close Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TabPage tab = tbcTabs.SelectedTab;
                if (tab != null)
                {
                    tbcTabs.TabPages.Remove(tab);
                    tab.Dispose();
                }
            }
        }

        public void Find()
        {
            if (tbcTabs.SelectedTab != null)
            {
                NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
                using (InputWindow inputwindow = new InputWindow())
                {
                    if (inputwindow.ShowDialog(this) == DialogResult.OK)
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

        void AddDirectoryToISO(CDBuilder fs, string prefix, DirectoryInfo dir)
        {
            foreach (DirectoryInfo subdir in dir.GetDirectories()) {
                AddDirectoryToISO(fs, $"{prefix}{subdir.Name}\\", subdir);
            }
            foreach (FileInfo file in dir.GetFiles()) {
                fs.AddFile($"{prefix}{file.Name};1", file.FullName);
            }
        }

        void tbxMakeBIN_Click(object sender,EventArgs e)
        {
            var log = new StringBuilder();

            if (dlgMakeBINDir.ShowDialog(this) != DialogResult.OK)
                return;

            string cnffile = Path.Combine(dlgMakeBINDir.SelectedPath, "SYSTEM.CNF");
            string exefile = Path.Combine(dlgMakeBINDir.SelectedPath, "PSX.EXE");

            if (!File.Exists(cnffile) && !File.Exists(exefile)) {
                if (MessageBox.Show("The selected drive or folder does not contain SYSTEM.CNF or PSX.EXE. At least one of these is required for a bootable PSX CD image. Continue anyway?", "Make BIN", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes)
                    return;
            }

            if (dlgMakeBINFile.ShowDialog(this) != DialogResult.OK)
                return;

            var fs = new CDBuilder();
            AddDirectoryToISO(fs, "", new DirectoryInfo(dlgMakeBINDir.SelectedPath));

            using (var bin = new FileStream(dlgMakeBINFile.FileName, FileMode.Create, FileAccess.Write))
            using (var iso = fs.Build()) {
                ISO2PSX.Run(iso, bin);
            }

            log.AppendLine("Created BIN file without region OK.");
            log.AppendLine();

            string cueFilename = Path.ChangeExtension(dlgMakeBINFile.FileName, ".cue");
            if (!File.Exists(cueFilename)) {
                try {
                    using (var cue = new StreamWriter(cueFilename)) {
                        cue.WriteLine($"FILE \"{Path.GetFileName(dlgMakeBINFile.FileName)}\" BINARY");
                        cue.WriteLine("  TRACK 01 MODE2/2352");
                        cue.WriteLine("    INDEX 01 00:00:00");
                    }
                    log.AppendLine("Created matching CUE file.");
                    log.AppendLine();
                } catch (IOException ex) {
                    log.AppendLine($"Failed to create CUE file: {ex}");
                    log.AppendLine();
                }
            } else {
                log.AppendLine("CUE file already exists, will not be modified.");
                log.AppendLine();
            }

            string imprintOpt;
            if (sender == tbxMakeBINUSA) {
                imprintOpt = ":cdxa-imprint --psx-scea";
            } else if (sender == tbxMakeBINEUR) {
                imprintOpt = ":cdxa-imprint --psx-scee";
            } else if (sender == tbxMakeBINJAP) {
                imprintOpt = ":cdxa-imprint --psx-scei";
            } else {
                log.Append("Done.");
                MessageBox.Show(log.ToString());
                return;
            }

            log.AppendLine("Launching DRNSF to apply selected region...");
            try {
                if (DRNSF.Invoke($"{imprintOpt} -- \"{dlgMakeBINFile.FileName}\"") != 0) {
                    log.AppendLine("DRNSF returned an error. No region has been applied.");
                    log.AppendLine();
                } else {
                    log.AppendLine("Region applied successfully.");
                    log.AppendLine();
                }
            } catch (FileNotFoundException) {
                log.AppendLine("Could not find DRNSF exe. Please place this in the same directory as CrashEdit.");
                log.AppendLine();
            } catch (Exception ex) {
                log.AppendLine($"Failed to launch DRNSF. Reason: {ex}");
                log.AppendLine();
            }
            log.Append("Done.");
            MessageBox.Show(log.ToString());
        }

        void tbxConvertVHVB_Click(object sender,EventArgs e)
        {
            try
            {
                byte[] vh_data = FileUtil.OpenFile(FileFilters.VH, FileFilters.Any);
                if (vh_data == null) throw new LoadAbortedException();
                byte[] vb_data = FileUtil.OpenFile(FileFilters.VB, FileFilters.Any);
                if (vb_data == null) throw new LoadAbortedException();

                VH vh = VH.Load(vh_data);

                if (vb_data.Length / 16 != vh.VBSize)
                {
                    ErrorManager.SignalIgnorableError("extra feature: VB size does not match size specified in VH");
                }
                SampleLine[] vb = new SampleLine [vb_data.Length / 16];
                byte[] line_data = new byte[16];
                for (int i = 0; i < vb.Length; i++)
                {
                    Array.Copy(vb_data, i * 16, line_data, 0, 16);
                    vb[i] = SampleLine.Load(line_data);
                }

                VAB vab = VAB.Join(vh, vb);

                FileUtil.SaveFile(vab.ToDLS().Save(), FileFilters.DLS, FileFilters.Any);
            }
            catch (LoadAbortedException)
            {
            }
        }

        void tbxConvertVAB_Click(object sender,EventArgs e)
        {
            try
            {
                byte[] vab_data = FileUtil.OpenFile(FileFilters.VAB, FileFilters.Any);

                if (vab_data == null) throw new LoadAbortedException();

                VH vh = VH.Load(vab_data);

                int vb_offset = 2592+32*16*vh.Programs.Count;
                if ((vab_data.Length - vb_offset) % 16 != 0)
                {
                    ErrorManager.SignalIgnorableError("extra feature: VB size is invalid");
                }
                vh.VBSize = (vab_data.Length - vb_offset) / 16;
                SampleLine[] vb = new SampleLine [vh.VBSize];
                byte[] line_data = new byte[16];
                for (int i = 0; i < vb.Length; i++)
                {
                    Array.Copy(vab_data, vb_offset + i * 16, line_data, 0, 16);
                    vb[i] = SampleLine.Load(line_data);
                }

                VAB vab = VAB.Join(vh, vb);

                FileUtil.SaveFile(vab.ToDLS().Save(), FileFilters.DLS, FileFilters.Any);
            }
            catch (LoadAbortedException)
            {
            }
        }
    }
}
