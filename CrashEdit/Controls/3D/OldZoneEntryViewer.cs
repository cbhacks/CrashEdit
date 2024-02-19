using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private readonly OctreeRenderer octree_renderer;

        private readonly List<int> zones;
        private readonly int this_zone;

        private readonly Dictionary<int, GOOLEntry> gools = new();
        private bool is_master_zone;
        private byte zone_alpha;
        private Vector3 zone_trans;

        private OldCamera anchor_cam;
        private float anchor_cam_pos;
        private int anchor_next_cam;
        private int anchor_prev_cam;
        private int anchor_zone;
        private bool anchor_sortlist = true;

        private Rgba GetZoneColor(Color4 color)
        {
            return new Rgba(color, zone_alpha);
        }

        private Rgba GetZoneColor(byte r, byte g, byte b)
        {
            return new Rgba(r, g, b, zone_alpha);
        }

        public OldZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
            octree_renderer = new(this);
        }

        public OldZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
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
                    var zone = nsf.GetEntry<OldZoneEntry>(eid);
                    var zonetrans = new Position(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
                    yield return zonetrans;
                    yield return new Position(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1 + zonetrans;
                    foreach (OldEntity entity in zone.Entities)
                    {
                        foreach (EntityPosition position in entity.Positions)
                        {
                            int x = entity.Type != 34 ? position.X : position.X + 50;
                            int y = entity.Type != 34 ? position.Y : position.Y + 50;
                            int z = entity.Type != 34 ? position.Z : position.Z + 50;
                            yield return new Position(x, y, z) / GameScales.ZoneEntityC1 + zonetrans;
                        }
                    }
                    foreach (OldCamera camera in zone.Cameras)
                    {
                        foreach (OldCameraPosition position in camera.Positions)
                        {
                            int x = position.X;
                            int y = position.Y;
                            int z = position.Z;
                            yield return new Position(x, y, z) / GameScales.ZoneCameraC1 + zonetrans;
                        }
                    }
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            if (anchormode)
            {
                con_help += Resources.ViewerControls_ZoneAnchorMove + '\n';
                con_help += KeyboardControls.ExitZoneAnchor.Print();
                con_help += KeyboardControls.ZoneAnchorNextCam.Print();
                con_help += KeyboardControls.ZoneAnchorPrevCam.Print();
                con_help += KeyboardControls.ZoneAnchorSortList.Print(OnOffName(anchor_sortlist));
            }
            else
            {
                con_help += KeyboardControls.EnterZoneAnchor.Print();
            }
            con_help += octree_renderer.PrintHelp();
        }

        private OldCamera GetNeighborCamera(OldCamera cam, int neighbor_index, out int neighbor_zone_eid)
        {
            neighbor_zone_eid = Entry.NullEID;
            if (neighbor_index >= cam.NeighborCount || neighbor_index < 0)
                return null;

            // TODO camera should have reference to its zone
            var neighbor = cam.Neighbors[neighbor_index];
            var zone = nsf.GetEntry<OldZoneEntry>(anchor_zone);
            var neighbor_zone = nsf.GetEntry<OldZoneEntry>(zone.GetLinkedZone(neighbor.ZoneIndex));
            if (neighbor_zone == null || neighbor.CameraIndex >= neighbor_zone.Cameras.Count)
                return null;

            neighbor_zone_eid = neighbor_zone.EID;
            return neighbor_zone.Cameras[neighbor.CameraIndex];
        }

        private int GetNeighborIndexForCamera(OldCamera cam, OldCamera want_cam)
        {
            for (var i = 0; i < cam.NeighborCount; ++i)
            {
                if (GetNeighborCamera(cam, i, out int n) == want_cam)
                    return i;
            }
            return -1;
        }

        private void SetBestNeighborCams(bool prev, bool next)
        {
            if (prev)
                anchor_prev_cam = -1;
            if (next)
                anchor_next_cam = -1;
            for (var i = 0; i < anchor_cam.NeighborCount; ++i)
            {
                var neighbor = anchor_cam.Neighbors[i];
                if (neighbor.LinkType == 0x1 && prev)
                    anchor_prev_cam = i;
                if (neighbor.LinkType == 0x2 && next)
                    anchor_next_cam = i;
            }
        }

        private bool EnterAnchorMode()
        {
            var master = GetMasterZone();
            if (master != null && master.Cameras.Count > 0)
            {
                anchor_cam = null;
                foreach (var cam in master.Cameras)
                {
                    if (cam.Positions.Count == 0)
                        continue;
                    if (anchor_cam == null || (cam.Mode == 5 && anchor_cam.Mode != 5))
                    {
                        anchor_cam = cam;
                    }
                }
                if (anchor_cam != null)
                {
                    anchor_zone = master.EID;
                    SetBestNeighborCams(true, true);
                    anchor_cam_pos = 0;
                    anchormode = true;
                }
            }
            return anchormode;
        }

        private void ExitAnchorMode()
        {
            anchormode = false;
            anchor_cam = null;
        }

        private bool CheckAnchorMode()
        {
            if (anchormode)
            {
                var zone = nsf.GetEntry<OldZoneEntry>(anchor_zone);
                if (zone == null || !zone.Cameras.Contains(anchor_cam) || anchor_cam.Positions.Count == 0)
                {
                    ExitAnchorMode();
                }
            }
            return anchormode;
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            octree_renderer.RunLogic();
            if (!anchormode)
            {
                if (this_zone != Entry.NullEID && KPress(KeyboardControls.EnterZoneAnchor))
                {
                    EnterAnchorMode();
                }
            }
            else if (CheckAnchorMode())
            {
                var zone = nsf.GetEntry<OldZoneEntry>(anchor_zone);
                int anchor_cam_index = zone.Cameras.IndexOf(anchor_cam);
                if (KPress(KeyboardControls.ZoneAnchorPrevCam) && anchor_cam_index > 0)
                {
                    anchor_cam = zone.Cameras[anchor_cam_index - 1];
                    anchor_cam_pos = 0;
                    SetBestNeighborCams(true, true);
                }
                if (KPress(KeyboardControls.ZoneAnchorNextCam) && anchor_cam_index < zone.Cameras.Count - 1)
                {
                    anchor_cam = zone.Cameras[anchor_cam_index + 1];
                    anchor_cam_pos = 0;
                    SetBestNeighborCams(true, true);
                }

                int move_x, move_y, move_z;
                if (KDown(Keys.D))
                    move_x = 1;
                else if (KDown(Keys.A))
                    move_x = -1;
                else
                    move_x = 0;
                if (KDown(Keys.S))
                    move_z = 1;
                else if (KDown(Keys.W))
                    move_z = -1;
                else
                    move_z = 0;
                if (KDown(Keys.Q))
                    move_y = 1;
                else if (KDown(Keys.E))
                    move_y = -1;
                else
                    move_y = 0;

                float move_speed = (anchor_cam.Mode == 1 || anchor_cam.Mode == 3 ? 30 : GameScales.ZoneCameraC1 * 6 / anchor_cam.AvgDist) * PerFrame;
                anchor_cam_pos += anchor_cam.XDir / 4096F * move_x * move_speed;
                anchor_cam_pos += anchor_cam.YDir / 4096F * move_y * move_speed;
                anchor_cam_pos += anchor_cam.ZDir / 4096F * move_z * move_speed;
                
                if (anchor_cam_pos > anchor_cam.Positions.Count - 1)
                {
                    if (anchor_next_cam != -1)
                    {
                        var newcam = GetNeighborCamera(anchor_cam, anchor_next_cam, out int newzone);
                        if (newcam != null)
                        {
                            anchor_cam_pos -= anchor_cam.Positions.Count - 1;

                            anchor_prev_cam = GetNeighborIndexForCamera(newcam, anchor_cam);
                            anchor_cam = newcam;
                            anchor_zone = newzone;
                            SetBestNeighborCams(anchor_prev_cam == -1, true);
                        }
                    }
                }
                else if (anchor_cam_pos < 0)
                {
                    if (anchor_prev_cam != -1)
                    {
                        var newcam = GetNeighborCamera(anchor_cam, anchor_prev_cam, out int newzone);
                        if (newcam != null)
                        {
                            // note that the position index will be negative here
                            anchor_cam_pos += newcam.Positions.Count - 1;

                            anchor_next_cam = GetNeighborIndexForCamera(newcam, anchor_cam);
                            anchor_cam = newcam;
                            anchor_zone = newzone;
                            SetBestNeighborCams(true, anchor_next_cam == -1);
                        }
                    }
                }

                // always make sure position is clamped
                anchor_cam_pos = Math.Max(0, Math.Min(anchor_cam_pos, anchor_cam.Positions.Count - 1));

                if (KPress(KeyboardControls.ZoneAnchorSortList)) anchor_sortlist = !anchor_sortlist;

                if (KPress(KeyboardControls.ExitZoneAnchor))
                {
                    ExitAnchorMode();
                }
            }
        }

        private IList<OldZoneEntry> GetZones()
        {
            var ret = new List<OldZoneEntry>();
            foreach (int eid in zones)
            {
                var zone = nsf.GetEntry<OldZoneEntry>(eid);
                if (zone != null)
                {
                    for (int i = 0; i < zone.ZoneCount; ++i)
                    {
                        var lzone = nsf.GetEntry<OldZoneEntry>(zone.GetLinkedZone(i));
                        if (lzone != null && !ret.Contains(lzone))
                        {
                            ret.Add(lzone);
                        }
                    }
                }
            }
            return ret;
        }

        private OldZoneEntry GetMasterZone()
        {
            if (!zones.Contains(this_zone))
                return null;
            else
                return nsf.GetEntry<OldZoneEntry>(this_zone);
        }

        protected override void Render()
        {
            var allzones = GetZones();
            OldZoneEntry master_zone = GetMasterZone();

            SetSortList(null);
            if (CheckAnchorMode())
            {
                master_zone = nsf.GetEntry<OldZoneEntry>(anchor_zone);
                allzones.Clear();
                for (int i = 0; i < master_zone.ZoneCount; ++i)
                {
                    var lzone = nsf.GetEntry<OldZoneEntry>(master_zone.GetLinkedZone(i));
                    if (lzone != null && !allzones.Contains(lzone))
                    {
                        allzones.Add(lzone);
                    }
                }

                con_debug += $"prev-cam: {anchor_prev_cam}\n";
                con_debug += $"next-cam: {anchor_next_cam}\n";
                var pos1 = anchor_cam.Positions[(int)Math.Floor(anchor_cam_pos)];
                var pos2 = anchor_cam.Positions[(int)Math.Ceiling(anchor_cam_pos)];
                float lerp = anchor_cam_pos.TruncatePart();
                Vector3 master_zone_trans = new Vector3(master_zone.X, master_zone.Y, master_zone.Z) / GameScales.ZoneC1;
                Vector3 trans = MathExt.Lerp(new Vector3(pos1.X, pos1.Y, pos1.Z), new Vector3(pos2.X, pos2.Y, pos2.Z), lerp) / GameScales.ZoneCameraC1 + master_zone_trans;
                Vector3 rot = MathExt.Lerp(new Vector3(-pos1.XRot, -pos1.YRot, -pos1.ZRot), new Vector3(-pos2.XRot, -pos2.YRot, -pos2.ZRot), lerp) / 0x800 * MathHelper.Pi;
                ForceViewTransRot(trans, rot);
                // TODO method
                float yratio = DefaultZNear * (float)Math.Tan(0.5f * MathHelper.DegreesToRadians(64)) / (nsf.GetScreenOffset() / 256f);
                float left = -yratio * ProjectionInfo.Aspect8x5;
                float right = yratio * ProjectionInfo.Aspect8x5;
                yratio *= 256f / 240f;
                render.Projection.Perspective = Matrix4.CreatePerspectiveOffCenter(left, right, -yratio, yratio, DefaultZNear, DefaultZFar);

                if (anchor_sortlist)
                {
                    var slst = nsf.GetEntry<OldSLSTEntry>(anchor_cam.SLSTEID);
                    if (slst != null)
                    {
                        var polys = slst.DecodeAt((int)Math.Floor(anchor_cam_pos));
                        con_debug += string.Format("wgeo polys: {0}/1460({1:F2}%)/2610({2:F2}%)\n", polys.Count, 100f*polys.Count/1460, 100f*polys.Count/2610);
                        SetSortList(polys);
                    }
                }
            }

            List<int> worlds = new();
            foreach (var zone in allzones)
            {
                for (int i = 0; i < zone.WorldCount; ++i)
                {
                    var world = zone.GetLinkedWorld(i);
                    if (!worlds.Contains(world) || (anchormode && anchor_sortlist))
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

            base.Render();

            is_master_zone = true;
            zone_alpha = 255;
            if (master_zone != null)
            {
                allzones.Remove(master_zone);
                RenderZone(master_zone);

                is_master_zone = false;
                zone_alpha = 128;
            }
            foreach (var zone in allzones)
            {
                RenderZone(zone);
            }
        }

        private void RenderZone(OldZoneEntry zone)
        {
            zone_trans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddText3D(zone.EName, zone_trans + new Vector3(zoneSize.X, 0, zoneSize.Z) / 2, GetZoneColor(Color4.White), size: 2, flags: TextRenderFlags.Shadow | TextRenderFlags.Top | TextRenderFlags.Center);
            AddBox(zone_trans, new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1, GetZoneColor(Color4.White), true);
            foreach (OldEntity entity in zone.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in zone.Cameras)
            {
                if (anchormode && camera == anchor_cam)
                    continue;
                RenderCamera(camera);
            }

            if (octree_renderer.enable && (is_master_zone || octree_renderer.show_neighbor_zones))
            {
                octree_renderer.alpha = zone_alpha;
                octree_renderer.RenderOctree(zone.Layout, 0x1C, zone_trans, zoneSize, zone.CollisionDepthX, zone.CollisionDepthY, zone.CollisionDepthZ);
            }
        }

        private void RenderEntity(OldEntity entity)
        {
            float text_y = Settings.Default.Font3DEnable ? 0 : float.MaxValue;
            bool draw_type = true;
            Vector3 trans = new Vector3(entity.Positions[0].X, entity.Positions[0].Y, entity.Positions[0].Z) / GameScales.ZoneEntityC1 + zone_trans;
            if (entity.Positions.Count > 0)
            {
                AddText3D("entity-" + entity.ID, trans, GetZoneColor(Color4.Yellow), ofs_y: text_y, flags: TextRenderFlags.Default | TextRenderFlags.Bottom);
                if (entity.Positions.Count == 1)
                {
                    if (entity.Type == 3)
                    {
                        draw_type = !RenderPickupEntity(trans + new Vector3(0, .5f, 0), entity.Subtype);
                    }
                    else if (entity.Type == 34)
                    {
                        RenderBoxEntity(trans, entity.Subtype);
                        draw_type = false;
                        int pickup = entity.VecX;
                        string pickup_name = $"unknown {pickup}";
                        if (pickup == 0) pickup_name = "";
                        else if (pickup == 100) pickup_name = "random";
                        else if (pickup == 101) pickup_name = "random-fruit";
                        else if (pickup >= 30 && pickup < 97) pickup_name = $"fruit {pickup}";
                        else if (pickup == 97) pickup_name = "1up";
                        else if (pickup == 102) pickup_name = "doctor";
                        else if (pickup == 103) pickup_name = "cortex";
                        else if (pickup == 104) pickup_name = "brio";
                        else if (pickup == 105) pickup_name = "tawna";
                        text_y += AddText3D(pickup_name, trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                        int link_a = entity.VecZ;
                        var link_info = link_a == 0 ? null : nsf.GetEntityC1(link_a);
                        if (link_info != null)
                        {
                            var link = link_info.Item1;
                            var lzone = link_info.Item2;
                            if (link.Positions.Count > 0)
                            {
                                float wave_pad = 0.15f;
                                float wave_ramp = 0.3f;
                                float wave_sustain = 0.15f;
                                float cycle_size = 1 + wave_pad * 2 + wave_ramp * 2 + wave_sustain;
                                float cycle = (float)(render.CurrentTime % 4) / 4 * cycle_size;
                                float lerp_ramp_end = cycle - wave_pad;
                                float lerp_sustain_end = lerp_ramp_end - wave_ramp;
                                float lerp_sustain_start = lerp_sustain_end - wave_sustain;
                                float lerp_ramp_start = lerp_sustain_start - wave_ramp;

                                var lzone_trans = new Vector3(lzone.X, lzone.Y, lzone.Z) / GameScales.ZoneC1;
                                Vector3 link_trans = new Vector3(link.Positions[0].X, link.Positions[0].Y, link.Positions[0].Z) / GameScales.ZoneEntityC1 + lzone_trans;
                                vaoLinesThick.PushAttrib(trans: trans, rgba: GetZoneColor(Color4.Red));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_ramp_start), rgba: GetZoneColor(Color4.Red));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_ramp_start), rgba: GetZoneColor(MathExt.Lerp(Color4.Red, Color4.Lime, (-lerp_ramp_start) / wave_ramp)));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_sustain_start), rgba: GetZoneColor(MathExt.Lerp(Color4.Red, Color4.Lime, -(lerp_ramp_start - 1) / wave_ramp)));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_sustain_start), rgba: GetZoneColor(Color4.Lime));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_sustain_end), rgba: GetZoneColor(Color4.Lime));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_sustain_end), rgba: GetZoneColor(MathExt.Lerp(Color4.Red, Color4.Lime, lerp_ramp_end / wave_ramp)));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_ramp_end), rgba: GetZoneColor(MathExt.Lerp(Color4.Red, Color4.Lime, (lerp_ramp_end - 1) / wave_ramp)));
                                vaoLinesThick.PushAttrib(trans: MathExt.Lerp(trans, link_trans, lerp_ramp_end), rgba: GetZoneColor(Color4.Red));
                                vaoLinesThick.PushAttrib(trans: link_trans, rgba: GetZoneColor(Color4.Red));
                            }
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
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i - 1].X, entity.Positions[i - 1].Y, entity.Positions[i - 1].Z) / GameScales.ZoneEntityC1 + zone_trans,
                                            rgba: GetZoneColor(Color4.Blue));
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z) / GameScales.ZoneEntityC1 + zone_trans,
                                            rgba: GetZoneColor(Color4.Blue));
                    }
                    foreach (EntityPosition position in entity.Positions)
                    {
                        var cur_trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zone_trans;
                        AddSprite(cur_trans, new Vector2(1), GetZoneColor(Color4.Red), OldResources.PointTexture);
                    }
                }

                if (draw_type)
                {
                    if (gools.ContainsKey(entity.Type))
                        text_y += AddText3D($"{gools[entity.Type].EName}-{entity.Subtype}", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                    else
                        text_y += AddText3D($"{entity.Type}-{entity.Subtype} (invalid type)", trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                }
            }
        }

        private void RenderCamera(OldCamera camera)
        {
            byte alpha_backup;
            for (int i = 0; i < camera.Positions.Count; ++i, zone_alpha = alpha_backup)
            {
                alpha_backup = zone_alpha;
                var position = camera.Positions[i];
                var trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zone_trans;
                if (anchormode)
                {
                    zone_alpha = (byte)MathExt.LerpScale(0, zone_alpha, Vector3.Distance(trans, -render.Projection.Trans), 3, 7);
                    if (zone_alpha == 0)
                        continue;
                }
                if (i > 0)
                {
                    var prev_position = camera.Positions[i - 1];
                    vaoLines.PushAttrib(trans: new Vector3(prev_position.X, prev_position.Y, prev_position.Z) / GameScales.ZoneCameraC1 + zone_trans, rgba: GetZoneColor(Color4.Green));
                    vaoLines.PushAttrib(trans: trans, rgba: GetZoneColor(Color4.Green));
                }

                AddSprite(trans, new Vector2(1), GetZoneColor(Color4.Yellow), OldResources.PointTexture);

                float ang2rad = MathHelper.Pi / 2048;
                var quatAng = Quaternion.FromEulerAngles(-position.XRot * ang2rad, -position.YRot * ang2rad, -position.ZRot * ang2rad);
                var rot_mat = Matrix4.CreateFromQuaternion(quatAng);
                var test_vec = (rot_mat * new Vector4(0, 0, -1, 1)).Xyz;

                Rgba angColor = GetZoneColor(Color4.Olive);
                vaoLines.PushAttrib(trans: trans, rgba: angColor);
                vaoLines.PushAttrib(trans: trans + test_vec, rgba: angColor);
                AddSprite(trans + test_vec, new Vector2(0.5f), angColor, OldResources.PointTexture);
            }
        }

        private bool RenderPickupEntity(Vector3 trans, int subtype)
        {
            var texture = GetPickupTexture(subtype);
            AddSprite(trans, GetPickupScale(subtype), GetZoneColor(Color4.White), texture);
            return texture != OldResources.UnknownPickupTexture;
        }

        private void RenderBoxEntity(Vector3 trans, int subtype)
        {
            Rectangle sideTexRect = OldResources.TexMap[GetBoxSideTexture(subtype)];
            Span<Vector2> uvs_side = stackalloc Vector2[6] {
                new Vector2(sideTexRect.Left, sideTexRect.Bottom),
                new Vector2(sideTexRect.Left, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Top),
                new Vector2(sideTexRect.Right, sideTexRect.Bottom),
                new Vector2(sideTexRect.Left, sideTexRect.Bottom)
            };
            Span<Rgba> cols = stackalloc Rgba[5]
            {
                GetZoneColor(93*2, 93*2, 93*2),
                GetZoneColor(51*2, 51*2, 76*2),
                GetZoneColor(115*2, 115*2, 92*2),
                GetZoneColor(33*2, 33*2, 59*2),
                GetZoneColor(115*2, 115*2, 92*2)
            };
            for (int i = 0; i < 3 * 6; ++i)
            {
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6], st: uvs_side[i % 6]);
            }
            
            Rectangle topTexRect = OldResources.TexMap[GetBoxTopTexture(subtype)];
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
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6 - 1], st: uvs_top[i % 6]);
            }
        }

        private Bitmap GetBoxTopTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    return (OldResources.TNTBoxTopTexture);
                case 2: // Empty
                case 3: // Spring
                case 6: // Fruit
                case 8: // Life
                case 9: // Doctor
                case 10: // Pickup
                case 11: // POW
                case 17: // Pickup AutoGrav
                case 20: // Empty AutoGrav
                    return (OldResources.EmptyBoxTexture);
                case 4: // Continue
                    return (OldResources.ContinueBoxTexture);
                case 5: // Iron
                case 7: // Action
                case 15: // Iron Spring
                    return (OldResources.IronBoxTexture);
                default:
                    return (OldResources.UnknownBoxTopTexture);
            }
        }

        private Bitmap GetBoxSideTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    return (OldResources.TNTBoxTexture);
                case 2: // Empty
                case 20: // Empty AutoGrav
                    return (OldResources.EmptyBoxTexture);
                case 3: // Spring
                    return (OldResources.SpringBoxTexture);
                case 4: // Continue
                    return (OldResources.ContinueBoxTexture);
                case 5: // Iron
                    return (OldResources.IronBoxTexture);
                case 6: // Fruit
                    return (OldResources.FruitBoxTexture);
                case 7: // Action
                    return (OldResources.ActionBoxTexture);
                case 8: // Life
                    return (OldResources.LifeBoxTexture);
                case 9: // Doctor
                    return (OldResources.DoctorBoxTexture);
                case 10: // Pickup
                case 17: // Pickup AutoGrav
                    return (OldResources.PickupBoxTexture);
                case 11: // POW
                    return (OldResources.POWBoxTexture);
                case 13: // Ghost
                case 19: // Ghost Iron
                    return (OldResources.UnknownBoxTopTexture);
                case 15: // Iron Spring
                    return (OldResources.IronSpringBoxTexture);
                default:
                    return (OldResources.UnknownBoxTexture);
            }
        }

        private Bitmap GetPickupTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // Lime
                    return (OldResources.LimeTexture);
                case 1: // Coconut
                    return (OldResources.CoconutTexture);
                case 2: // Pineapple
                    return (OldResources.PineappleTexture);
                case 3: // Strawberry
                    return (OldResources.StrawberryTexture);
                case 4: // Mango
                    return (OldResources.MangoTexture);
                case 5: // Life
                    return (OldResources.LifeTexture);
                case 6: // Mask
                    return (OldResources.MaskTexture);
                case 7: // Lemon
                    return (OldResources.LemonTexture);
                case 8: // YYY
                    return (OldResources.YYYTexture);
                case 11: // Grape
                    return (OldResources.GrapeTexture);
                case 16: // Apple
                    return (OldResources.AppleTexture);
                case 18: // Cortex
                    return (OldResources.CortexTexture);
                case 19: // Brio
                    return (OldResources.BrioTexture);
                case 20: // Tawna
                    return (OldResources.TawnaTexture);
                default:
                    return (OldResources.UnknownPickupTexture);
            }
        }

        private Vector2 GetPickupScale(int subtype)
        {
            switch (subtype)
            {
                case 0: // Lime
                case 1: // Coconut
                case 4: // Mango
                case 7: // Lemon
                case 8: // YYY
                    return new Vector2(0.7f, 0.7f);
                case 2: // Pineapple
                    return new Vector2(0.7f, 1.4f);
                case 3: // Strawberry
                    return new Vector2(0.8f, 0.8f);
                case 5: // Life
                case 6: // Mask
                case 18: // Cortex
                case 20: // Tawna
                    return new Vector2(1.8f, 1.125f);
                case 16: // Apple
                    return new Vector2(0.675f, 0.84375f);
                case 19: // Brio
                    return new Vector2(1.8f, 1.8f);
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