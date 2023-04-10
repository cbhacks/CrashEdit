using Crash;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CrashEdit.Exporters;
using DiscUtils;
using OpenTK;

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
            }

            if (GameVersion == GameVersion.Crash1 || GameVersion == GameVersion.Crash1BetaMAR08 || GameVersion == GameVersion.Crash1BetaMAY11)
            {
                AddMenuSeparator();
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC1);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC1);
            }
            else if (GameVersion == GameVersion.Crash1Beta1995)
            {
                AddMenuSeparator();
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC1Proto);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC1Proto);
            }
            else if (GameVersion == GameVersion.Crash2 || GameVersion == GameVersion.Crash3)
            {
                AddMenuSeparator();
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevel, Menu_ShowLevelC2);
                AddMenu(Crash.UI.Properties.Resources.NSFController_AcShowLevelZones, Menu_ShowLevelZonesC2);
                AddMenuSeparator ();
                AddMenu (Crash.UI.Properties.Resources.NSFController_AcExportScenery, Menu_ExportSceneryOBJ);
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

        private void Menu_ExportSceneryOBJ ()
        {
            if (!FileUtil.SelectSaveFile (out string filename, FileFilters.OBJ, FileFilters.Any))
                return;
            
            ExportSceneryOBJ (Path.GetDirectoryName (filename), Path.GetFileNameWithoutExtension (filename));
        }
        
        private void ExportSceneryOBJ (string path, string modelname)
        {
            var exporter = new OBJExporter ();
            
            // detect how many textures are used and their eids to prepare the image
            Dictionary <int, int> textureEIDs = new ();
            Dictionary <string, TexInfoUnpacked> objTranslate = new Dictionary <string, TexInfoUnpacked> ();
            
            // find all the scenery insde chunks
            foreach (SceneryEntry scenery in NSF.GetEntries<SceneryEntry> ())
            {
                var offset = new Vector3 (scenery.XOffset, scenery.YOffset, scenery.ZOffset);
                var scale = new Vector3 (1 / GameScales.WorldC1);
                
                for (int i = 0; i < scenery.TPAGCount; i++)
                {
                    int tpag_eid = scenery.GetTPAG (i);

                    if (textureEIDs.ContainsKey (tpag_eid))
                        continue;

                    textureEIDs.Add (tpag_eid, textureEIDs.Count);
                }
                
                foreach (var tri in scenery.Triangles)
                {
                    if (tri.VertexA > scenery.Vertices.Count ||
                        tri.VertexB > scenery.Vertices.Count ||
                        tri.VertexC > scenery.Vertices.Count)
                        continue;
                    
                    var info = TextureUtils.ProcessTextureInfoC2 (0, tri.Texture, tri.Animated, scenery.Textures, scenery.AnimatedTextures);
                    string material = null;
                    Vector2? uv1 = null, uv2 = null, uv3 = null;

                    if (info.Item1 && info.Item2 is not null)
                    {
                        var value = info.Item2.Value;

                        material = objTranslate.FirstOrDefault (x =>
                            x.Value.color == value.ColorMode &&
                            x.Value.blend == value.BlendMode &&
                            x.Value.clutx == value.ClutX &&
                            x.Value.cluty == value.ClutY &&
                            x.Value.page == textureEIDs [scenery.GetTPAG (value.Page)]
                        ).Key;

                        // ignore the texinfo if there's already a texture with the exact same settings stored
                        if (material is null)
                        {
                            int textureEID = scenery.GetTPAG (value.Page);
                            
                            var texinfo = new TexInfoUnpacked (
                                true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                                page: textureEIDs [textureEID]
                            );

                            var tpag = this.NSF.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

                            Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                            // the material name changes
                            material = exporter.AddTexture (((int) texinfo).ToString ("X8"), texture);

                            // add it to the lookup table too
                            objTranslate [material] = texinfo;
                        }

                        Vector2 texsize = value.ColorMode switch
                        {
                            0 => new Vector2 (1024, 128),
                            1 => new Vector2 (512, 128),
                            _ => new Vector2 (256, 128)
                        };

                        uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                        uv2 = new Vector2 (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                        uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                    }

                    // add the face
                    SceneryVertex fv1 = scenery.Vertices [tri.VertexA];
                    SceneryVertex fv2 = scenery.Vertices [tri.VertexB];
                    SceneryVertex fv3 = scenery.Vertices [tri.VertexC];
                    SceneryColor fc1 = scenery.Colors [fv1.Color];
                    SceneryColor fc2 = scenery.Colors [fv2.Color];
                    SceneryColor fc3 = scenery.Colors [fv3.Color];
                    Vector3 v1 = new Vector3 (fv1.X, fv1.Y, fv1.Z);
                    Vector3 v2 = new Vector3 (fv2.X, fv2.Y, fv2.Z);
                    Vector3 v3 = new Vector3 (fv3.X, fv3.Y, fv3.Z);
                    Vector3 c1 = new Vector3 (fc1.Red, fc1.Green, fc1.Blue) / 255f;
                    Vector3 c2 = new Vector3 (fc2.Red, fc2.Green, fc2.Blue) / 255f;
                    Vector3 c3 = new Vector3 (fc3.Red, fc3.Green, fc3.Blue) / 255f;

                    exporter.AddFace (
                        (v1 + offset / 16) * scale,
                        (v2 + offset / 16) * scale,
                        (v3 + offset / 16) * scale,
                        c1, c2, c3,
                        material,
                        uv1, uv2, uv3
                    );
                }

                foreach (var quad in scenery.Quads)
                {
                    // ignore quads that are out of limits
                    if (quad.VertexA > scenery.Vertices.Count ||
                        quad.VertexB > scenery.Vertices.Count ||
                        quad.VertexC > scenery.Vertices.Count ||
                        quad.VertexD > scenery.Vertices.Count)
                        continue;
                    
                    var info = TextureUtils.ProcessTextureInfoC2 (0, quad.Texture, quad.Animated, scenery.Textures, scenery.AnimatedTextures);
                    string material = null;
                    Vector2? uv1 = null, uv2 = null, uv3 = null, uv4 = null;

                    if (info.Item1 && info.Item2 is not null)
                    {
                        var value = info.Item2.Value;
                        int textureEID = scenery.GetTPAG (value.Page);

                        material = objTranslate.FirstOrDefault (x =>
                            x.Value.color == value.ColorMode &&
                            x.Value.blend == value.BlendMode &&
                            x.Value.clutx == value.ClutX &&
                            x.Value.cluty == value.ClutY &&
                            x.Value.page == textureEIDs [textureEID]
                        ).Key;

                        // ignore the texinfo if there's already a texture with the exact same settings stored
                        if (material is null)
                        {
                            var texinfo = new TexInfoUnpacked (
                                true, color: value.ColorMode, blend: value.BlendMode, clutx: value.ClutX, cluty: value.ClutY,
                                page: textureEIDs [textureEID]
                            );

                            var tpag = this.NSF.GetEntry <TextureChunk> (textureEIDs.First (x => x.Key == textureEID).Key);

                            Bitmap texture = TextureExporter.CreateTexture (tpag.Data, texinfo);

                            // the material name changes
                            material = exporter.AddTexture (((int) texinfo).ToString ("X8"), texture);

                            // add it to the lookup table too
                            objTranslate [material] = texinfo;
                        }

                        Vector2 texsize = value.ColorMode switch
                        {
                            0 => new Vector2 (1024, 128),
                            1 => new Vector2 (512, 128),
                            _ => new Vector2 (256, 128)
                        };

                        uv1 = new (value.X2 / texsize.X, (128 - value.Y2) / texsize.Y);
                        uv2 = new (value.X1 / texsize.X, (128 - value.Y1) / texsize.Y);
                        uv3 = new (value.X3 / texsize.X, (128 - value.Y3) / texsize.Y);
                        uv4 = new (value.X4 / texsize.X, (128 - value.Y4) / texsize.Y);
                    }

                    // add the face
                    SceneryVertex fv1 = scenery.Vertices [quad.VertexA];
                    SceneryVertex fv2 = scenery.Vertices [quad.VertexB];
                    SceneryVertex fv3 = scenery.Vertices [quad.VertexC];
                    SceneryVertex fv4 = scenery.Vertices [quad.VertexD];
                    SceneryColor fc1 = scenery.Colors [fv1.Color];
                    SceneryColor fc2 = scenery.Colors [fv2.Color];
                    SceneryColor fc3 = scenery.Colors [fv3.Color];
                    SceneryColor fc4 = scenery.Colors [fv4.Color];
                    Vector3 v1 = new Vector3 (fv1.X, fv1.Y, fv1.Z);
                    Vector3 v2 = new Vector3 (fv2.X, fv2.Y, fv2.Z);
                    Vector3 v3 = new Vector3 (fv3.X, fv3.Y, fv3.Z);
                    Vector3 v4 = new Vector3 (fv4.X, fv4.Y, fv4.Z);
                    Vector3 c1 = new Vector3 (fc1.Red, fc1.Green, fc1.Blue) / 255f;
                    Vector3 c2 = new Vector3 (fc2.Red, fc2.Green, fc2.Blue) / 255f;
                    Vector3 c3 = new Vector3 (fc3.Red, fc3.Green, fc3.Blue) / 255f;
                    Vector3 c4 = new Vector3 (fc4.Red, fc4.Green, fc4.Blue) / 255f;

                    exporter.AddFace (
                        (v1 + offset / 16) * scale,
                        (v2 + offset / 16) * scale,
                        (v3 + offset / 16) * scale,
                        (v4 + offset / 16) * scale,
                        c1, c2, c3, c4,
                        material,
                        uv1, uv2, uv3, uv4
                    );
                }
            }
            
            exporter.Export (path, modelname);
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
