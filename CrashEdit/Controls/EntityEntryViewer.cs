using Crash;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class EntityEntryViewer : SceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static EntityEntryViewer()
        {
            stipplea = new byte [128];
            stippleb = new byte [128];
            for (int i = 0;i < 128;i += 8)
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

        private EntityEntry entry;
        private EntityEntry[] linkedentries;

        public EntityEntryViewer(EntityEntry entry,SceneryEntry[] linkedsceneryentries,EntityEntry[] linkedentries) : base(linkedsceneryentries)
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
                foreach (Entity entity in entry.Entities)
                {
                    if (entity.Name != null)
                    {
                        foreach (EntityPosition position in entity.Positions)
                        {
                            int x = position.X * 4 + xoffset;
                            int y = position.Y * 4 + yoffset;
                            int z = position.Z * 4 + zoffset;
                            yield return new Position(x,y,z);
                        }
                    }
                }
            }
        }

        protected override void RenderObjects()
        {
            RenderEntry(entry);
            GL.Enable(EnableCap.PolygonStipple);
            GL.PolygonStipple(stipplea);
            base.RenderObjects();
            GL.PolygonStipple(stippleb);
            foreach (EntityEntry linkedentry in linkedentries)
            {
                if (linkedentry == entry)
                    continue;
                RenderEntry(linkedentry);
            }
            GL.Disable(EnableCap.PolygonStipple);
        }

        private void RenderEntry(EntityEntry entry)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            GL.Scale(4,4,4);
            foreach (Entity entity in entry.Entities)
            {
                if (entity.Name != null)
                {
                    RenderEntity(entity);
                }
            }
            GL.PopMatrix();
        }

        private void RenderEntity(Entity entity)
        {
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                GL.PushMatrix();
                GL.Translate(position.X,position.Y,position.Z);
                switch (entity.Type)
                {
                    case 0x3:
                        if (entity.Subtype.HasValue)
                        {
                            RenderPickup(entity.Subtype.Value);
                        }
                        break;
                    case 0x22:
                        if (entity.Subtype.HasValue)
                        {
                            RenderBox(entity.Subtype.Value);
                        }
                        break;
                    default:
                        GL.Color3(Color.White);
                        LoadTexture(Resources.PointTexture);
                        RenderSprite();
                        break;
                }
                GL.PopMatrix();
            }
            else
            {
                GL.Color3(Color.Blue);
                GL.Begin(BeginMode.LineStrip);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Vertex3(position.X,position.Y,position.Z);
                }
                GL.End();
                GL.Color3(Color.Red);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Color3(Color.Red);
                    LoadTexture(Resources.PointTexture);
                    GL.PushMatrix();
                    GL.Translate(position.X,position.Y,position.Z);
                    RenderSprite();
                    GL.PopMatrix();
                }
            }
        }

        private void RenderSprite()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();
            GL.Rotate(-rotx,0,1,0);
            GL.Rotate(-roty,1,0,0);
            GL.Begin(BeginMode.Quads);
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
            GL.Color3(Color.White);
            LoadPickupTexture(subtype);
            RenderSprite();
        }

        private void RenderBox(int subtype)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Color3(Color.White);
            LoadBoxSideTexture(subtype);
            GL.PushMatrix();
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.PopMatrix();
            LoadBoxTopTexture(subtype);
            GL.PushMatrix();
            RenderBoxFace();
            GL.Rotate(90,1,0,0);
            RenderBoxFace();
            GL.Rotate(-180,1,0,0);
            RenderBoxFace();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);
        }

        private void RenderBoxFace()
        {
            GL.Begin(BeginMode.Quads);
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
                    LoadTexture(Resources.LifeTexture);
                    break;
                case 6: // Mask
                    LoadTexture(Resources.MaskTexture);
                    break;
                case 16: // Apple
                    LoadTexture(Resources.AppleTexture);
                    break;
                default:
                    LoadTexture(Resources.UnknownPickupTexture);
                    break;
            }
        }

        private void LoadBoxTopTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                    LoadTexture(Resources.TNTTopTexture);
                    break;
                case 2: // Normal
                case 3: // Arrow
                case 6: // Apple
                case 8: // Life
                case 9: // Mask
                case 10: // Question Mark
                    LoadTexture(Resources.BoxTexture);
                    break;
                case 4: // Checkpoint
                    LoadTexture(Resources.CheckpointTexture);
                    break;
                case 5: // Iron
                case 7: // Activator
                case 15: // Iron Arrow
                    LoadTexture(Resources.IronBoxTexture);
                    break;
                case 18: // Nitro
                    LoadTexture(Resources.NitroTopTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(Resources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(Resources.DetonatorBoxTopTexture);
                    break;
                default:
                    LoadTexture(Resources.UnknownBoxTopTexture);
                    break;
            }
        }

        private void LoadBoxSideTexture(int subtype)
        {
            switch (subtype)
            {
                case 0: // TNT
                    LoadTexture(Resources.TNTTexture);
                    break;
                case 2: // Normal
                    LoadTexture(Resources.BoxTexture);
                    break;
                case 3: // Arrow
                    LoadTexture(Resources.ArrowBoxTexture);
                    break;
                case 4: // Checkpoint
                    LoadTexture(Resources.CheckpointTexture);
                    break;
                case 5: // Iron
                    LoadTexture(Resources.IronBoxTexture);
                    break;
                case 6: // Apple
                    LoadTexture(Resources.AppleBoxTexture);
                    break;
                case 7: // Activator
                    LoadTexture(Resources.ActivatorBoxTexture);
                    break;
                case 8: // Life
                    LoadTexture(Resources.LifeBoxTexture);
                    break;
                case 9: // Mask
                    LoadTexture(Resources.MaskBoxTexture);
                    break;
                case 10: // Question Mark
                    LoadTexture(Resources.QuestionMarkBoxTexture);
                    break;
                case 15: // Iron Arrow
                    LoadTexture(Resources.IronArrowBoxTexture);
                    break;
                case 18: // Nitro
                    LoadTexture(Resources.NitroTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(Resources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(Resources.DetonatorBoxTexture);
                    break;
                default:
                    LoadTexture(Resources.UnknownBoxTexture);
                    break;
            }
        }
    }
}
