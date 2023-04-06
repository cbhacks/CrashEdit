using Crash;
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

        private bool octreeFlip;

        internal bool masterZone;
        internal byte masterZoneAlpha;
        internal Vector3 zoneTrans;

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

        private IEnumerable<ProtoZoneEntry> GetZones()
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

        private void RenderZone(ProtoZoneEntry zone)
        {
            zoneTrans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddBox(zoneTrans,
                   new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                   new Rgba(255, 255, 255, masterZoneAlpha),
                   true);
            foreach (ProtoEntity entity in zone.Entities)
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
                int maxx = zone.CollisionDepthX;
                int maxy = zone.CollisionDepthY;
                int maxz = zone.CollisionDepthZ;
                if (maxy == 0)
                    maxy = maxx;
                if (maxz == 0)
                    maxz = maxy;
                if (octreeFlip)
                {
                    octreeRenderer.RenderOctree(zone.Layout, 0x1C, zoneTrans.X, zoneTrans.Y + zoneSize.Y, zoneTrans.Z + zoneSize.Z, zoneSize.X, -zoneSize.Y, -zoneSize.Z, maxx, maxy, maxz);
                }
                else
                {
                    octreeRenderer.RenderOctree(zone.Layout, 0x1C, zoneTrans.X, zoneTrans.Y, zoneTrans.Z, zoneSize.X, zoneSize.Y, zoneSize.Z, maxx, maxy, maxz);
                }
            }
        }

        private void RenderEntity(ProtoEntity entity)
        {
            Vector3 trans = new Vector3(entity.StartX, entity.StartY, entity.StartZ) / GameScales.ZoneC1 + zoneTrans;
            if (entity.Deltas.Count == 0)
            {
                switch (entity.Type)
                {
                    case 3:
                        RenderPickupEntity(trans + new Vector3(0, .5f, 0), entity.Subtype);
                        break;
                    default:
                        AddSprite(trans, new Vector2(1), new(255, 255, 255, masterZoneAlpha), OldResources.PointTexture);
                        break;
                }
            }
            else
            {
                AddSprite(trans, new Vector2(1), new(255, 0, 0, masterZoneAlpha), OldResources.PointTexture);
                foreach (ProtoEntityPosition delta in entity.Deltas)
                {
                    vaoLines.PushAttrib(trans: trans, rgba: new Rgba(0, 0, 255, masterZoneAlpha));
                    trans += new Vector3(delta.X, delta.Y, delta.Z) * 8 / GameScales.ZoneC1;
                    vaoLines.PushAttrib(trans: trans, rgba: new Rgba(0, 0, 255, masterZoneAlpha));
                    AddSprite(trans, new Vector2(1), new(255, 0, 0, masterZoneAlpha), OldResources.PointTexture);
                }
            }
        }

        private void RenderCamera(OldCamera camera)
        {
            for (int i = 1; i < camera.Positions.Count; ++i)
            {
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i - 1].X, camera.Positions[i - 1].Y, camera.Positions[i - 1].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: new Rgba(0, 128, 0, masterZoneAlpha));
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i].X, camera.Positions[i].Y, camera.Positions[i].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: new Rgba(0, 128, 0, masterZoneAlpha));
            }
            foreach (OldCameraPosition position in camera.Positions)
            {
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zoneTrans;
                AddSprite(trans, new Vector2(1), new(255, 255, 0, masterZoneAlpha), OldResources.PointTexture);

                float ang2rad = MathHelper.Pi / 2048;
                var quatAng = Quaternion.FromEulerAngles(-position.XRot * ang2rad, -position.YRot * ang2rad, -position.ZRot * ang2rad);
                var rot_mat = Matrix4.CreateFromQuaternion(quatAng);
                var test_vec = (rot_mat * new Vector4(0, 0, -1, 1)).Xyz;

                Rgba angColor = (Rgba)Color4.Olive;
                vaoLines.PushAttrib(trans: trans, rgba: angColor);
                vaoLines.PushAttrib(trans: trans + test_vec, rgba: angColor);
                AddSprite(trans + test_vec, new Vector2(0.5f), angColor, OldResources.PointTexture);
            }
        }

        private void RenderPickupEntity(Vector3 trans, int subtype)
        {
            AddSprite(trans, GetPickupScale(subtype), new(255, 255, 255, masterZoneAlpha), GetPickupTexture(subtype));
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