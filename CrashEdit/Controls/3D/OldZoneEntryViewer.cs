using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private readonly OctreeRenderer octreeRenderer;

        private readonly List<int> zones;
        private readonly int this_zone;

        private readonly Dictionary<int, GOOLEntry> gools = new();
        private bool masterZone;
        private byte masterZoneAlpha;
        private Vector3 zoneTrans;

        private Rgba GetZoneColor(Color4 color)
        {
            return new Rgba(color, masterZoneAlpha);
        }

        private Rgba GetZoneColor(byte r, byte g, byte b)
        {
            return new Rgba(r, g, b, masterZoneAlpha);
        }

        public OldZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
            octreeRenderer = new(this);
        }

        public OldZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
        {
            zones = zone_eids;
            this_zone = Entry.NullEID;
            octreeRenderer = new(this);
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            octreeRenderer?.UpdateForm();
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

        protected override void RunLogic()
        {
            base.RunLogic();
            octreeRenderer.RunLogic();
        }

        private IEnumerable<OldZoneEntry> GetZones()
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

        protected override void Render()
        {
            var allzones = GetZones();
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

            foreach (var zone in allzones)
            {
                masterZone = Entry.NullEID == this_zone || zone.EID == this_zone;
                masterZoneAlpha = (byte)(masterZone ? 255 : 128);
                RenderZone(zone);
            }

            // render early
            PostRender();

            base.Render();
        }

        private void RenderZone(OldZoneEntry zone)
        {
            zoneTrans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddText3D(zone.EName, zoneTrans + new Vector3(zoneSize.X, 0, zoneSize.Z) / 2, GetZoneColor(Color4.White), size: 2, flags: TextRenderFlags.Shadow | TextRenderFlags.Top | TextRenderFlags.Center);
            AddBox(zoneTrans,
                   new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                   GetZoneColor(Color4.White),
                   true);
            foreach (OldEntity entity in zone.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in zone.Cameras)
            {
                RenderCamera(camera);
            }

            if (octreeRenderer.Enabled && (masterZone || octreeRenderer.ShowAllEntries))
            {
                octreeRenderer.NodeAlpha = masterZoneAlpha;
                octreeRenderer.RenderOctree(zone.Layout, 0x1C, zoneTrans.X, zoneTrans.Y, zoneTrans.Z, zoneSize.X, zoneSize.Y, zoneSize.Z, zone.CollisionDepthX, zone.CollisionDepthY, zone.CollisionDepthZ);
            }
        }

        private void RenderEntity(OldEntity entity)
        {
            float text_y = Settings.Default.Font3DEnable ? 0 : float.MaxValue;
            bool draw_type = true;
            Vector3 trans = new Vector3(entity.Positions[0].X, entity.Positions[0].Y, entity.Positions[0].Z) / GameScales.ZoneEntityC1 + zoneTrans;
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
                        else if (pickup >= 200 && pickup < 264) pickup_name = "crystal-" + (pickup - 200);
                        else if (pickup >= 300 && pickup < 364) pickup_name = "gem-" + (pickup - 300);
                        text_y += AddText3D(pickup_name, trans, GetZoneColor(Color4.White), ofs_y: text_y).Y;
                        int link_a = entity.VecZ;
                        var link_info = link_a == 0 ? null : nsf.GetEntityC1(link_a);
                        if (link_info != null)
                        {
                            var link = link_info.Item1;
                            var lzone = link_info.Item2;
                            if (link.Positions.Count > 0)
                            {
                                var lzone_trans = new Vector3(lzone.X, lzone.Y, lzone.Z) / GameScales.ZoneC1;
                                Vector3 link_trans = new Vector3(link.Positions[0].X, link.Positions[0].Y, link.Positions[0].Z) / GameScales.ZoneEntityC1 + lzone_trans;
                                vaoLinesThick.PushAttrib(trans: trans, rgba: GetZoneColor(Color4.Red));
                                vaoLinesThick.PushAttrib(trans: link_trans, rgba: GetZoneColor(Color4.DarkRed));
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
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i - 1].X, entity.Positions[i - 1].Y, entity.Positions[i - 1].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                            rgba: GetZoneColor(Color4.Blue));
                        vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                            rgba: GetZoneColor(Color4.Blue));
                    }
                    foreach (EntityPosition position in entity.Positions)
                    {
                        var cur_trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zoneTrans;
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
            for (int i = 1; i < camera.Positions.Count; ++i)
            {
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i - 1].X, camera.Positions[i - 1].Y, camera.Positions[i - 1].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: GetZoneColor(Color4.Green));
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i].X, camera.Positions[i].Y, camera.Positions[i].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: GetZoneColor(Color4.Green));
            }
            foreach (OldCameraPosition position in camera.Positions)
            {
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zoneTrans;
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
            Rectangle topTexRect = OldResources.TexMap[GetBoxTopTexture(subtype)];
            Vector2[] uvs = new Vector2[6];
            uvs[0] = new Vector2(sideTexRect.Left, sideTexRect.Bottom);
            uvs[1] = new Vector2(sideTexRect.Left, sideTexRect.Top);
            uvs[2] = new Vector2(sideTexRect.Right, sideTexRect.Top);
            uvs[3] = new Vector2(sideTexRect.Right, sideTexRect.Top);
            uvs[4] = new Vector2(sideTexRect.Right, sideTexRect.Bottom);
            uvs[5] = new Vector2(sideTexRect.Left, sideTexRect.Bottom);
            Rgba[] cols = new Rgba[5]
            {
                GetZoneColor(93*2, 93*2, 93*2),
                GetZoneColor(51*2, 51*2, 76*2),
                GetZoneColor(115*2, 115*2, 92*2),
                GetZoneColor(33*2, 33*2, 59*2),
                GetZoneColor(115*2, 115*2, 92*2)
            };
            for (int i = 0; i < 3 * 6; ++i)
            {
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6], st: uvs[i % 6]);
            }
            uvs[0] = new Vector2(topTexRect.Left, topTexRect.Bottom);
            uvs[1] = new Vector2(topTexRect.Left, topTexRect.Top);
            uvs[2] = new Vector2(topTexRect.Right, topTexRect.Top);
            uvs[3] = new Vector2(topTexRect.Right, topTexRect.Top);
            uvs[4] = new Vector2(topTexRect.Right, topTexRect.Bottom);
            uvs[5] = new Vector2(topTexRect.Left, topTexRect.Bottom);
            for (int i = 4 * 6; i < 6 * 6; ++i)
            {
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6 - 1], st: uvs[i % 6]);
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
            octreeRenderer?.Dispose();

            base.Dispose(disposing);
        }
    }
}