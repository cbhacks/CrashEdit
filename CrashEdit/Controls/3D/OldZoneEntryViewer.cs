using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Drawing;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private VAO vaoLines;
        private VAO vaoBoxEntity;

        private List<int> zones;
        private int this_zone;

        internal bool masterZone;
        internal float masterZoneAlpha;
        internal Vector3 zoneTrans;

        public OldZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
        }

        public OldZoneEntryViewer(NSF nsf, List<int> zone_eids) : base(nsf, new List<int>())
        {
            zones = zone_eids;
            this_zone = Entry.NullEID;
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            vaoLines = new VAO(shaderContext, "line", PrimitiveType.Lines);
            vaoBoxEntity = new VAO(shaderContext, "generic", PrimitiveType.Triangles);
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

            base.Render();

            SetBlendMode(BlendMode.Solid);
            foreach (var zone in allzones)
            {
                masterZone = zones.Count != 1 || zone.EID == this_zone;
                masterZoneAlpha = masterZone ? 1f : 0.5f;
                RenderZone(zone);
            }

            vaoBoxEntity.RenderAndDiscard(render);
            vaoLines.RenderAndDiscard(render);
        }

        private void RenderZone(OldZoneEntry zone)
        {
            zoneTrans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            RenderBoxLineAS(zoneTrans,
                            new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1,
                            new(1, 1, 1, masterZoneAlpha));
            foreach (OldEntity entity in zone.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in zone.Cameras)
            {
                RenderCamera(camera);
            }
        }

        private void RenderEntity(OldEntity entity)
        {
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zoneTrans;
                switch (entity.Type)
                {
                    case 3:
                        RenderPickupEntity(trans + new Vector3(0, .5f, 0), entity.Subtype);
                        break;
                    case 34:
                        RenderBoxEntity(trans, entity.Subtype);
                        break;
                    default:
                        RenderSprite(trans, new Vector2(1), new(1, 1, 1, masterZoneAlpha), OldResources.PointTexture);
                        break;
                }
            }
            else
            {
                for (int i = 1; i < entity.Positions.Count; ++i)
                {
                    vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i - 1].X, entity.Positions[i - 1].Y, entity.Positions[i - 1].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                        rgba: new Rgba(0, 0, 255, 255));
                    vaoLines.PushAttrib(trans: new Vector3(entity.Positions[i].X, entity.Positions[i].Y, entity.Positions[i].Z) / GameScales.ZoneEntityC1 + zoneTrans,
                                        rgba: new Rgba(0, 0, 255, 255));
                }
                foreach (EntityPosition position in entity.Positions)
                {
                    Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + zoneTrans;
                    RenderSprite(trans, new Vector2(1), Color4.Red, OldResources.PointTexture);
                }
            }
        }

        private void RenderCamera(OldCamera camera)
        {
            for (int i = 1; i < camera.Positions.Count; ++i)
            {
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i - 1].X, camera.Positions[i - 1].Y, camera.Positions[i - 1].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: new Rgba(0, 128, 0, (byte)(masterZoneAlpha * 255)));
                vaoLines.PushAttrib(trans: new Vector3(camera.Positions[i].X, camera.Positions[i].Y, camera.Positions[i].Z) / GameScales.ZoneCameraC1 + zoneTrans,
                                    rgba: new Rgba(0, 128, 0, (byte)(masterZoneAlpha * 255)));
            }
            foreach (OldCameraPosition position in camera.Positions)
            {
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneCameraC1 + zoneTrans;
                RenderSprite(trans, new Vector2(1), new(1, 1, 0, masterZoneAlpha), OldResources.PointTexture);
            }
        }

        private void RenderPickupEntity(Vector3 trans, int subtype)
        {
            RenderSprite(trans, GetPickupScale(subtype), new(1, 1, 1, masterZoneAlpha), GetPickupTexture(subtype));
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
                new Rgba(93*2, 93*2, 93*2, 255),
                new Rgba(51*2, 51*2, 76*2, 255),
                new Rgba(115*2, 115*2, 92*2, 255),
                new Rgba(33*2, 33*2, 59*2, 255),
                new Rgba(115*2, 115*2, 92*2, 255)
            };
            for (int i = 0; i < 3 * 6; ++i)
            {
                vaoBoxEntity.PushAttrib(trans: trans + BoxTriVerts[i] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6], st: uvs[i % 6]);
            }
            uvs[0] = new Vector2(topTexRect.Left, topTexRect.Bottom);
            uvs[1] = new Vector2(topTexRect.Left, topTexRect.Top);
            uvs[2] = new Vector2(topTexRect.Right, topTexRect.Top);
            uvs[3] = new Vector2(topTexRect.Right, topTexRect.Top);
            uvs[4] = new Vector2(topTexRect.Right, topTexRect.Bottom);
            uvs[5] = new Vector2(topTexRect.Left, topTexRect.Bottom);
            for (int i = 4 * 6; i < 6 * 6; ++i)
            {
                vaoBoxEntity.PushAttrib(trans: trans + BoxTriVerts[i] * 0.5f + new Vector3(0.5f), rgba: cols[i / 6 - 1], st: uvs[i % 6]);
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
            vaoLines?.Dispose();

            base.Dispose(disposing);
        }
    }
}