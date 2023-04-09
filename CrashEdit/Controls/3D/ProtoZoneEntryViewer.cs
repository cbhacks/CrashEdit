using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryViewer : ProtoSceneryEntryViewer
    {
        private readonly OctreeRenderer octreeRenderer;

        private readonly List<int> zones;
        private readonly int this_zone;

        private readonly Dictionary<int, GOOLEntry> gools = new();
        private bool is_master_zone;
        private byte zone_alpha;
        private Vector3 zone_trans;

        private Rgba GetZoneColor(Color4 color)
        {
            return new Rgba(color, zone_alpha);
        }

        private Rgba GetZoneColor(byte r, byte g, byte b)
        {
            return new Rgba(r, g, b, zone_alpha);
        }

        private bool octreeFlip;

        public ProtoZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
            octreeRenderer = new(this);
        }

        public ProtoZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
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
                    var zone = nsf.GetEntry<ProtoZoneEntry>(eid);
                    var zonetrans = new Position(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
                    yield return zonetrans;
                    yield return new Position(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1 + zonetrans;
                    foreach (ProtoEntity entity in zone.Entities)
                    {
                        float x = entity.StartX / GameScales.ZoneC1;
                        float y = entity.StartY / GameScales.ZoneC1;
                        float z = entity.StartZ / GameScales.ZoneC1;
                        yield return new Position(x, y, z) + zonetrans;
                        foreach (ProtoEntityPosition delta in entity.Deltas)
                        {
                            x += delta.X * 8 / GameScales.ZoneC1;
                            y += delta.Y * 8 / GameScales.ZoneC1;
                            z += delta.Z * 8 / GameScales.ZoneC1;
                            yield return new Position(x, y, z) + zonetrans;
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

            if (KPress(Keys.O)) octreeFlip = !octreeFlip;
        }

        private IList<ProtoZoneEntry> GetZones()
        {
            var ret = new List<ProtoZoneEntry>();
            foreach (int eid in zones)
            {
                var zone = nsf.GetEntry<ProtoZoneEntry>(eid);
                if (zone != null)
                {
                    for (int i = 0; i < zone.ZoneCount; ++i)
                    {
                        var lzone = nsf.GetEntry<ProtoZoneEntry>(zone.GetLinkedZone(i));
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
            ProtoZoneEntry master_zone = null;
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

        private void RenderZone(ProtoZoneEntry zone)
        {
            zone_trans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddText3D(zone.EName, zone_trans + new Vector3(zoneSize.X, 0, zoneSize.Z) / 2, GetZoneColor(Color4.White), size: 2, flags: TextRenderFlags.Shadow | TextRenderFlags.Top | TextRenderFlags.Center);
            AddBox(zone_trans,
                   new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                   GetZoneColor(Color4.White),
                   true);
            foreach (ProtoEntity entity in zone.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in zone.Cameras)
            {
                RenderCamera(camera);
            }

            if (octreeRenderer.Enabled && (is_master_zone || octreeRenderer.ShowAllEntries))
            {
                octreeRenderer.NodeAlpha = zone_alpha;
                int maxx = zone.CollisionDepthX;
                int maxy = zone.CollisionDepthY;
                int maxz = zone.CollisionDepthZ;
                if (maxy == 0)
                    maxy = maxx;
                if (maxz == 0)
                    maxz = maxy;
                if (octreeFlip)
                {
                    octreeRenderer.RenderOctree(zone.Layout, 0x1C, zone_trans.X, zone_trans.Y + zoneSize.Y, zone_trans.Z + zoneSize.Z, zoneSize.X, -zoneSize.Y, -zoneSize.Z, maxx, maxy, maxz);
                }
                else
                {
                    octreeRenderer.RenderOctree(zone.Layout, 0x1C, zone_trans.X, zone_trans.Y, zone_trans.Z, zoneSize.X, zoneSize.Y, zoneSize.Z, maxx, maxy, maxz);
                }
            }
        }

        private void RenderEntity(ProtoEntity entity)
        {
            float text_y = Settings.Default.Font3DEnable ? 0 : float.MaxValue;
            bool draw_type = true;

            Vector3 trans = new Vector3(entity.StartX, entity.StartY, entity.StartZ) / GameScales.ZoneC1 + zone_trans;
            AddText3D("entity-" + entity.ID, trans, GetZoneColor(Color4.Yellow), ofs_y: text_y, flags: TextRenderFlags.Default | TextRenderFlags.Bottom);
            if (entity.Deltas.Count == 0)
            {
                if (entity.Type == 3)
                {
                    draw_type = !RenderPickupEntity(trans + new Vector3(0, .5f, 0), entity.Subtype);
                }
                else
                {
                    AddSprite(trans, new Vector2(1), GetZoneColor(Color4.White), OldResources.PointTexture);
                }
            }
            else
            {
                var cur_trans = trans;
                AddSprite(cur_trans, new Vector2(1), GetZoneColor(Color4.Red), OldResources.PointTexture);
                foreach (ProtoEntityPosition delta in entity.Deltas)
                {
                    vaoLines.PushAttrib(trans: cur_trans, rgba: GetZoneColor(Color4.Blue));
                    cur_trans += new Vector3(delta.X, delta.Y, delta.Z) * 8 / GameScales.ZoneC1;
                    vaoLines.PushAttrib(trans: cur_trans, rgba: GetZoneColor(Color4.Blue));
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

        private void RenderCamera(OldCamera camera)
        {
            for (int i = 1; i < camera.Positions.Count; ++i)
            {
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i - 1].X, camera.Positions[i - 1].Y, camera.Positions[i - 1].Z) / GameScales.ZoneCameraC1 + zone_trans,
                                    rgba: GetZoneColor(Color4.Green));
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i].X, camera.Positions[i].Y, camera.Positions[i].Z) / GameScales.ZoneCameraC1 + zone_trans,
                                    rgba: GetZoneColor(Color4.Green));
            }
            foreach (OldCameraPosition position in camera.Positions)
            {
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zone_trans;
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
                    return new Vector2(1.8f, 1.125f);
                case 16: // Apple
                    return new Vector2(0.675f, 0.84375f);
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