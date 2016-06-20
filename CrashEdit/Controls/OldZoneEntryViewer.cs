using Crash;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class OldZoneEntryViewer : OldSceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static OldZoneEntryViewer()
        {
            stipplea = new byte [128];
            stippleb = new byte [128];
            for (int i = 0;i < 128;i += 8)
            {
                stipplea[i + 0] = 0xFF;
                stipplea[i + 1] = 0xFF;
                stipplea[i + 2] = 0xFF;
                stipplea[i + 3] = 0xFF;
                stipplea[i + 4] = 0xFF;
                stipplea[i + 5] = 0xFF;
                stipplea[i + 6] = 0xFF;
                stipplea[i + 7] = 0xFF;
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

        public OldZoneEntryViewer(OldZoneEntry entry,OldSceneryEntry[] linkedsceneryentries,OldZoneEntry[] linkedentries) : base(linkedsceneryentries)
        {
            this.entry = entry;
            this.linkedentries = linkedentries;
        }

        protected override int CameraRangeMargin
        {
            get { return 1600; }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                int xoffset = BitConv.FromInt32(entry.Unknown2,0);
                int yoffset = BitConv.FromInt32(entry.Unknown2,4);
                int zoffset = BitConv.FromInt32(entry.Unknown2,8);
                foreach (OldEntity entity in entry.Entities)
                {
                    foreach (EntityPosition position in entity.Index)
                    {
                        int x = position.X * 4 + xoffset;
                        int y = position.Y * 4 + yoffset;
                        int z = position.Z * 4 + zoffset;
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

        protected override void RenderObjects()
        {
            RenderEntry(entry);
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            GL.Enable(EnableCap.PolygonStipple);
            GL.PolygonStipple(stipplea);
            base.RenderObjects();
            GL.PolygonStipple(stippleb);
            foreach (OldZoneEntry linkedentry in linkedentries)
            {
                if (linkedentry == entry)
                    continue;
                if (linkedentry == null)
                    continue;
                RenderLinkedEntry(linkedentry);
            }
            GL.Disable(EnableCap.PolygonStipple);
        }

        private void RenderEntry(OldZoneEntry entry)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            GL.Scale(4, 4, 4);
            int xdepth = BitConv.FromInt32(entry.Unknown2, 12);
            int ydepth = BitConv.FromInt32(entry.Unknown2, 16);
            int zdepth = BitConv.FromInt32(entry.Unknown2, 20);
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(xdepth / 4, 0, 0);
            GL.Vertex3(xdepth / 4, ydepth / 4, 0);
            GL.Vertex3(0, ydepth / 4, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, zdepth / 4);
            GL.Vertex3(xdepth / 4, 0, zdepth / 4);
            GL.Vertex3(xdepth / 4, ydepth / 4, zdepth / 4);
            GL.Vertex3(0, ydepth / 4, zdepth / 4);
            GL.Vertex3(0, 0, zdepth / 4);
            GL.Vertex3(xdepth / 4, 0, zdepth / 4);
            GL.Vertex3(xdepth / 4, 0, 0);
            GL.Vertex3(xdepth / 4, ydepth / 4, 0);
            GL.Vertex3(xdepth / 4, ydepth / 4, zdepth / 4);
            GL.Vertex3(0, ydepth / 4, zdepth / 4);
            GL.Vertex3(0, ydepth / 4, 0);
            GL.End();
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

        private void RenderLinkedEntry(OldZoneEntry entry)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2, 0);
            int yoffset = BitConv.FromInt32(entry.Unknown2, 4);
            int zoffset = BitConv.FromInt32(entry.Unknown2, 8);
            GL.PushMatrix();
            GL.Translate(xoffset, yoffset, zoffset);
            GL.Scale(4, 4, 4);
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
            if (entity.Index.Count == 1)
            {
                EntityPosition position = entity.Index[0];
                GL.PushMatrix();
                switch (entity.Type)
                {
                    case 0x3:
                        GL.Translate(position.X,position.Y,position.Z);
                        RenderPickup(entity.Subtype.Value);
                        break;
                    case 0x22:
                        GL.Translate(position.X + 50,position.Y,position.Z + 50);
                        RenderBox(entity.Subtype.Value);
                        break;
                    default:
                        GL.Translate(position.X,position.Y,position.Z);
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
                foreach (EntityPosition position in entity.Index)
                {
                    GL.Vertex3(position.X,position.Y,position.Z);
                }
                GL.End();
                GL.Color3(Color.Red);
                LoadTexture(OldResources.PointTexture);
                foreach (EntityPosition position in entity.Index)
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
            GL.Color3(Color.Blue);
            GL.PushMatrix();
            GL.Begin(PrimitiveType.LineStrip);
            foreach (OldCameraPosition position in camera.Positions)
            {
                GL.Vertex3(position.X / 4,position.Y / 4,position.Z / 4);
            }
            GL.End();
            GL.Color3(Color.Yellow);
            LoadTexture(OldResources.PointTexture);
            foreach (OldCameraPosition position in camera.Positions)
            {
                GL.PushMatrix();
                GL.Translate(position.X / 4,position.Y / 4,position.Z / 4);
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
            RenderSprite();
        }

        private void RenderBox(int subtype)
        {
            GL.Translate(0,50,0);
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.White);
            LoadBoxSideTexture(subtype);
            GL.PushMatrix();
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(180,0,1,0);
            RenderBoxFace();
            //GL.Rotate(90,0,1,0);
            //RenderBoxFace();
            GL.PopMatrix();
            LoadBoxTopTexture(subtype);
            GL.PushMatrix();
            GL.Rotate(90,1,0,0);
            RenderBoxFace();
            GL.Rotate(180,1,0,0);
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
                case 5: // Life
                    LoadTexture(OldResources.LifeTexture);
                    break;
                case 6: // Mask
                    LoadTexture(OldResources.MaskTexture);
                    break;
                case 16: // Apple
                    LoadTexture(OldResources.AppleTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownPickupTexture);
                    break;
            }
        }

        private void LoadBoxTopTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                    LoadTexture(OldResources.TNTTopTexture);
                    break;
                case 2: // Normal
                case 3: // Arrow
                case 6: // Apple
                case 8: // Life
                case 9: // Mask
                case 10: // Question Mark
                    LoadTexture(OldResources.BoxTexture);
                    break;
                case 4: // Checkpoint
                    LoadTexture(OldResources.CheckpointTexture);
                    break;
                case 5: // Iron
                case 7: // Activator
                case 15: // Iron Arrow
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
                    LoadTexture(OldResources.TNTTexture);
                    break;
                case 2: // Normal
                    LoadTexture(OldResources.BoxTexture);
                    break;
                case 3: // Arrow
                    LoadTexture(OldResources.ArrowBoxTexture);
                    break;
                case 4: // Checkpoint
                    LoadTexture(OldResources.CheckpointTexture);
                    break;
                case 5: // Iron
                    LoadTexture(OldResources.IronBoxTexture);
                    break;
                case 6: // Apple
                    LoadTexture(OldResources.AppleBoxTexture);
                    break;
                case 7: // Activator
                    LoadTexture(OldResources.ActivatorBoxTexture);
                    break;
                case 8: // Life
                    LoadTexture(OldResources.LifeBoxTexture);
                    break;
                case 9: // Mask
                    LoadTexture(OldResources.MaskBoxTexture);
                    break;
                case 10: // Question Mark
                    LoadTexture(OldResources.QuestionMarkBoxTexture);
                    break;
                case 15: // Iron Arrow
                    LoadTexture(OldResources.IronArrowBoxTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownBoxTexture);
                    break;
            }
        }
    }
}

