using Crash;
using Crash.UI;
using System;
using System.IO;
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

        private FolderBrowserDialog dlgMakeBINDir = new FolderBrowserDialog();
        private SaveFileDialog dlgMakeBINFile = new SaveFileDialog();

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
            tbbExtra.DropDown = new ToolStripDropDown();
            tbbExtra.DropDown.Items.Add(tbxMakeBIN);
            tbbExtra.DropDown.Items.Add(tbxMakeBINUSA);
            tbbExtra.DropDown.Items.Add(tbxMakeBINEUR);
            tbbExtra.DropDown.Items.Add(tbxMakeBINJAP);
            tbbExtra.DropDown.Items.Add(new ToolStripSeparator());
            tbbExtra.DropDown.Items.Add(tbxConvertVHVB);
            tbbExtra.DropDown.Items.Add(tbxConvertVAB);

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

            tbcTabs = new TabControl
            {
                Dock = DockStyle.Fill
            };

            dlgGameVersion = new GameVersionForm();

            Width = 640;
            Height = 480;
            Text = "CrashEdit";
            Controls.Add(tbcTabs);
            Controls.Add(tsToolbar);

            dlgMakeBINFile.Filter = "Playstation Disc Images (*.bin)|*.bin";
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
                dialog.Dispose();
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
                    List<int> eids = new List<int>();
                    for (int i = 0; i < nsf.Chunks.Count; i++)
                    {
                        if (nsf.Chunks[i] is IEntry ientry)
                        {
                            newindex.Add(ientry.EID, i * 2 + 1);
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
                        eids.Add(link.EntryID);
                        if (newindex.ContainsKey(link.EntryID))
                        {
                            link.ChunkID = newindex[link.EntryID];
                            newindex.Remove(link.EntryID);
                        }
                    }
                    if (newindex.Count > 0)
                    {
                        List<string> neweids = new List<string>();
                        foreach (KeyValuePair<int, int> kvp in newindex)
                        {
                            neweids.Add(Entry.EIDToEName(kvp.Key));
                        }
                        string question = "The NSD is missing some entry ID's:\n\n";
                        foreach (string eid in neweids)
                        {
                            question += eid + "\n";
                        }
                        question += "\nDo you want to add these to the end of the NSD's entry index?";
                        if (MessageBox.Show(question, "Patch NSD - New EID's", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            foreach (KeyValuePair<int, int> kvp in newindex)
                            {
                                nsd.Index.Add(new NSDLink(kvp.Value, kvp.Key));
                                nsd.EntryCount++;
                            }
                        }
                    }
                    if (MessageBox.Show("Are you sure you want to overwrite the NSD file?", "Save Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        File.WriteAllBytes(filename, nsd.Save());
                    }
                    if (MessageBox.Show("Do you want to sort all Crash 2 and Crash 3 loadlists according to the NSD?", "Loadlist autosorter", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (Chunk chunk in nsf.Chunks)
                        {
                            if (!(chunk is EntryChunk))
                                continue;
                            foreach (Entry entry in ((EntryChunk)chunk).Entries)
                            {
                                if (entry is ZoneEntry)
                                {
                                    foreach (Entity ent in ((ZoneEntry)entry).Entities)
                                    {
                                        if (ent.LoadListA != null)
                                        {
                                            foreach (EntityPropertyRow<int> row in ent.LoadListA.Rows)
                                            {
                                                List<int> values = (List<int>)row.Values;
                                                values.Sort(delegate (int a,int b) {
                                                    return eids.IndexOf(a) - eids.IndexOf(b);
                                                });
                                            }
                                        }
                                        if (ent.LoadListB != null)
                                        {
                                            foreach (EntityPropertyRow<int> row in ent.LoadListB.Rows)
                                            {
                                                List<int> values = (List<int>)row.Values;
                                                values.Sort(delegate (int a,int b) {
                                                    return eids.IndexOf(a) - eids.IndexOf(b);
                                                });
                                            }
                                        }
                                    }
                                }
                                else if (entry is NewZoneEntry)
                                {
                                    foreach (Entity ent in ((NewZoneEntry)entry).Entities)
                                    {
                                        if (ent.LoadListA != null)
                                        {
                                            foreach (EntityPropertyRow<int> row in ent.LoadListA.Rows)
                                            {
                                                List<int> values = (List<int>)row.Values;
                                                values.Sort(delegate (int a,int b) {
                                                    return eids.IndexOf(a) - eids.IndexOf(b);
                                                });
                                            }
                                        }
                                        if (ent.LoadListB != null)
                                        {
                                            foreach (EntityPropertyRow<int> row in ent.LoadListB.Rows)
                                            {
                                                List<int> values = (List<int>)row.Values;
                                                values.Sort(delegate (int a,int b) {
                                                    return eids.IndexOf(a) - eids.IndexOf(b);
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
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

            if (dlgMakeBINDir.ShowDialog() != DialogResult.OK)
                return;

            string cnffile = Path.Combine(dlgMakeBINDir.SelectedPath, "SYSTEM.CNF");
            string exefile = Path.Combine(dlgMakeBINDir.SelectedPath, "PSX.EXE");

            if (!File.Exists(cnffile) && !File.Exists(exefile)) {
                if (MessageBox.Show("The selected drive or folder does not contain SYSTEM.CNF or PSX.EXE. At least one of these is required for a bootable PSX CD image. Continue anyway?", "Make BIN", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes)
                    return;
            }

            if (dlgMakeBINFile.ShowDialog() != DialogResult.OK)
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
