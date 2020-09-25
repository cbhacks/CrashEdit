using Crash;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static OldZoneEntryViewer()
        {
            stipplea = new byte[128];
            stippleb = new byte[128];
            for (int i = 0; i < 128; i += 8)
            {
                stipplea[i + 0] = 0x55;
                stipplea[i + 1] = 0x55;
                stipplea[i + 2] = 0x55;
                stipplea[i + 3] = 0x55;
                stipplea[i + 4] = 0xAA;
                stipplea[i + 5] = 0xAA;
                stipplea[i + 6] = 0xAA;
                stipplea[i + 7] = 0xAA;
                stippleb[i + 0] = 0xAA;
                stippleb[i + 1] = 0xAA;
                stippleb[i + 2] = 0xAA;
                stippleb[i + 3] = 0xAA;
                stippleb[i + 4] = 0x55;
                stippleb[i + 5] = 0x55;
                stippleb[i + 6] = 0x55;
                stippleb[i + 7] = 0x55;
            }
        }

        private OldZoneEntry entry;
        private OldZoneEntry[] linkedentries;
        private CommonZoneEntryViewer common;

        public string EntryName => entry.EName;

        public OldZoneEntryViewer(OldZoneEntry entry,OldSceneryEntry[] linkedsceneryentries,TextureChunk[][] texturechunks,OldZoneEntry[] linkedentries) 
            : base(linkedsceneryentries,texturechunks)
        {
            this.entry = entry;
            this.linkedentries = linkedentries;
            common = new CommonZoneEntryViewer(linkedentries.Length + 1);
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                int xoffset = BitConv.FromInt32(entry.Layout,0);
                int yoffset = BitConv.FromInt32(entry.Layout,4);
                int zoffset = BitConv.FromInt32(entry.Layout,8);
                yield return new Position(xoffset,yoffset,zoffset);
                int x2 = BitConv.FromInt32(entry.Layout,12);
                int y2 = BitConv.FromInt32(entry.Layout,16);
                int z2 = BitConv.FromInt32(entry.Layout,20);
                yield return new Position(x2 + xoffset, y2 + yoffset, z2 + zoffset);
                foreach (OldEntity entity in entry.Entities)
                {
                    foreach (EntityPosition position in entity.Positions)
                    {
                        int x = (entity.Type != 34 ? position.X : position.X + 50) + xoffset;
                        int y = (entity.Type != 34 ? position.Y : position.Y + 50) + yoffset;
                        int z = (entity.Type != 34 ? position.Z : position.Z + 50) + zoffset;
                        yield return new Position(x,y,z);
                    }
                }
                foreach (OldCamera camera in entry.Cameras)
                {
                    foreach (OldCameraPosition position in camera.Positions)
                    {
                        int x = position.X + xoffset;
                        int y = position.Y + yoffset;
                        int z = position.Z + zoffset;
                        yield return new Position(x,y,z);
                    }
                }
            }
        }

        protected override int CameraRangeMinimum => 800;

        protected override bool IsInputKey(Keys keyData)
        {
            bool? settingsinput = common.IsInputKey(keyData);
            if (settingsinput != null)
                return settingsinput.Value;
            switch (keyData)
            {
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            common.OnKeyDown(e);
            switch (e.KeyCode)
            {
            }
        }

        protected override void RenderObjects()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 1.0f);
            RenderEntry(entry,ref common.OctreeDisplayLists[0]);
            GL.Enable(EnableCap.PolygonStipple);
            for (int i = 0; i < linkedentries.Length; i++)
            {
                OldZoneEntry linkedentry = linkedentries[i];
                if (linkedentry == entry)
                    continue;
                if (linkedentry == null)
                    continue;
                RenderLinkedEntry(linkedentry,ref common.OctreeDisplayLists[i + 1]);
            }
            GL.Disable(EnableCap.PolygonStipple);
            if (common.DeleteLists)
                common.DeleteLists = false;
            GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.RgbScale, 2.0f);
            GL.Enable(EnableCap.Texture2D);
            base.RenderObjects();
        }

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

        private void RenderSprite()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();
            GL.Rotate(-rotx,0,1,0);
            GL.Rotate(-roty,1,0,0);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex2(-50,+50);
            GL.TexCoord2(1,0);
            GL.Vertex2(+50,+50);
            GL.TexCoord2(1,1);
            GL.Vertex2(+50,-50);
            GL.TexCoord2(0,1);
            GL.Vertex2(-50,-50);
            GL.End();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderPickup(int subtype)
        {
            GL.Translate(0,50,0);
            GL.Color3(Color.White);
            LoadPickupTexture(subtype);
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();
            GL.Rotate(-rotx,0,1,0);
            GL.Rotate(-roty,1,0,0);
            ScalePickup(subtype);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0,0);
            GL.Vertex2(-50,+50);
            GL.TexCoord2(1,0);
            GL.Vertex2(+50,+50);
            GL.TexCoord2(1,1);
            GL.Vertex2(+50,-50);
            GL.TexCoord2(0,1);
            GL.Vertex2(-50,-50);
            GL.End();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

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

        private void LoadPickupTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // Lime
                    LoadTexture(OldResources.LimeTexture);
                    break;
                case 1: // Coconut
                    LoadTexture(OldResources.CoconutTexture);
                    break;
                case 2: // Pineapple
                    LoadTexture(OldResources.PineappleTexture);
                    break;
                case 3: // Strawberry
                    LoadTexture(OldResources.StrawberryTexture);
                    break;
                case 4: // Mango
                    LoadTexture(OldResources.MangoTexture);
                    break;
                case 5: // Life
                    LoadTexture(OldResources.LifeTexture);
                    break;
                case 6: // Mask
                    LoadTexture(OldResources.MaskTexture);
                    break;
                case 7: // Lemon
                    LoadTexture(OldResources.LemonTexture);
                    break;
                case 8: // YYY
                    LoadTexture(OldResources.YYYTexture);
                    break;
                case 11: // Grape
                    LoadTexture(OldResources.GrapeTexture);
                    break;
                case 16: // Apple
                    LoadTexture(OldResources.AppleTexture);
                    break;
                case 18: // Cortex
                    LoadTexture(OldResources.CortexTexture);
                    break;
                case 19: // Brio
                    LoadTexture(OldResources.BrioTexture);
                    break;
                case 20: // Tawna
                    LoadTexture(OldResources.TawnaTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownPickupTexture);
                    break;
            }
        }

        private void ScalePickup(int subtype)
        {
            switch (subtype)
            {
                case 0: // Lime
                case 1: // Coconut
                case 4: // Mango
                case 7: // Lemon
                case 8: // YYY
                    GL.Scale(0.7f,0.7f,1);
                    break;
                case 2: // Pineapple
                    GL.Scale(0.7f,1.4f,1);
                    break;
                case 3: // Strawberry
                    GL.Scale(0.8f,0.8f,1);
                    break;
                case 5: // Life
                case 6: // Mask
                case 18: // Cortex
                case 20: // Tawna
                    GL.Scale(1.8f,1.125f,1);
                    break;
                case 16: // Apple
                    GL.Scale(0.675f,0.84375f,1);
                    break;
                case 19: // Brio
                    GL.Scale(1.8f,1.8f,1);
                    break;
            }
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
    }
}
