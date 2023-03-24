using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private static int sprites;
        // private VAO vaoStuff;

        private List<int> zones;
        private int this_zone;

        public OldZoneEntryViewer(NSF nsf, int zone_eid) : base(nsf, new List<int>())
        {
            zones = new() { zone_eid };
            this_zone = zone_eid;
        }

        protected override void GLLoadStatic()
        {
            base.GLLoadStatic();

            // make texture for sprites
            sprites = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, sprites);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            BitmapData data = OldResources.AllTex.LockBits(new Rectangle(Point.Empty, OldResources.AllTex.Size), ImageLockMode.ReadOnly, OldResources.AllTex.PixelFormat);
            try
            {
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8ui, OldResources.AllTex.Width, OldResources.AllTex.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.BgraInteger, PixelType.UnsignedByte, data.Scan0);
            }
            catch
            {
                GL.BindTexture(TextureTarget.Texture2D, 0);
                Console.WriteLine("Error making texture.");
            }
            finally
            {
                OldResources.AllTex.UnlockBits(data);
            }
        }

        protected override void GLLoad()
        {
            base.GLLoad();

            // vaoStuff = new(render.ShaderContext, "crash1", PrimitiveType.Triangles);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var zone = nsf.GetEntry<OldZoneEntry>(this_zone);
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
                        yield return new Position(x,y,z) / GameScales.ZoneEntityC1 + zonetrans;
                    }
                }
                foreach (OldCamera camera in zone.Cameras)
                {
                    foreach (OldCameraPosition position in camera.Positions)
                    {
                        int x = position.X;
                        int y = position.Y;
                        int z = position.Z;
                        yield return new Position(x,y,z) / GameScales.ZoneCameraC1 + zonetrans;
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
                RenderZone(zone);
            }
        }

        private void RenderZone(OldZoneEntry zone)
        {
            RenderBoxLineAS(new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1, new Vector3(zone.Width, zone.Height, zone.Depth) / GameScales.ZoneC1, Color4.White);
            foreach (OldEntity entity in zone.Entities)
            {
                RenderEntity(zone, entity);
            }
            foreach (OldCamera camera in zone.Cameras)
            {
                //RenderCamera(camera);
            }
        }

        private void RenderEntity(OldZoneEntry zone, OldEntity entity)
        {
            var ztrans = new Vector3(zone.X, zone.Y, zone.Z) / GameScales.ZoneC1;
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + ztrans;
                switch (entity.Type)
                {
                    case 0x3:
                        RenderPickup(trans + new Vector3(0, .5f, 0), entity.Subtype);
                        break;
                    //case 0x22:
                        //RenderBox(trans, entity.Subtype);
                        //break;
                    default:
                        RenderSprite(trans, new Vector2(1), Color4.White, OldResources.PointTexture);
                        break;
                }
            }
            else
            {
                /*
                GL.Color3(Color.Blue);
                GL.PushMatrix();
                GL.Begin(PrimitiveType.LineStrip);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Vertex3(position.X, position.Y, position.Z);
                }
                GL.End();
                */
                foreach (EntityPosition position in entity.Positions)
                {
                    Vector3 trans = new Vector3(position.X, position.Y, position.Z) / GameScales.ZoneEntityC1 + ztrans;
                    RenderSprite(trans, new Vector2(1), Color4.Red, OldResources.PointTexture);
                }
            }
        }

        /*
        private void RenderEntry(OldZoneEntry entry,ref int octreedisplaylist)
        {
            common.CurrentEntry = entry;
            int xoffset = BitConv.FromInt32(entry.Layout,0);
            int yoffset = BitConv.FromInt32(entry.Layout,4);
            int zoffset = BitConv.FromInt32(entry.Layout,8);
            int x2 = BitConv.FromInt32(entry.Layout,12);
            int y2 = BitConv.FromInt32(entry.Layout,16);
            int z2 = BitConv.FromInt32(entry.Layout,20);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            if (common.DeleteLists)
            {
                GL.DeleteLists(octreedisplaylist,1);
                octreedisplaylist = -1;
            }
            if (common.EnableOctree)
            {
                if (!common.PolygonMode)
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
                if (octreedisplaylist == -1)
                {
                    octreedisplaylist = GL.GenLists(1);
                    GL.NewList(octreedisplaylist,ListMode.CompileAndExecute);
                    GL.PushMatrix();
                    int xmax = (ushort)BitConv.FromInt16(entry.Layout,0x1E);
                    int ymax = (ushort)BitConv.FromInt16(entry.Layout,0x20);
                    int zmax = (ushort)BitConv.FromInt16(entry.Layout,0x22);
                    common.RenderOctree(entry.Layout,0x1C,0,0,0,x2,y2,z2,xmax,ymax,zmax);
                    GL.PopMatrix();
                    GL.EndList();
                }
                else
                {
                    GL.CallList(octreedisplaylist);
                }
                GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
            }
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(0,0,0);
            GL.Vertex3(x2,0,0);
            GL.Vertex3(x2,y2,0);
            GL.Vertex3(0,y2,0);
            GL.Vertex3(0,0,0);
            GL.Vertex3(0,0,z2);
            GL.Vertex3(x2,0,z2);
            GL.Vertex3(x2,y2,z2);
            GL.Vertex3(0,y2,z2);
            GL.Vertex3(0,0,z2);
            GL.Vertex3(x2,0,z2);
            GL.Vertex3(x2,0,0);
            GL.Vertex3(x2,y2,0);
            GL.Vertex3(x2,y2,z2);
            GL.Vertex3(0,y2,z2);
            GL.Vertex3(0,y2,0);
            GL.End();
            GL.Scale(4,4,4);
            foreach (OldEntity entity in entry.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in entry.Cameras)
            {
                RenderCamera(camera);
            }
            GL.PopMatrix();
        }

        private void RenderLinkedEntry(OldZoneEntry entry, ref int octreedisplaylist)
        {
            common.CurrentEntry = entry;
            int xoffset = BitConv.FromInt32(entry.Layout,0);
            int yoffset = BitConv.FromInt32(entry.Layout,4);
            int zoffset = BitConv.FromInt32(entry.Layout,8);
            int x2 = BitConv.FromInt32(entry.Layout,12);
            int y2 = BitConv.FromInt32(entry.Layout,16);
            int z2 = BitConv.FromInt32(entry.Layout,20);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            if (common.AllEntries)
            {
                if (common.DeleteLists)
                {
                    GL.DeleteLists(octreedisplaylist,1);
                    octreedisplaylist = -1;
                }
                if (common.EnableOctree)
                {
                    GL.Disable(EnableCap.PolygonStipple);
                    if (!common.PolygonMode)
                        GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
                    if (octreedisplaylist == -1)
                    {
                        octreedisplaylist = GL.GenLists(1);
                        GL.NewList(octreedisplaylist,ListMode.CompileAndExecute);
                        GL.PushMatrix();
                        int xmax = (ushort)BitConv.FromInt16(entry.Layout,0x1E);
                        int ymax = (ushort)BitConv.FromInt16(entry.Layout,0x20);
                        int zmax = (ushort)BitConv.FromInt16(entry.Layout,0x22);
                        common.RenderOctree(entry.Layout,0x1C,0,0,0,x2,y2,z2,xmax,ymax,zmax);
                        GL.PopMatrix();
                        GL.EndList();
                    }
                    else
                    {
                        GL.CallList(octreedisplaylist);
                    }
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
                    GL.Enable(EnableCap.PolygonStipple);
                }
            }
            GL.Scale(4,4,4);
            foreach (OldEntity entity in entry.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in entry.Cameras)
            {
                RenderCamera(camera);
            }
            GL.PopMatrix();
        }
        private void RenderEntity(OldEntity entity)
        {
            GL.PolygonStipple(stipplea);
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                GL.PushMatrix();
                GL.Translate(position.X,position.Y,position.Z);
                switch (entity.Type)
                {
                    case 0x3:
                        RenderPickup(entity.Subtype);
                        break;
                    case 0x22:
                        RenderBox(entity.Subtype);
                        break;
                    default:
                        GL.Color3(Color.White);
                        LoadTexture(OldResources.PointTexture);
                        RenderSprite();
                        break;
                }
                GL.PopMatrix();
            }
            else
            {
                GL.Color3(Color.Blue);
                GL.PushMatrix();
                GL.Begin(PrimitiveType.LineStrip);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Vertex3(position.X,position.Y,position.Z);
                }
                GL.End();
                GL.Color3(Color.Red);
                LoadTexture(OldResources.PointTexture);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.PushMatrix();
                    GL.Translate(position.X,position.Y,position.Z);
                    RenderSprite();
                    GL.PopMatrix();
                }
                GL.PopMatrix();
            }
        }

        private void RenderCamera(OldCamera camera)
        {
            GL.PolygonStipple(stippleb);
            GL.Color3(Color.Green);
            GL.PushMatrix();
            GL.Scale(0.25F,0.25F,0.25F);
            GL.Begin(PrimitiveType.LineStrip);
            foreach (OldCameraPosition position in camera.Positions)
            {
                GL.Vertex3(position.X,position.Y,position.Z);
            }
            GL.End();
            GL.Color3(Color.Yellow);
            LoadTexture(OldResources.PointTexture);
            foreach (OldCameraPosition position in camera.Positions)
            {
                GL.PushMatrix();
                GL.Translate(position.X,position.Y,position.Z);
                GL.Scale(4,4,4);
                RenderSprite();
                GL.PopMatrix();
            }
            GL.PopMatrix();
        }
        */

        private void RenderPickup(Vector3 trans, int subtype)
        {
            RenderSprite(trans, GetPickupScale(subtype), Color4.White, GetPickupTexture(subtype));
        }
        /*
        private void RenderBox(int subtype)
        {
            GL.Translate(50,50,50);
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.White);
            LoadBoxSideTexture(subtype);
            GL.PushMatrix();
            GL.Color3(93/128F,93/128F,93/128F);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            GL.Color3(51/128F,51/128F,76/128F);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            //RenderBoxFace();
            GL.Rotate(90,0,1,0);
            GL.Color3(115/128F,115/128F,92/128F);
            RenderBoxFace();
            GL.PopMatrix();
            LoadBoxTopTexture(subtype);
            GL.PushMatrix();
            GL.Rotate(90,1,0,0);
            GL.Color3(33/128F,33/128F,59/128F);
            RenderBoxFace();
            GL.Rotate(180,1,0,0);
            GL.Color3(115/128F,115/128F,92/128F);
            RenderBoxFace();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderBoxFace()
        {
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex3(-50,+50,50);
            GL.TexCoord2(1,0);
            GL.Vertex3(+50,+50,50);
            GL.TexCoord2(1,1);
            GL.Vertex3(+50,-50,50);
            GL.TexCoord2(0,1);
            GL.Vertex3(-50,-50,50);
            GL.End();
        }

        private void LoadBoxTopTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    LoadTexture(OldResources.TNTBoxTopTexture);
                    break;
                case 2: // Empty
                case 3: // Spring
                case 6: // Fruit
                case 8: // Life
                case 9: // Doctor
                case 10: // Pickup
                case 11: // POW
                case 17: // Pickup AutoGrav
                case 20: // Empty AutoGrav
                    LoadTexture(OldResources.EmptyBoxTexture);
                    break;
                case 4: // Continue
                    LoadTexture(OldResources.ContinueBoxTexture);
                    break;
                case 5: // Iron
                case 7: // Action
                case 15: // Iron Spring
                    LoadTexture(OldResources.IronBoxTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownBoxTopTexture);
                    break;
            }
        }

        private void LoadBoxSideTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                case 16: // TNT AutoGrav
                    LoadTexture(OldResources.TNTBoxTexture);
                    break;
                case 2: // Empty
                case 20: // Empty AutoGrav
                    LoadTexture(OldResources.EmptyBoxTexture);
                    break;
                case 3: // Spring
                    LoadTexture(OldResources.SpringBoxTexture);
                    break;
                case 4: // Continue
                    LoadTexture(OldResources.ContinueBoxTexture);
                    break;
                case 5: // Iron
                    LoadTexture(OldResources.IronBoxTexture);
                    break;
                case 6: // Fruit
                    LoadTexture(OldResources.FruitBoxTexture);
                    break;
                case 7: // Action
                    LoadTexture(OldResources.ActionBoxTexture);
                    break;
                case 8: // Life
                    LoadTexture(OldResources.LifeBoxTexture);
                    break;
                case 9: // Doctor
                    LoadTexture(OldResources.DoctorBoxTexture);
                    break;
                case 10: // Pickup
                case 17: // Pickup AutoGrav
                    LoadTexture(OldResources.PickupBoxTexture);
                    break;
                case 11: // POW
                    LoadTexture(OldResources.POWBoxTexture);
                    break;
                case 13: // Ghost
                case 19: // Ghost Iron
                    LoadTexture(OldResources.UnknownBoxTopTexture);
                    break;
                case 15: // Iron Spring
                    LoadTexture(OldResources.IronSpringBoxTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownBoxTexture);
                    break;
            }
        }
        
         */

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
            //vaoStuff?.Dispose();

            base.Dispose(disposing);
        }
    }
}