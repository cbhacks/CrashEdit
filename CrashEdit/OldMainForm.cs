using Crash;
using Crash.UI;
using DiscUtils.Iso9660;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Xml;
using CrashEdit.Properties;

namespace CrashEdit
{
    public sealed class OldMainForm : Form
    {
        private static ImageList imglist;

        static OldMainForm()
        {
            imglist = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
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
        private ToolStripButton tbbPlay;
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
                Text = Properties.Resources.Toolbar_Open,
                ImageKey = "tb_open",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbOpen.Click += new EventHandler(tbbOpen_Click);

            tbbSave = new ToolStripButton
            {
                Text = Properties.Resources.Toolbar_Save,
                ImageKey = "tb_save",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbSave.Click += new EventHandler(tbbSave_Click);

            tbbPatchNSD = new ToolStripButton
            {
                Text = Properties.Resources.Toolbar_PatchNSD,
                ImageKey = "tb_patchnsd",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbPatchNSD.Click += new EventHandler(tbbPatchNSD_Click);

            tbbClose = new ToolStripButton
            {
                Text = Properties.Resources.Toolbar_Close,
                ImageKey = "tb_close",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbClose.Click += new EventHandler(tbbClose_Click);

            tbbFind = new ToolStripButton
            {
                Text = Properties.Resources.Toolbar_Find,
                ImageKey = "tb_find",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbFind.Click += new EventHandler(tbbFind_Click);

            tbbFindNext = new ToolStripButton
            {
                Text = Properties.Resources.Toolbar_FindNext,
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

            tbbPlay = new ToolStripButton
            {
                Text = "Play",
                TextImageRelation = TextImageRelation.ImageAboveText
            };
            tbbPlay.Click += new EventHandler(tbbPlay_Click);

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
            tsToolbar.Items.Add(new ToolStripSeparator());
            tsToolbar.Items.Add(tbbPlay);

            tbcTabs = new TabControl
            {
                Dock = DockStyle.Fill
            };
            tbcTabs.SelectedIndexChanged += tbcTabs_SelectedIndexChanged;

            TabPage configtab = new TabPage("CrashEdit")
            {
                Tag = new ConfigEditor() { Dock = DockStyle.Fill }
            };
            configtab.Controls.Add((ConfigEditor)configtab.Tag);

            tbcTabs.TabPages.Add(configtab);

            tbcTabs_SelectedIndexChanged(null,null);

            dlgGameVersion = new GameVersionForm();

            Icon = OldResources.CBHacksIcon;
            Width = Properties.Settings.Default.DefaultFormW;
            Height = Properties.Settings.Default.DefaultFormH;
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
            tbbFindNext.Enabled =
            tbbPlay.Enabled = tab != null && tab.Tag is NSFBox;
        }

        void tbbPAL_Click(object sender, EventArgs e)
        {
            PAL = tbbPAL.Checked;
        }

        void tbbPlay_Click(object sender, EventArgs e)
        {
            var tab = tbcTabs.SelectedTab;
            if (tab == null || !(tab.Tag is NSFBox))
                return;

            var nsfBox = (NSFBox)tab.Tag;
            var nsf = nsfBox.NSF;

            var nsfFilename = tbcTabs.SelectedTab.Text;

            var nsfFilenameBase = Path.GetFileName(nsfFilename);
            if (nsfFilenameBase.Length != 12) {
                MessageBox.Show($"NSF filename '{nsfFilenameBase}' is not appropriate!", "Playtest", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var levelID = int.Parse(nsfFilenameBase.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

            string nsdFilename;
            if (nsfFilename.EndsWith("F"))
            {
                nsdFilename = nsfFilename.Remove(nsfFilename.Length - 1);
                nsdFilename += "D";
            }
            else if (nsfFilename.EndsWith("f"))
            {
                nsdFilename = nsfFilename.Remove(nsfFilename.Length - 1);
                nsdFilename += "d";
            }
            else
            {
                MessageBox.Show(string.Format("Can't figure out NSD filename. Make sure NSF file ends in \"f\" (case-insensitive)!\n\nFOO.NSF -> FOO.NSD\n\n{0} -> ???", nsfFilename), "Playtest",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(nsdFilename)) {
                MessageBox.Show($"NSD file '{nsdFilename}' does not exist!", "Playtest", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string exeFilename = null;
            var isofsPath = Path.GetDirectoryName(Path.GetDirectoryName(nsfFilename));
            foreach (string s in Directory.GetFiles(isofsPath)) {
                if (Regex.IsMatch(Path.GetFileName(s).ToUpper(), @"^(S[CL][UEP]S_\d\d\d\.\d\d|PSX\.EXE)$")) {
                    exeFilename = s;
                    break;
                }
            }
            if (exeFilename == null) {
                MessageBox.Show("Could not find exe file (PSX.EXE, SCUS_123.45, SLES_123.45, etc).", "Playtest", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string warpscusFilename = null;
            foreach (string s in Directory.GetFiles(Path.Combine(isofsPath, "S0"))) {
                if (Regex.IsMatch(Path.GetFileName(s).ToUpper(), @"^WARPSC[UEP]S\.BIN$")) {
                    warpscusFilename = s;
                    break;
                }
            }

            string basePath;
            do {
                basePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            } while (Directory.Exists(basePath));
            Directory.CreateDirectory(basePath);

            var fs = new CDBuilder();
            fs.AddFile("S0\\" + Path.GetFileName(nsfFilename) + ";1", nsf.Save());
            fs.AddFile("S0\\" + Path.GetFileName(nsdFilename) + ";1", nsdFilename);
            fs.AddFile("PSX.EXE;1", exeFilename);
            if (warpscusFilename != null)
            {
                fs.AddFile("S0\\" + Path.GetFileName(warpscusFilename) + ";1", warpscusFilename);
            }

            string binPath = Path.Combine(basePath, "game.bin");
            using (var bin = new FileStream(binPath, FileMode.Create, FileAccess.Write))
            using (var iso = fs.Build()) {
                ISO2PSX.Run(iso, bin);
            }

            var regionStr = PAL ? "pal" : "ntsc";

            Task.Run(() => {
                ExternalTool.Invoke("pcsx-hdbg", $"gamefile=\"{binPath}\" bootlevel={levelID} region={regionStr}");
                Directory.Delete(basePath, true);
            });
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
                bool exists = true;
                if (!File.Exists(filename))
                {
                    //if (MessageBox.Show("NSD file does not exist. Create one?", "Patch NSD", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) != DialogResult.Yes)
                    {
                        return;
                    }
                    if (nsfbox.NSFController.GameVersion != GameVersion.Crash1BetaMAR08 && MessageBox.Show("Default NSD file is not a valid NSD file and needs to be manually fixed using a hex editor. Continue anyway?", "Patch NSD", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    {
                        return;
                    }
                    exists = false;
                }
                PatchNSD(filename,exists,nsfbox.NSFController);
            }
        }

        public void PatchNSD(string filename, bool exists, NSFController nsfc)
        {
            NSF nsf = nsfc.NSF;
            byte[] data = exists ? File.ReadAllBytes(filename) : null;
            try
            {
                switch (nsfc.GameVersion)
                {
                    case GameVersion.Crash1BetaMAR08:
                        {
                            ProtoNSD nsd = data != null ? ProtoNSD.Load(data) : new ProtoNSD(new int[256], 0, new NSDLink[0]);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash1:
                        {
                            OldNSD nsd = data != null ? OldNSD.Load(data) : new OldNSD(new int[256], 0, new int[4], 0, 0, new int[64], new NSDLink[0], 1, 0x3F, Entry.NullEID, 0, 0, new int[64], new byte[0xFC]);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash2:
                        {
                            NSD nsd = data != null ? NSD.Load(data) : new NSD(new int[256], 0, new int[4], 0, 0, new int[64], new NSDLink[0], 0, 0x3F, 0, new int[64], new byte[0xFC], new NSDSpawnPoint[1] { new NSDSpawnPoint(Entry.NullEID, 0, 0, 0, 0, 0) }, new byte[0]);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    case GameVersion.Crash3:
                        {
                            NewNSD nsd = data != null ? NewNSD.Load(data) : new NewNSD(new int[256], 0, new int[4], 0, 0, new int[64], new NSDLink[0], 0, 0x3F, 0, new int[128], new byte[0xFC], new NSDSpawnPoint[1] { new NSDSpawnPoint(Entry.NullEID, 0, 0, 0, 0, 0) }, new byte[0]);
                            PatchNSD(nsd, nsf, filename);
                        }
                        break;
                    default:
                        MessageBox.Show("NSD patching is not supported for this game version.", "Patch NSD", MessageBoxButtons.OK);
                        return;
                }
                nsfc.Node.TreeView.BeginUpdate();
                bool order_updated = false;
                foreach (TreeNode node in nsfc.Node.Nodes) // nsd patching might have moved entries, recreate moved entry chunks if that's the case
                {
                    if (node.Tag is EntryChunkController entrychunkcontroller)
                    {
                        int i = 0;
                        TreeNode[] nodes = new TreeNode[node.Nodes.Count];
                        foreach (TreeNode oldnode in node.Nodes)
                        {
                            nodes[i++] = oldnode;
                        }
                        for (i = 0; i < nodes.Length; ++i)
                        {
                            EntryController c = (EntryController)nodes[i].Tag;
                            if (c.Entry != entrychunkcontroller.EntryChunk.Entries[i])
                            {
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
                                order_updated = true;
                                break;
                            }
                        }
                    }
                }
                nsfc.Node.TreeView.EndUpdate();
                if (order_updated && MessageBox.Show("The chunk contents in this NSF may have been moved in accordance to the patched NSD and needs to be resaved. Continue?", "Patch NSD", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SaveNSF();
                }
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
            PatchNSDGoolMap(nsd.GOOLMap, nsf);

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
                                        if (Settings.Default.DeleteInvalidEntries) values.RemoveAll(eid => nsf.FindEID<IEntry>(eid) == null);
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
                                        if (Settings.Default.DeleteInvalidEntries) values.RemoveAll(eid => nsf.FindEID<IEntry>(eid) == null);
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
            PatchNSDGoolMap(nsd.GOOLMap, nsf);

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
                                        if (Settings.Default.DeleteInvalidEntries) values.RemoveAll(eid => nsf.FindEID<IEntry>(eid) == null);
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
                                        if (Settings.Default.DeleteInvalidEntries) values.RemoveAll(eid => nsf.FindEID<IEntry>(eid) == null);
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
            PatchNSDGoolMap(nsd.GOOLMap, nsf);
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

        public void PatchNSDGoolMap(int[] map, NSF nsf)
        {
            for (int i = 0; i < map.Length; ++i)
            {
                map[i] = Entry.NullEID;
            }
            foreach (Chunk chunk in nsf.Chunks)
            {
                if (chunk is EntryChunk entrychunk)
                {
                    foreach (Entry entry in entrychunk.Entries)
                    {
                        if (entry is GOOLEntry gool)
                        {
                            if (gool.Format == 1)
                            {
                                map[BitConv.FromInt32(gool.Header, 0)] = gool.EID;
                            }
                        }
                    }
                }
            }
        }
        
        public void CloseNSF()
        {
            string filename = tbcTabs.SelectedTab.Text;
            NSFBox nsfbox = (NSFBox)tbcTabs.SelectedTab.Tag;
            byte[] nsfdata;
            try
            {
                nsfdata = nsfbox.NSF.Save();
            }
            catch
            {
                nsfdata = null;
            }
            byte[] olddata = File.ReadAllBytes(filename);
            if (nsfdata == null || (nsfdata.Length == olddata.Length && nsfdata.SequenceEqual(olddata)) || MessageBox.Show("Are you sure you want to close the NSF file?", "Close Confirmation Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                if (ExternalTool.Invoke("drnsf", $"{imprintOpt} -- \"{dlgMakeBINFile.FileName}\"") != 0) {
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

        public void ResetConfig()
        {
            TabPage configtab = tbcTabs.TabPages[0];
            if (configtab.Tag is ConfigEditor)
            {
                configtab.Controls.Clear();
                configtab.Tag = new ConfigEditor() { Dock = DockStyle.Fill };
                configtab.Controls.Add((ConfigEditor)configtab.Tag);
            }
        }
    }
}
