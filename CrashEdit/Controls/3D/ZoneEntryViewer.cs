using Crash;
using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ZoneEntryViewer : SceneryEntryViewer
    {
        private readonly OctreeRenderer octreeRenderer;

        private readonly List<int> zones;
        private readonly int this_zone;

        internal bool masterZone;
        internal byte masterZoneAlpha;
        internal Vector3 zoneTrans;

        public ZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
            octreeRenderer = new(this);
        }

        public ZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
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
                    var zone = nsf.GetEntry<ZoneEntry>(eid);
                    var zonetrans = new Position(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
                    yield return zonetrans;
                    yield return new Position(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1 + zonetrans;
                    for (int i = zone.CameraCount; i < zone.Entities.Count; ++i)
                    {
                        Entity entity = zone.Entities[i];
                        foreach (EntityPosition position in entity.Positions)
                        {
                            int x = entity.Type != 34 ? position.X : position.X + 50;
                            int y = entity.Type != 34 ? position.Y : position.Y + 50;
                            int z = entity.Type != 34 ? position.Z : position.Z + 50;
                            yield return new Position(x, y, z) / GameScales.ZoneEntityC1 + zonetrans;
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

        private IEnumerable<ZoneEntry> GetZones()
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

            base.Render();

            foreach (var zone in allzones)
            {
                masterZone = Entry.NullEID == this_zone || zone.EID == this_zone;
                masterZoneAlpha = (byte)(masterZone ? 255 : 128);
                RenderZone(zone);
            }
        }

        private void RenderZone(ZoneEntry zone)
        {
            zoneTrans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            Vector3 zoneSize = new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1;
            AddBox(zoneTrans,
                   new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                   new Rgba(255, 255, 255, masterZoneAlpha),
                   true);
            for (int i = zone.CameraCount; i < zone.Entities.Count; ++i)
            {
                Entity entity = zone.Entities[i];
                RenderEntity(entity);
            }

            if (octreeRenderer.Enabled && (masterZone || octreeRenderer.ShowAllEntries))
            {
                octreeRenderer.NodeAlpha = masterZoneAlpha;
                octreeRenderer.RenderOctree(zone.Layout, 0x1C, zoneTrans.X, zoneTrans.Y, zoneTrans.Z, zoneSize.X, zoneSize.Y, zoneSize.Z, zone.CollisionDepthX, zone.CollisionDepthY, zone.CollisionDepthZ);
            }
        }

        private void RenderEntity(Entity entity)
        {
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zoneTrans;
                int subtype = entity.Subtype ?? -1;
                switch (entity.Type)
                {
                    case 3:
                        RenderPickupEntity(trans + new Vector3(0, .5f, 0), subtype);
                        break;
                    case 34:
                        RenderBoxEntity(trans, subtype);
                        break;
                    default:
                        AddSprite(trans, new Vector2(1), new(255, 255, 255, masterZoneAlpha), OldResources.PointTexture);
                        break;
                }
            }
            else
            {
                for (int i = 1; i < entity.Positions.Count; ++i)
                {
                    vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i - 1].X, entity.Positions[i - 1].Y, entity.Positions[i - 1].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                        rgba: new Rgba(0, 0, 255, masterZoneAlpha));
                    vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                        rgba: new Rgba(0, 0, 255, masterZoneAlpha));
                }
                foreach (EntityPosition position in entity.Positions)
                {
                    Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zoneTrans;
                    AddSprite(trans, new Vector2(1), new(255, 0, 0, masterZoneAlpha), OldResources.PointTexture);
                }
            }
        }

        private void RenderPickupEntity(Vector3 trans, int subtype)
        {
            AddSprite(trans, GetPickupScale(subtype), new(255, 255, 255, masterZoneAlpha), GetPickupTexture(subtype));
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
            Rgba[] cols = new Rgba[6]
            {
                new Rgba(93*2, 93*2, 93*2, masterZoneAlpha),
                new Rgba(51*2, 51*2, 76*2, masterZoneAlpha),
                new Rgba(115*2, 115*2, 92*2, masterZoneAlpha),
                new Rgba(51*2, 51*2, 76*2, masterZoneAlpha),
                new Rgba(33*2, 33*2, 59*2, masterZoneAlpha),
                new Rgba(115*2, 115*2, 92*2, masterZoneAlpha)
            };
            for (int i = 0; i < 4 * 6; ++i)
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
                vaoTris.PushAttrib(trans: trans + BoxVerts[BoxTriIndices[i]] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6], st: uvs[i % 6]);
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
                case 18: // Nitro
                    return (OldResources.NitroBoxTopTexture);
                case 23: // Steel
                    return (OldResources.SteelBoxTexture);
                case 24: // Action Nitro
                    return (OldResources.ActionNitroBoxTopTexture);
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
                case 18: // Nitro
                    return (OldResources.NitroBoxTexture);
                case 23: // Steel
                    return (OldResources.SteelBoxTexture);
                case 24: // Action Nitro
                    return (OldResources.ActionNitroBoxTexture);
                default:
                    return (OldResources.UnknownBoxTexture);
            }
        }

        private Bitmap GetPickupTexture(int subtype)
        {
            switch (subtype)
            {
                case 5: // Life
                    return (OldResources.LifeTexture);
                case 6: // Mask
                    return (OldResources.MaskTexture);
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