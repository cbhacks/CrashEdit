using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class NSFController : Controller
    {
        public NSFController(NSF nsf, GameVersion gameversion)
        {
            NSF = nsf;
            GameVersion = gameversion;
            foreach (Chunk chunk in nsf.Chunks)
            {
                AddNode(CreateChunkController(chunk));
            }
            AddMenu(Crash.UI.Properties.Resources.NSFController_AcAddNormalChunk, Menu_Add_NormalChunk);
            if (GameVersion != GameVersion.Crash2 && GameVersion != GameVersion.Crash3 && GameVersion != GameVersion.Crash1)
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcAddOldSoundChunk, Menu_Add_OldSoundChunk);
            AddMenu(Crash.UI.Properties.Resources.NSFController_AcAddSoundChunk, Menu_Add_SoundChunk);
            AddMenu(Crash.UI.Properties.Resources.NSFController_AcAddWavebankChunk, Menu_Add_WavebankChunk);
            AddMenu(Crash.UI.Properties.Resources.NSFController_AcAddSpeechChunk, Menu_Add_SpeechChunk);
            AddMenu(Crash.UI.Properties.Resources.NSFController_AcImportChunk, Menu_Import_Chunk);
            if (GameVersion == GameVersion.Crash2 || GameVersion == GameVersion.Crash3)
            {
                AddMenuSeparator();
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcFixDetonator, Menu_Fix_Detonator);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcFixBoxCount, Menu_Fix_BoxCount);
                AddMenuSeparator();
            }

            if (GameVersion == GameVersion.Crash1 || GameVersion == GameVersion.Crash1BetaMAR08 || GameVersion == GameVersion.Crash1BetaMAY11)
            {
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC1);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC1);
            }
            else if (GameVersion == GameVersion.Crash1Beta1995)
            {
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC1Proto);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC1Proto);
            }
            else if (GameVersion == GameVersion.Crash2 || GameVersion == GameVersion.Crash3)
            {
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC2);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC2);
            }
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = Crash.UI.Properties.Resources.NSFController_Text;
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "nsf";
            Node.SelectedImageKey = "nsf";
        }

        public NSF NSF { get; }
        public GameVersion GameVersion { get; }

        private Form ShowLevelForm { get; set; }
        private Form ShowLevelZonesForm { get; set; }

        public ChunkController CreateChunkController(Chunk chunk)
        {
            if (chunk is NormalChunk)
            {
                return new NormalChunkController(this, (NormalChunk)chunk);
            }
            else if (chunk is TextureChunk)
            {
                return new TextureChunkController(this, (TextureChunk)chunk);
            }
            else if (chunk is OldSoundChunk)
            {
                return new OldSoundChunkController(this, (OldSoundChunk)chunk);
            }
            else if (chunk is SoundChunk)
            {
                return new SoundChunkController(this, (SoundChunk)chunk);
            }
            else if (chunk is WavebankChunk)
            {
                return new WavebankChunkController(this, (WavebankChunk)chunk);
            }
            else if (chunk is SpeechChunk)
            {
                return new SpeechChunkController(this, (SpeechChunk)chunk);
            }
            else if (chunk is UnprocessedChunk)
            {
                return new UnprocessedChunkController(this, (UnprocessedChunk)chunk);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Menu_Add_NormalChunk()
        {
            NormalChunk chunk = new NormalChunk(NSF);
            NSF.Chunks.Add(chunk);
            NormalChunkController controller = new NormalChunkController(this, chunk);
            AddNode(controller);
        }

        private void Menu_Add_OldSoundChunk()
        {
            OldSoundChunk chunk = new OldSoundChunk(NSF);
            NSF.Chunks.Add(chunk);
            OldSoundChunkController controller = new OldSoundChunkController(this, chunk);
            AddNode(controller);
        }

        private void Menu_Add_SoundChunk()
        {
            SoundChunk chunk = new SoundChunk(NSF);
            NSF.Chunks.Add(chunk);
            SoundChunkController controller = new SoundChunkController(this, chunk);
            AddNode(controller);
        }

        private void Menu_Add_WavebankChunk()
        {
            WavebankChunk chunk = new WavebankChunk(NSF);
            NSF.Chunks.Add(chunk);
            WavebankChunkController controller = new WavebankChunkController(this, chunk);
            AddNode(controller);
        }

        private void Menu_Add_SpeechChunk()
        {
            SpeechChunk chunk = new SpeechChunk(NSF);
            NSF.Chunks.Add(chunk);
            SpeechChunkController controller = new SpeechChunkController(this, chunk);
            AddNode(controller);
        }

        private void Menu_Fix_Detonator()
        {
            List<Entity> nitros = new List<Entity>();
            List<Entity> detonators = new List<Entity>();
            foreach (ZoneEntry entry in NSF.GetEntries<ZoneEntry>())
            {
                foreach (Entity entity in entry.Entities)
                {
                    if (entity.Type == 34)
                    {
                        if (entity.Subtype == 18 && entity.ID.HasValue)
                        {
                            nitros.Add(entity);
                        }
                        else if (entity.Subtype == 24)
                        {
                            detonators.Add(entity);
                        }
                    }
                }
            }
            foreach (Entity detonator in detonators)
            {
                detonator.Victims.Clear();
                foreach (Entity nitro in nitros)
                {
                    detonator.Victims.Add(new EntityVictim((short)nitro.ID.Value));
                }
            }
        }

        private void Menu_Fix_BoxCount()
        {
            int boxcount = 0;
            List<Entity> willys = new List<Entity>();
            foreach (ZoneEntry zone in NSF.GetEntries<ZoneEntry>())
            {
                foreach (Entity entity in zone.Entities)
                {
                    if (entity.Type == 0 && entity.Subtype == 0)
                    {
                        willys.Add(entity);
                    }
                    else if (entity.Type == 34)
                    {
                        switch (entity.Subtype)
                        {
                            case 0: // tnt
                            case 2: // empty
                            case 3: // spring
                            case 4: // continue
                            case 6: // fruit
                            case 8: // life
                            case 9: // doctor
                            case 10: // pickup
                            case 11: // pow
                            case 13: // ghost
                            case 17: // auto pickup
                            case 18: // nitro
                            case 20: // auto empty
                            case 21: // empty 2
                            case 25: // slot
                                boxcount++;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            foreach (Entity willy in willys)
            {
                if (willy.BoxCount.HasValue)
                {
                    willy.BoxCount = new EntitySetting(0, boxcount);
                }
            }
        }

        private void Menu_ShowLevelC1()
        {
            if (ShowLevelForm != null)
            {
                ShowLevelForm.Focus();
                return;
            }
            ShowLevelForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelForm.Show();
            List<int> worlds = new();
            foreach (var entry in NSF.GetEntries<OldSceneryEntry>())
            {
                worlds.Add(entry.EID);
            }
            OldSceneryEntryViewer viewer = new(NSF, worlds) { Dock = DockStyle.Fill };
            ShowLevelForm.Controls.Add(viewer);
            ShowLevelForm.Text = string.Empty;
            ShowLevelForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelForm = null;
            };
        }

        private void Menu_ShowLevelZonesC1()
        {
            if (ShowLevelZonesForm != null)
            {
                ShowLevelZonesForm.Focus();
                return;
            }
            ShowLevelZonesForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelZonesForm.Show();
            List<int> zones = new();
            foreach (var entry in NSF.GetEntries<OldZoneEntry>())
            {
                zones.Add(entry.EID);
            }
            OldZoneEntryViewer viewer = new(NSF, zones) { Dock = DockStyle.Fill };
            ShowLevelZonesForm.Controls.Add(viewer);
            ShowLevelZonesForm.Text = string.Empty;
            ShowLevelZonesForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelZonesForm = null;
            };
        }

        private void Menu_ShowLevelC1Proto()
        {
            if (ShowLevelForm != null)
            {
                ShowLevelForm.Focus();
                return;
            }
            ShowLevelForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelForm.Show();
            List<int> worlds = new();
            foreach (var entry in NSF.GetEntries<ProtoSceneryEntry>())
            {
                worlds.Add(entry.EID);
            }
            ProtoSceneryEntryViewer viewer = new(NSF, worlds) { Dock = DockStyle.Fill };
            ShowLevelForm.Controls.Add(viewer);
            ShowLevelForm.Text = string.Empty;
            ShowLevelForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelForm = null;
            };
        }

        private void Menu_ShowLevelZonesC1Proto()
        {
            if (ShowLevelZonesForm != null)
            {
                ShowLevelZonesForm.Focus();
                return;
            }
            ShowLevelZonesForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelZonesForm.Show();
            List<int> zones = new();
            foreach (var entry in NSF.GetEntries<ProtoZoneEntry>())
            {
                zones.Add(entry.EID);
            }
            ProtoZoneEntryViewer viewer = new(NSF, zones) { Dock = DockStyle.Fill };
            ShowLevelZonesForm.Controls.Add(viewer);
            ShowLevelZonesForm.Text = string.Empty;
            ShowLevelZonesForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelZonesForm = null;
            };
        }

        private void Menu_ShowLevelC2()
        {
            if (ShowLevelForm != null)
            {
                ShowLevelForm.Focus();
                return;
            }
            ShowLevelForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelForm.Show();
            List<int> worlds = new();
            foreach (var entry in NSF.GetEntries<SceneryEntry>())
            {
                worlds.Add(entry.EID);
            }
            SceneryEntryViewer viewer = new(NSF, worlds) { Dock = DockStyle.Fill };
            ShowLevelForm.Controls.Add(viewer);
            ShowLevelForm.Text = string.Empty;
            ShowLevelForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelForm = null;
            };
        }

        private void Menu_ShowLevelZonesC2()
        {
            if (ShowLevelZonesForm != null)
            {
                ShowLevelZonesForm.Focus();
                return;
            }
            ShowLevelZonesForm = new() { Text = "Loading...", Width = 480, Height = 360 };
            ShowLevelZonesForm.Show();
            List<int> zones = new();
            foreach (var entry in NSF.GetEntries<ZoneEntry>())
            {
                zones.Add(entry.EID);
            }
            ZoneEntryViewer viewer = new(NSF, zones) { Dock = DockStyle.Fill };
            ShowLevelZonesForm.Controls.Add(viewer);
            ShowLevelZonesForm.Text = string.Empty;
            ShowLevelZonesForm.FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                ShowLevelZonesForm = null;
            };
        }

        private void Menu_Import_Chunk()
        {
            byte[][] datas = FileUtil.OpenFiles(FileFilters.Any);
            if (datas == null)
                return;
            bool process = MessageBox.Show("Do you want to process the imported chunks?", "Import Chunk", MessageBoxButtons.YesNo) == DialogResult.Yes;
            foreach (var data in datas)
            {
                try
                {
                    UnprocessedChunk chunk = Chunk.Load(data, NSF);
                    if (process)
                    {
                        Chunk processedchunk = chunk.Process(NSF.Chunks.Count * 2 + 1);
                        NSF.Chunks.Add(processedchunk);
                        AddNode(CreateChunkController(processedchunk));
                    }
                    else
                    {
                        NSF.Chunks.Add(chunk);
                        AddNode(new UnprocessedChunkController(this, chunk));
                    }
                }
                catch (LoadAbortedException)
                {
                }
            }
        }

        public override void Dispose()
        {
            ShowLevelForm?.Close();
            ShowLevelZonesForm?.Close();

            base.Dispose();
        }
    }
}
