using OpenTK.Mathematics;
using CrashEdit.Crash;
using CrashEdit.CE.Properties;

namespace CrashEdit.CE
{
    public sealed class ZoneEntryViewer : SceneryEntryViewer
    {
        private readonly OctreeRenderer octree_renderer;

        private readonly List<int> zones;
        private readonly int this_zone;

        private readonly Dictionary<int, GOOLEntry> gools = new();
        private bool is_master_zone;
        private byte zone_alpha;
        private Vector3 zone_trans;
        private bool time_trial_mode;

        private Rgba GetZoneColor(Color4 color)
        {
            return new Rgba(color, zone_alpha);
        }

        private Rgba GetZoneColor(byte r, byte g, byte b)
        {
            return new Rgba(r, g, b, zone_alpha);
        }

        public ZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
            octree_renderer = new(this);
        }

        public ZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
        {
            zones = zone_eids;
            this_zone = Entry.NullEID;
            octree_renderer = new(this);
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            octree_renderer?.UpdateForm();
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                foreach (int eid in zones)
                {
                    var zone = nsf.GetEntry<ZoneEntry>(eid);
                    var zonetrans = new Position(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
                    yield return zonetrans;
                    yield return new Position(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1 + zonetrans;
                    for (int i = 0; i < zone.CameraCount; i += 3)
                    {
                        Entity entity1 = zone.Entities[i];
                        foreach (EntityPosition position in entity1.Positions)
                        {
                            yield return new Position(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zonetrans;
                        }
                    }
                    for (int i = zone.CameraCount; i < zone.Entities.Count; ++i)
                    {
                        Entity entity = zone.Entities[i];
                        float scale = GameScales.ZoneEntityC1;
                        if (entity.Scaling.HasValue)
                        {
                            scale *= 4;
                            scale /= 1 << entity.Scaling.Value;
                        }
                        foreach (EntityPosition position in entity.Positions)
                        {
                            int x = entity.Type != 34 ? position.X : position.X + 50;
                            int y = entity.Type != 34 ? position.Y : position.Y + 50;
                            int z = entity.Type != 34 ? position.Z : position.Z + 50;
                            yield return new Position(x, y, z) / scale + zonetrans;
                        }
                    }
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += octree_renderer.PrintHelp();
            con_help += KeyboardControls.ToggleTimeTrial.Print(OnOffName(time_trial_mode));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            octree_renderer.RunLogic();
            if (KPress(KeyboardControls.ToggleTimeTrial)) time_trial_mode = !time_trial_mode;
        }

        private IList<ZoneEntry> GetZones()
        {
            var ret = new List<ZoneEntry>();
            foreach (int eid in zones)
            {
                var zone = nsf.GetEntry<ZoneEntry>(eid);
                if (zone != null)
                {
                    for (int i = 0; i < zone.ZoneCount; ++i)
                    {
                        var lzone = nsf.GetEntry<ZoneEntry>(zone.GetLinkedZone(i));
                        if (lzone != null && !ret.Contains(lzone))
                        {
                            ret.Add(lzone);
                        }
                    }
                }
            }
            return ret;
        }

        protected override void Render()
        {
            var allzones = GetZones();
            ZoneEntry master_zone = null;
            foreach (var zone in allzones)
            {
                if (zone.EID == this_zone)
                {
                    master_zone = zone;
                    break;
                }
            }

            List<int> worlds = new();
            foreach (var zone in allzones)
            {
                for (int i = 0; i < zone.WorldCount; ++i)
                {
                    var world = zone.GetLinkedWorld(i);
                    if (!worlds.Contains(world))
                        worlds.Add(world);
                }
            }
            SetWorlds(worlds);

            gools.Clear();
            foreach (var e in nsf.GetEntries<GOOLEntry>())
            {
                if (e.ParentGOOL == null)
                {
                    gools.Add(e.ID, e);
                }
            }

            is_master_zone = true;
            zone_alpha = 255;
            if (master_zone != null)
            {
                allzones.Remove(master_zone);
                RenderZone(master_zone);

                is_master_zone = false;
                zone_alpha = 128;
            }
            else
            {
                foreach (var zone in allzones)
                {
                    RenderZone(zone);
                }
                allzones.Clear();
            }

            // render early
            PostRender();

            base.Render();

            foreach (var zone in allzones)
            {
                RenderZone(zone);
            }
        }

        private void RenderZone(ZoneEntry zone)
        {
            zone_trans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddText3D(zone.EName, zone_trans + new Vector3(zoneSize.X, 0, zoneSize.Z) / 2, GetZoneColor(Color4.White), size: 2, flags: TextRenderFlags.Shadow | TextRenderFlags.Top | TextRenderFlags.Center);
            AddBox(zone_trans,
                   new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                   GetZoneColor(Color4.White),
                   true);
            for (int i = zone.CameraCount; i < zone.Entities.Count; ++i)
            {
                Entity entity = zone.Entities[i];
                RenderEntity(entity);
            }
            for (int i = 0; i < zone.CameraCount; i += 3)
            {
                Entity entity1 = zone.Entities[i];
                Entity entity2 = zone.Entities[i + 1];
                Entity entity3 = zone.Entities[i + 2];
                RenderCamera(entity1, entity2, entity3);
            }

            if (octree_renderer.enable && (is_master_zone || octree_renderer.show_neighbor_zones))
            {
                octree_renderer.alpha = zone_alpha;
                octree_renderer.RenderOctree(zone.Layout, 0x1C, zone_trans, zoneSize, zone.CollisionDepthX, zone.CollisionDepthY, zone.CollisionDepthZ);
            }
        }

        private void RenderEntity(Entity entity)
        {
            float text_y = Settings.Default.Font3DEnable ? 0 : float.MaxValue;
            bool draw_type = entity.Type.HasValue && entity.Subtype.HasValue;
            float scale = GameScales.ZoneEntityC1;
            if (entity.Scaling.HasValue)
            {
                scale *= 4;
                scale /= 1 << entity.Scaling.Value;
            }
            if (entity.Positions.Count > 0)
            {
                Vector3 trans = new Vector3(entity.Positions[0].X, entity.Positions[0].Y, entity.Positions[0].Z) / scale + zone_trans;
                if (!string.IsNullOrEmpty(entity.Name))
                {
                    AddText3D(entity.Name, trans, GetZoneColor(Color4.Yellow), ofs_y: text_y, flags: TextRenderFlags.Default | TextRenderFlags.Bottom);
                }

                if (entity.Positions.Count == 1)
                {
                    if (entity.Subtype.HasValue && entity.Type == 3)
                    {
                        draw_type = !RenderPickupEntity(trans + new Vector3(0, .5f, 0), entity.Subtype.Value);
                        if (entity.Subtype.Value == 25 && entity.Settings.Count > 0)
                        {
                            draw_type = false;
                            int gem_id = entity.Settings[0].ValueB;
                            text_y += AddText3D(gem_id.ToString(), trans, GetZoneColor(GetColorForGemId(gem_id)), ofs_y: text_y).Y;
                        }
                    }
                    else if (entity.Subtype.HasValue && entity.Type == 34)
                    {
                        draw_type = false;
                        int timetrialcontents = entity.TimeTrialReward.HasValue ? entity.TimeTrialReward.Value >> 8 : 0;
                        if (entity.Subtype.Value == 29)
                        {
                            float size_x = 1, size_y = 1, size_z = 1;
                            if (entity.Settings.Count > 2)
                            {
                                size_x = entity.Settings[0].Value / 4096f;
                                size_y = entity.Settings[1].Value / 4096f;
                                size_z = entity.Settings[2].Value / 4096f;
                                text_y += AddText3D($"{size_x} x {size_y} x {size_z}", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                            }
                            RenderBoxEntity(trans, entity.Subtype.Value, timetrialcontents, size_x, size_y, size_z);
                        }
                        else
                        {
                            RenderBoxEntity(trans, entity.Subtype.Value, timetrialcontents);
                            if (entity.Settings.Count > 0)
                            {
                                int pickup = entity.Settings[0].ValueB;
                                string pickup_name = $"unknown {pickup}";
                                if (pickup == 0) pickup_name = "";
                                else if (pickup == 100) pickup_name = "random";
                                else if (pickup == 101) pickup_name = "random-fruit";
                                else if (pickup >= 30 && pickup < 97) pickup_name = $"fruit {pickup}";
                                else if (pickup == 97) pickup_name = "1up";
                                else if (pickup == 102) pickup_name = "doctor";
                                else if (pickup >= 200 && pickup < 264) pickup_name = "crystal-" + (pickup - 200);
                                else if (pickup >= 300 && pickup < 364) pickup_name = "gem-" + (pickup - 300);
                                else if (pickup >= 400 && pickup < 464) pickup_name = "relic-1-" + (pickup - 400);
                                else if (pickup >= 500 && pickup < 564) pickup_name = "relic-2-" + (pickup - 500);
                                else if (pickup >= 600 && pickup < 664) pickup_name = "relic-3-" + (pickup - 600);
                                else if (pickup >= 700 && pickup < 764) pickup_name = "power-" + (pickup - 700);
                                text_y += AddText3D(pickup_name, trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                            }
                            if (entity.Settings.Count > 2)
                            {
                                int link_a = entity.Settings[2].ValueB;
                                int link_b = 0;
                                if (entity.Settings.Count > 3 && (entity.Subtype == 7 || entity.Subtype == 24))
                                {
                                    link_b = entity.Settings[3].ValueB;
                                }
                                for (int i = link_a; i <= (link_b == 0 ? link_a : link_b); ++i)
                                {
                                    var link_info = i == 0 ? null : nsf.GetEntityC2(i);
                                    if (link_info != null)
                                    {
                                        var link = link_info.Item1;
                                        var lzone = link_info.Item2;
                                        if (link.Positions.Count > 0)
                                        {
                                            var lzone_trans = new Vector3(lzone.X, lzone.Y, lzone.Z) / GameScales.ZoneC1;
                                            Vector3 link_trans = new Vector3(link.Positions[0].X, link.Positions[0].Y, link.Positions[0].Z) / scale + lzone_trans;
                                            vaoLinesThick.PushAttrib(trans: trans, rgba: GetZoneColor(Color4.Red));
                                            vaoLinesThick.PushAttrib(trans: link_trans, rgba: GetZoneColor(Color4.Lime));
                                        }
                                    }
                                }
                            }
                            if (entity.DDASettings.HasValue)
                                text_y += AddText3D($"dda {entity.DDASettings.Value >> 8}", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                            if (entity.DDASection.HasValue)
                                text_y += AddText3D($"dda-section {entity.DDASection.Value}", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                        }
                    }
                    else
                    {
                        AddSprite(trans, new Vector2(1), GetZoneColor(Color4.White), OldResources.PointTexture);
                    }
                }
                else
                {
                    for (int i = 1; i < entity.Positions.Count; ++i)
                    {
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i - 1].X, entity.Positions[i - 1].Y, entity.Positions[i - 1].Z) / scale + zone_trans,
                                            rgba: GetZoneColor(Color4.Blue));
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z) / scale + zone_trans,
                                            rgba: GetZoneColor(Color4.Blue));
                    }
                    foreach (EntityPosition position in entity.Positions)
                    {
                        var cur_trans = new Vector3(position.X, position.Y, position.Z) / scale + zone_trans;
                        AddSprite(cur_trans, new Vector2(1), GetZoneColor(Color4.Red), OldResources.PointTexture);
                    }
                }

                if (draw_type)
                {
                    if (gools.ContainsKey(entity.Type.Value))
                        text_y += AddText3D($"{gools[entity.Type.Value].EName}-{entity.Subtype.Value}", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                    else
                        text_y += AddText3D($"{entity.Type.Value}-{entity.Subtype.Value} (invalid type)", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                }
            }
        }

        private void RenderCamera(Entity entity1, Entity entity2, Entity entity3)
        {
            for (int i = 1; i < entity1.Positions.Count; ++i)
            {
                vaoLines.PushAttrib(trans: new Vector3(entity1.Positions[i - 1].X, entity1.Positions[i - 1].Y, entity1.Positions[i - 1].Z) / GameScales.ZoneCameraC1 + zone_trans,
                                    rgba: GetZoneColor(Color4.Green));
                vaoLines.PushAttrib(trans: new Vector3(entity1.Positions[i].X, entity1.Positions[i].Y, entity1.Positions[i].Z) / GameScales.ZoneCameraC1 + zone_trans,
                                    rgba: GetZoneColor(Color4.Green));
            }
            bool render_angles = entity2.Positions.Count == entity1.Positions.Count * 2;
            for (int i = 0; i < entity1.Positions.Count; ++i)
            {
                Vector3 trans = new Vector3(entity1.Positions[i].X, entity1.Positions[i].Y, entity1.Positions[i].Z) / GameScales.ZoneCameraC1 + zone_trans;
                AddSprite(trans, new Vector2(1), GetZoneColor(Color4.Yellow), OldResources.PointTexture);

                if (render_angles)
                {
                    const float ang2rad = MathHelper.Pi / 2048;
                    var quatAng1 = Quaternion.FromEulerAngles(-entity2.Positions[i * 2 + 0].X * ang2rad, -entity2.Positions[i * 2 + 0].Y * ang2rad, -entity2.Positions[i * 2 + 0].Z * ang2rad);
                    var rot_mat1 = Matrix4.CreateFromQuaternion(quatAng1);
                    var dir_vec1 = (rot_mat1 * new Vector4(0, 0, -1, 1)).Xyz;
                    var quatAng2 = Quaternion.FromEulerAngles(-entity2.Positions[i * 2 + 1].X * ang2rad, -entity2.Positions[i * 2 + 1].Y * ang2rad, -entity2.Positions[i * 2 + 1].Z * ang2rad);
                    var rot_mat2 = Matrix4.CreateFromQuaternion(quatAng2);
                    var dir_vec2 = (rot_mat2 * new Vector4(0, 0, -1, 1)).Xyz;

                    Rgba angColor1 = GetZoneColor(Color4.Olive);
                    Rgba angColor2 = GetZoneColor(Color4.DarkRed);
                    vaoLines.PushAttrib(trans: trans, rgba: angColor1);
                    vaoLines.PushAttrib(trans: trans + dir_vec1, rgba: angColor1);
                    AddSprite(trans + dir_vec1, new Vector2(0.5f), angColor1, OldResources.PointTexture);
                    vaoLines.PushAttrib(trans: trans, rgba: angColor2);
                    vaoLines.PushAttrib(trans: trans + dir_vec2, rgba: angColor2);
                    AddSprite(trans + dir_vec2, new Vector2(0.5f), angColor2, OldResources.PointTexture);
                }
            }
        }

        private Color4 GetColorForGemId(int id)
        {
            return id switch
            {
                0x3a => Color4.DarkRed,
                0x3b => Color4.Green,
                0x3c => Color4.Purple,
                0x3d => Color4.DarkBlue,
                0x3e => Color4.Gold,
                _ => Color4.LightGray,
            };
        }

        private bool RenderPickupEntity(Vector3 trans, int subtype)
        {
            var texture = GetPickupTexture(subtype);
            AddSprite(trans, GetPickupScale(subtype), GetZoneColor(Color4.White), texture);
            return texture != OldResources.UnknownPickupTexture;
        }

        private void RenderBoxEntity(Vector3 trans, int subtype, int timetrialcontents, float size_x = 1, float size_y = 1, float size_z = 1)
        {
            Rectangle sideTexRect = OldResources.TexMap[GetBoxSideTexture(subtype, timetrialcontents)];
            Span<Vector2> uvs_side = stackalloc Vector2[6] {
                new Vector2(sideTexRect.Left, sideTexRect.Bottom),
                new Vector2(sideTexRect.Left, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Bottom),
                new Vector2(sideTexRect.Left, sideTexRect.Bottom)
            };
            Span<Rgba> cols = stackalloc Rgba[6]
            {
                GetZoneColor(93*2, 93*2, 93*2),
                GetZoneColor(51*2, 51*2, 76*2),
                GetZoneColor(115*2, 115*2, 92*2),
                GetZoneColor(51*2, 51*2, 76*2),
                GetZoneColor(33*2, 33*2, 59*2),
                GetZoneColor(115*2, 115*2, 92*2)
            };
            Vector3 size = new Vector3(size_x, size_y, size_z) * 0.5f;
            for (int i = 0; i < 4 * 6; ++i)
            {
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * size + new Vector3(0, 0.5f, 0), rgba: cols[i / 6], st: uvs_side[i % 6]);
            }

            Rectangle topTexRect = OldResources.TexMap[GetBoxTopTexture(subtype, timetrialcontents)];
            Span<Vector2> uvs_top = stackalloc Vector2[6] {
                new Vector2(topTexRect.Left, topTexRect.Bottom),
                new Vector2(topTexRect.Left, topTexRect.Top),
                new Vector2(topTexRect.Right, topTexRect.Top),
                new Vector2(topTexRect.Right, topTexRect.Top),
                new Vector2(topTexRect.Right, topTexRect.Bottom),
                new Vector2(topTexRect.Left, topTexRect.Bottom)
            };
            for (int i = 4 * 6; i < 6 * 6; ++i)
            {
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * size + new Vector3(0, 0.5f, 0), rgba: cols[i / 6], st: uvs_top[i % 6]);
            }
        }

        private Bitmap GetBoxTopTexture(int subtype, int timetrialcontents)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    return OldResources.TNTBoxTopTexture;
                case 2: // Empty
                case 6: // Fruit
                case 8: // Life
                case 10: // Pickup
                case 11: // POW
                case 17: // Pickup AutoGrav
                case 20: // Empty AutoGrav
                case 25: // Slot
                    if (time_trial_mode && timetrialcontents >= 111 && timetrialcontents <= 113)
                        return OldResources.TimeBoxTopTexture;
                    else
                        return OldResources.EmptyBoxTexture;
                case 3: // Spring
                case 9: // Doctor
                    return OldResources.EmptyBoxTexture;
                case 4: // Continue
                    return time_trial_mode ? OldResources.EmptyBoxTexture : OldResources.ContinueBoxTexture;
                case 5: // Iron
                case 7: // Action
                case 15: // Iron Spring
                case 27: // Iron Continue
                case 28: // Clock
                    return OldResources.IronBoxTexture;
                case 18: // Nitro
                    return OldResources.NitroBoxTopTexture;
                case 23: // Steel
                    return OldResources.SteelBoxTexture;
                case 24: // Action Nitro
                    return OldResources.ActionNitroBoxTopTexture;
                case 26: // Time ?
                    return OldResources.TimeBoxTopTexture;
                default:
                    return OldResources.UnknownBoxTopTexture;
            }
        }

        private Bitmap GetBoxSideTexture(int subtype, int timetrialcontents)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    return OldResources.TNTBoxTexture;
                case 2: // Empty
                case 20: // Empty AutoGrav
                    return time_trial_mode ? LoadBoxSideTextureTimeTrial(timetrialcontents) : OldResources.EmptyBoxTexture;
                case 3: // Spring
                    return OldResources.SpringBoxTexture;
                case 4: // Continue
                    return time_trial_mode ? OldResources.EmptyBoxTexture : OldResources.ContinueBoxTexture;
                case 5: // Iron
                    return OldResources.IronBoxTexture;
                case 6: // Fruit
                    return time_trial_mode ? LoadBoxSideTextureTimeTrial(timetrialcontents) : OldResources.FruitBoxTexture;
                case 7: // Action
                    return OldResources.ActionBoxTexture;
                case 8: // Life
                    return time_trial_mode ? LoadBoxSideTextureTimeTrial(timetrialcontents) : OldResources.LifeBoxTexture;
                case 9: // Doctor
                    return OldResources.DoctorBoxTexture;
                case 10: // Pickup
                case 17: // Pickup AutoGrav
                    return time_trial_mode ? LoadBoxSideTextureTimeTrial(timetrialcontents) : OldResources.PickupBoxTexture;
                case 11: // POW
                    return OldResources.POWBoxTexture;
                case 13: // Ghost
                case 19: // Ghost Iron
                    return OldResources.UnknownBoxTopTexture;
                case 15: // Iron Spring
                    return OldResources.IronSpringBoxTexture;
                case 18: // Nitro
                    return OldResources.NitroBoxTexture;
                case 23: // Steel
                    return OldResources.SteelBoxTexture;
                case 24: // Action Nitro
                    return OldResources.ActionNitroBoxTexture;
                case 25: // Slot
                    return time_trial_mode ? LoadBoxSideTextureTimeTrial(timetrialcontents) : OldResources.SlotBoxTexture;
                case 27: // Iron Continue
                    return time_trial_mode ? OldResources.IronBoxTexture : OldResources.IronContinueBoxTexture;
                case 28: // Clock
                    return OldResources.ClockBoxTexture;
                default:
                    return OldResources.UnknownBoxTexture;
            }
        }

        private Bitmap LoadBoxSideTextureTimeTrial(int timetrialreward)
        {
            switch (timetrialreward)
            {
                case 102: // Doctor
                    return OldResources.DoctorBoxTexture;
                case 111: // Time 1
                    return OldResources.Time1BoxTexture;
                case 112: // Time 2
                    return OldResources.Time2BoxTexture;
                case 113: // Time 3
                    return OldResources.Time3BoxTexture;
                default: // Empty
                    return OldResources.EmptyBoxTexture;
            }
        }

        private Bitmap GetPickupTexture(int subtype)
        {
            switch (subtype)
            {
                case 5: // Life
                    return OldResources.LifeTexture;
                case 6: // Mask
                    return OldResources.MaskTexture;
                case 16: // Apple
                    return OldResources.AppleTexture;
                default:
                    return OldResources.UnknownPickupTexture;
            }
        }

        private Vector2 GetPickupScale(int subtype)
        {
            switch (subtype)
            {
                case 5: // Life
                case 6: // Mask
                    return new Vector2(1.8f, 1.125f);
                case 16: // Apple
                    return new Vector2(0.675f, 0.84375f);
                default:
                    return new Vector2(1);
            }
        }

        protected override void Dispose(bool disposing)
        {
            octree_renderer?.Dispose();

            base.Dispose(disposing);
        }
    }
}