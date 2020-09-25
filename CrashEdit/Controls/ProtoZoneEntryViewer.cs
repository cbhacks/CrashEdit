using Crash;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryViewer : ProtoSceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static ProtoZoneEntryViewer()
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

        private ProtoZoneEntry entry;
        private ProtoZoneEntry[] linkedentries;
        private CommonZoneEntryViewer common;
        private bool flipoctree;

        public string EntryName => entry.EName;

        public ProtoZoneEntryViewer(ProtoZoneEntry entry,ProtoSceneryEntry[] linkedsceneryentries,TextureChunk[][] texturechunks,ProtoZoneEntry[] linkedentries) : base(linkedsceneryentries,texturechunks)
        {
            this.entry = entry;
            this.linkedentries = linkedentries;
            common = new CommonZoneEntryViewer(linkedentries.Length + 1);
            flipoctree = false;
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
                yield return new Position(x2 + xoffset,y2 + yoffset,z2 + zoffset);
                foreach (ProtoEntity entity in entry.Entities)
                {
                    int x = entity.StartX/4 + xoffset;
                    int y = entity.StartY/4 + yoffset;
                    int z = entity.StartZ/4 + zoffset;
                    yield return new Position(x,y,z);
                    foreach (ProtoEntityPosition delta in entity.Deltas)
                    {
                        x += delta.X*2;
                        y += delta.Y*2;
                        z += delta.Z*2;
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
                case Keys.O:
                    return true;
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
                case Keys.O:
                    flipoctree = !flipoctree;
                    common.DeleteLists = true;
                    break;
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
                ProtoZoneEntry linkedentry = linkedentries[i];
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

        private void RenderEntry(ProtoZoneEntry entry,ref int octreedisplaylist)
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
                    if (ymax == 0) ymax = xmax;
                    int zmax = (ushort)BitConv.FromInt16(entry.Layout,0x22);
                    if (zmax == 0) zmax = ymax;
                    common.RenderOctree(entry.Layout,0x1C,0,flipoctree ? -y2 : 0,flipoctree ? -z2 : 0,x2,y2,z2,xmax,ymax,zmax);
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

        private void RenderLinkedEntry(ProtoZoneEntry entry,ref int octreedisplaylist)
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
                GL.Disable(EnableCap.PolygonStipple);
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
                        if (ymax == 0) ymax = xmax;
                        int zmax = (ushort)BitConv.FromInt16(entry.Layout,0x22);
                        if (zmax == 0) zmax = ymax;
                        common.RenderOctree(entry.Layout,0x1C,0,flipoctree ? -y2 : 0,flipoctree ? -z2 : 0,x2,y2,z2,xmax,ymax,zmax);
                        GL.PopMatrix();
                        GL.EndList();
                    }
                    else
                    {
                        GL.CallList(octreedisplaylist);
                    }
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
                }
                GL.Enable(EnableCap.PolygonStipple);
            }
            GL.Scale(4,4,4);
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
            GL.PolygonStipple(stipplea);
            if (entity.Deltas.Count == 0)
            {
                GL.PushMatrix();
                GL.Translate(entity.StartX/4,entity.StartY/4,entity.StartZ/4);
                switch (entity.Type)
                {
                    case 0x3:
                        RenderPickup(entity.Subtype);
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
                int curx = entity.StartX / 4;
                int cury = entity.StartY / 4;
                int curz = entity.StartZ / 4;
                GL.Vertex3(curx,cury,curz);
                foreach (ProtoEntityPosition delta in entity.Deltas)
                {
                    curx += delta.X*2;
                    cury += delta.Y*2;
                    curz += delta.Z*2;
                    GL.Vertex3(curx,cury,curz);
                }
                GL.End();
                curx = entity.StartX / 4;
                cury = entity.StartY / 4;
                curz = entity.StartZ / 4;
                GL.Color3(Color.Red);
                LoadTexture(OldResources.PointTexture);
                GL.PushMatrix();
                GL.Translate(curx,cury,curz);
                RenderSprite();
                GL.PopMatrix();
                GL.Vertex3(curx,cury,curz);
                foreach (ProtoEntityPosition delta in entity.Deltas)
                {
                    GL.PushMatrix();
                    curx += delta.X*2;
                    cury += delta.Y*2;
                    curz += delta.Z*2;
                    GL.Translate(curx,cury,curz);
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
