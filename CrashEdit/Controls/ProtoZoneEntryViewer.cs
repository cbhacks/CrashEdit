using Crash;
using System.Drawing;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryViewer : ProtoSceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static ProtoZoneEntryViewer()
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

        private ProtoZoneEntry entry;
        private ProtoZoneEntry[] linkedentries;

        public ProtoZoneEntryViewer(ProtoZoneEntry entry,ProtoSceneryEntry[] linkedsceneryentries,ProtoZoneEntry[] linkedentries) : base(linkedsceneryentries)
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
                foreach (ProtoEntity entity in entry.Entities)
                {
                    int x = entity.StartX.Value + xoffset;
                    int y = entity.StartY.Value + yoffset;
                    int z = entity.StartZ.Value + zoffset;
                    yield return new Position(x,y,z);
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
            foreach (ProtoZoneEntry linkedentry in linkedentries)
            {
                if (linkedentry == entry)
                    continue;
                if (linkedentry == null)
                    continue;
                RenderLinkedEntry(linkedentry);
            }
            GL.Disable(EnableCap.PolygonStipple);
        }

        private void RenderEntry(ProtoZoneEntry entry)
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
            foreach (ProtoEntity entity in entry.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in entry.Cameras)
            {
                RenderCamera(camera);
            }
            GL.PopMatrix();
        }

        private void RenderLinkedEntry(ProtoZoneEntry entry)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2, 0);
            int yoffset = BitConv.FromInt32(entry.Unknown2, 4);
            int zoffset = BitConv.FromInt32(entry.Unknown2, 8);
            GL.PushMatrix();
            GL.Translate(xoffset, yoffset, zoffset);
            GL.Scale(4, 4, 4);
            foreach (ProtoEntity entity in entry.Entities)
            {
                RenderEntity(entity);
            }
            foreach (OldCamera camera in entry.Cameras)
            {
                RenderCamera(camera);
            }
            GL.PopMatrix();
        }

        private void RenderEntity(ProtoEntity entity)
        {
            if (entity.Index.Count == 1)
            {
                GL.PushMatrix();
                switch (entity.Type)
                {
                    case 0x3:
                        GL.Translate(entity.StartX.Value / 4,entity.StartY.Value / 4,entity.StartZ.Value / 4);
                        RenderPickup(entity.Subtype.Value);
                        break;
                    default:
                        GL.Translate(entity.StartX.Value / 4,entity.StartY.Value / 4,entity.StartZ.Value / 4);
                        GL.Color3(Color.White);
                        LoadTexture(OldResources.PointTexture);
                        RenderSprite();
                        break;
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
            RenderSprite();
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
    }
}

