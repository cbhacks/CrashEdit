using Crash;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace CrashEdit
{
    public sealed class NewZoneEntryViewer : NewSceneryEntryViewer
    {
        private static byte[] stipplea;
        private static byte[] stippleb;

        static NewZoneEntryViewer()
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

        private NewZoneEntry entry;
        private NewZoneEntry[] linkedentries;
        private bool renderoctree;
        private int[] octreedisplaylists;
        private Dictionary<short,Color> octreevalues;
        private int octreeselection;
        private bool deletelists;
        private bool polygonmode;
        private bool allentries;

        public NewZoneEntryViewer(NewZoneEntry entry,NewSceneryEntry[] linkedsceneryentries,NewZoneEntry[] linkedentries)
            : base(linkedsceneryentries)
        {
            this.entry = entry;
            this.linkedentries = linkedentries;
            renderoctree = false;
            octreedisplaylists = new int[linkedentries.Length + 1];
            for (int i = 0; i < octreedisplaylists.Length; i++)
            {
                octreedisplaylists[i] = -1;
            }
            octreevalues = new Dictionary<short,Color>();
            octreeselection = -1;
            deletelists = false;
            polygonmode = false;
            allentries = false;
        }

        protected override int CameraRangeMargin
        {
            get { return 3200; }
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                int xoffset = BitConv.FromInt32(entry.Unknown2,0);
                int yoffset = BitConv.FromInt32(entry.Unknown2,4);
                int zoffset = BitConv.FromInt32(entry.Unknown2,8);
                yield return new Position(xoffset,yoffset,zoffset);
                int x2 = BitConv.FromInt32(entry.Unknown2,12);
                int y2 = BitConv.FromInt32(entry.Unknown2,16);
                int z2 = BitConv.FromInt32(entry.Unknown2,20);
                yield return new Position(x2 + xoffset,y2 + yoffset,z2 + zoffset);
                foreach (Entity entity in entry.Entities)
                {
                    if (entry.Entities.IndexOf(entity) % 3 == 0 || entity.ID != null)
                    {
                        float scale = 1;
                        if (entity.Scaling.HasValue)
                            scale = (1 << entity.Scaling.Value) / 4;
                        foreach (EntityPosition position in entity.Positions)
                        {
                            float x = position.X * scale + xoffset;
                            float y = position.Y * scale + yoffset;
                            float z = position.Z * scale + zoffset;
                            yield return new Position(x,y,z);
                        }
                    }
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.X:
                case Keys.C:
                case Keys.R:
                case Keys.V:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.X:
                    renderoctree = !renderoctree;
                    break;
                case Keys.C:
                    {
                        Form frm = new Form();
                        ListView lst = new ListView();
                        lst.Dock = DockStyle.Fill;
                        foreach (KeyValuePair<short,Color> color in octreevalues)
                        {
                            ListViewItem lsi = new ListViewItem();
                            lsi.Text = color.Key.ToString("X4");
                            lsi.BackColor = color.Value;
                            lsi.ForeColor = color.Value.GetBrightness() >= 0.5 ? Color.Black : Color.White;
                            lsi.Tag = color.Key;
                            lst.Items.Add(lsi);
                        }
                        lst.SelectedIndexChanged += delegate (object sender,EventArgs ee)
                        {
                            if (lst.SelectedItems.Count == 0)
                            {
                                octreeselection = -1;
                            }
                            else
                            {
                                octreeselection = (ushort)(short)lst.SelectedItems[0].Tag;
                            }
                        };
                        frm.Controls.Add(lst);
                        frm.Show();
                    }
                    break;
                case Keys.R:
                    deletelists = true;
                    break;
                case Keys.V:
                    polygonmode = !polygonmode;
                    break;
                case Keys.F:
                    allentries = !allentries;
                    break;
            }
        }

        protected override void RenderObjects()
        {
            RenderEntry(entry,ref octreedisplaylists[0]);
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            base.RenderObjects();
            GL.Enable(EnableCap.PolygonStipple);
            GL.PolygonStipple(stippleb);
            for (int i = 0; i < linkedentries.Length; i++)
            {
                NewZoneEntry linkedentry = linkedentries[i];
                if (linkedentry == entry)
                    continue;
                if (linkedentry == null)
                    continue;
                RenderLinkedEntry(linkedentry,ref octreedisplaylists[i + 1]);
            }
            GL.Disable(EnableCap.PolygonStipple);
        }

        private void RenderEntry(NewZoneEntry entry,ref int octreedisplaylist)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            int x2 = BitConv.FromInt32(entry.Unknown2,12);
            int y2 = BitConv.FromInt32(entry.Unknown2,16);
            int z2 = BitConv.FromInt32(entry.Unknown2,20);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            if (deletelists)
            {
                GL.DeleteLists(octreedisplaylist,1);
                octreedisplaylist = -1;
                deletelists = false;
            }
            if (renderoctree)
            {
                if (polygonmode)
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
                if (octreedisplaylist == -1)
                {
                    octreedisplaylist = GL.GenLists(1);
                    GL.NewList(octreedisplaylist,ListMode.CompileAndExecute);
                    GL.PushMatrix();
                    int xmax = (ushort)BitConv.FromInt16(entry.Unknown2,0x1E);
                    int ymax = (ushort)BitConv.FromInt16(entry.Unknown2,0x20);
                    int zmax = (ushort)BitConv.FromInt16(entry.Unknown2,0x22);
                    RenderOctree(entry.Unknown2,0x1C,0,0,0,x2,y2,z2,xmax,ymax,zmax);
                    GL.PopMatrix();
                    GL.EndList();
                }
                else
                {
                    GL.CallList(octreedisplaylist);
                }
                GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
            }
            GL.Scale(4,4,4);
            int xdepth = BitConv.FromInt32(entry.Unknown2,12);
            int ydepth = BitConv.FromInt32(entry.Unknown2,16);
            int zdepth = BitConv.FromInt32(entry.Unknown2,20);
            GL.Color3(Color.White);
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(0,0,0);
            GL.Vertex3(xdepth / 4,0,0);
            GL.Vertex3(xdepth / 4,ydepth / 4,0);
            GL.Vertex3(0,ydepth / 4,0);
            GL.Vertex3(0,0,0);
            GL.Vertex3(0,0,zdepth / 4);
            GL.Vertex3(xdepth / 4,0,zdepth / 4);
            GL.Vertex3(xdepth / 4,ydepth / 4,zdepth / 4);
            GL.Vertex3(0,ydepth / 4,zdepth / 4);
            GL.Vertex3(0,0,zdepth / 4);
            GL.Vertex3(xdepth / 4,0,zdepth / 4);
            GL.Vertex3(xdepth / 4,0,0);
            GL.Vertex3(xdepth / 4,ydepth / 4,0);
            GL.Vertex3(xdepth / 4,ydepth / 4,zdepth / 4);
            GL.Vertex3(0,ydepth / 4,zdepth / 4);
            GL.Vertex3(0,ydepth / 4,0);
            GL.End();
            foreach (Entity entity in entry.Entities)
            {
                if (entity.ID != null)
                {
                    RenderEntity(entity,false);
                }
                else if (entry.Entities.IndexOf(entity) % 3 == 0)
                {
                    RenderEntity(entity,true);
                }
            }
            GL.PopMatrix();
        }

        private void RenderLinkedEntry(NewZoneEntry entry,ref int octreedisplaylist)
        {
            int xoffset = BitConv.FromInt32(entry.Unknown2,0);
            int yoffset = BitConv.FromInt32(entry.Unknown2,4);
            int zoffset = BitConv.FromInt32(entry.Unknown2,8);
            int x2 = BitConv.FromInt32(entry.Unknown2,12);
            int y2 = BitConv.FromInt32(entry.Unknown2,16);
            int z2 = BitConv.FromInt32(entry.Unknown2,20);
            GL.PushMatrix();
            GL.Translate(xoffset,yoffset,zoffset);
            if (allentries)
            {
                GL.PolygonStipple(stippleb);
                if (deletelists)
                {
                    GL.DeleteLists(octreedisplaylist,1);
                    octreedisplaylist = -1;
                    deletelists = false;
                }
                if (renderoctree)
                {
                    if (polygonmode)
                        GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
                    if (octreedisplaylist == -1)
                    {
                        octreedisplaylist = GL.GenLists(1);
                        GL.NewList(octreedisplaylist,ListMode.CompileAndExecute);
                        GL.PushMatrix();
                        int xmax = (ushort)BitConv.FromInt16(entry.Unknown2,0x1E);
                        int ymax = (ushort)BitConv.FromInt16(entry.Unknown2,0x20);
                        int zmax = (ushort)BitConv.FromInt16(entry.Unknown2,0x22);
                        RenderOctree(entry.Unknown2,0x1C,0,0,0,x2,y2,z2,xmax,ymax,zmax);
                        GL.PopMatrix();
                        GL.EndList();
                    }
                    else
                    {
                        GL.CallList(octreedisplaylist);
                    }
                    GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
                }
            }
            GL.Scale(4,4,4);
            foreach (Entity entity in entry.Entities)
            {
                if (entity.ID != null)
                {
                    RenderEntity(entity,false);
                }
                else if (entry.Entities.IndexOf(entity) % 3 == 0)
                {
                    RenderEntity(entity,true);
                }
            }
            GL.PopMatrix();
        }

        private void RenderOctree(byte[] data,int offset,double x,double y,double z,double w,double h,double d,int xmax,int ymax,int zmax)
        {
            int value = (ushort)BitConv.FromInt16(data,offset);
            if ((value & 1) != 0)
            {
                Color color;
                if (!octreevalues.TryGetValue((short)value,out color))
                {
                    byte[] colorbuf = new byte[3];
                    Random random = new Random(value);
                    random.NextBytes(colorbuf);
                    color = Color.FromArgb(255,colorbuf[0],colorbuf[1],colorbuf[2]);
                    octreevalues.Add((short)value,color);
                }
                if (octreeselection != -1 && octreeselection != value)
                    return;
                Color c1 = Color.FromArgb((color.R + 4) % 256,(color.G + 4) % 256,(color.B + 4) % 256);
                Color c2 = Color.FromArgb((color.R + 8) % 256,(color.G + 8) % 256,(color.B + 8) % 256);
                Color c3 = Color.FromArgb((color.R + 12) % 256,(color.G + 12) % 256,(color.B + 12) % 256);
                Color c4 = Color.FromArgb((color.R + 16) % 256,(color.G + 16) % 256,(color.B + 16) % 256);
                GL.Color3(color);
                GL.Begin(PrimitiveType.Quads);
                // Bottom
                GL.Color3(c1);
                GL.Vertex3(x + 0,y + 0,z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w,y + 0,z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w,y + 0,z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0,y + 0,z + d);

                // Top
                GL.Color3(c1);
                GL.Vertex3(x + 0,y + h,z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w,y + h,z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w,y + h,z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0,y + h,z + d);

                // Left
                GL.Color3(c1);
                GL.Vertex3(x + 0,y + 0,z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + 0,y + h,z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + 0,y + h,z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0,y + 0,z + d);

                // Right
                GL.Color3(c1);
                GL.Vertex3(x + w,y + 0,z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w,y + h,z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w,y + h,z + d);
                GL.Color3(c4);
                GL.Vertex3(x + w,y + 0,z + d);

                // Front
                GL.Color3(c1);
                GL.Vertex3(x + 0,y + 0,z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w,y + 0,z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w,y + h,z + 0);
                GL.Color3(c4);
                GL.Vertex3(x + 0,y + h,z + 0);

                // Back
                GL.Color3(c1);
                GL.Vertex3(x + 0,y + 0,z + d);
                GL.Color3(c2);
                GL.Vertex3(x + w,y + 0,z + d);
                GL.Color3(c3);
                GL.Vertex3(x + w,y + h,z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0,y + h,z + d);
                GL.End();
            }
            else if (value != 0)
            {
                RenderOctreeX(data,ref value,x,y,z,w,h,d,xmax,ymax,zmax);
            }
        }

        private void RenderOctreeX(byte[] data,ref int offset,double x,double y,double z,double w,double h,double d,int xmax,int ymax,int zmax)
        {
            if (xmax > 0)
            {
                RenderOctreeY(data,ref offset,x + 0 / 2,y,z,w / 2,h,d,xmax - 1,ymax,zmax);
                RenderOctreeY(data,ref offset,x + w / 2,y,z,w / 2,h,d,xmax - 1,ymax,zmax);
            }
            else
            {
                RenderOctreeY(data,ref offset,x,y,z,w,h,d,xmax - 1,ymax,zmax);
            }
        }
        private void RenderOctreeY(byte[] data,ref int offset,double x,double y,double z,double w,double h,double d,int xmax,int ymax,int zmax)
        {
            if (ymax > 0)
            {
                RenderOctreeZ(data,ref offset,x,y + 0 / 2,z,w,h / 2,d,xmax,ymax - 1,zmax);
                RenderOctreeZ(data,ref offset,x,y + h / 2,z,w,h / 2,d,xmax,ymax - 1,zmax);
            }
            else
            {
                RenderOctreeZ(data,ref offset,x,y,z,w,h,d,xmax,ymax - 1,zmax);
            }
        }

        private void RenderOctreeZ(byte[] data,ref int offset,double x,double y,double z,double w,double h,double d,int xmax,int ymax,int zmax)
        {
            if (zmax > 0)
            {
                RenderOctree(data,offset,x,y,z + 0 / 2,w,h,d / 2,xmax,ymax,zmax - 1);
                offset += 2;
                RenderOctree(data,offset,x,y,z + d / 2,w,h,d / 2,xmax,ymax,zmax - 1);
                offset += 2;
            }
            else
            {
                RenderOctree(data,offset,x,y,z,w,h,d,xmax,ymax,zmax - 1);
                offset += 2;
            }
        }

        private void RenderEntity(Entity entity,bool camera)
        {
            float scale = 1;
            if (entity.Scaling.HasValue)
                scale = (1 << entity.Scaling.Value) / 4.0f;
            if (camera)
                GL.PolygonStipple(stippleb);
            else
                GL.PolygonStipple(stipplea);
            if (entity.Positions.Count == 1)
            {
                EntityPosition position = entity.Positions[0];
                GL.PushMatrix();
                if (camera)
                    GL.Scale(0.25,0.25,0.25);
                GL.Translate(position.X * scale,position.Y * scale,position.Z * scale);
                if (camera)
                    GL.Scale(4,4,4);
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
                        if (camera)
                            GL.Color3(Color.Yellow);
                        else
                            GL.Color3(Color.White);
                        LoadTexture(OldResources.PointTexture);
                        RenderSprite();
                        break;
                }
                GL.PopMatrix();
            }
            else
            {
                if (camera)
                    GL.Color3(Color.Green);
                else
                    GL.Color3(Color.Blue);
                GL.PushMatrix();
                if (camera)
                    GL.Scale(0.25,0.25,0.25);
                GL.Begin(PrimitiveType.LineStrip);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.Vertex3(position.X * scale,position.Y * scale,position.Z * scale);
                }
                GL.End();
                if (camera)
                    GL.Color3(Color.Yellow);
                else
                    GL.Color3(Color.Red);
                LoadTexture(OldResources.PointTexture);
                foreach (EntityPosition position in entity.Positions)
                {
                    GL.PushMatrix();
                    GL.Translate(position.X * scale,position.Y * scale,position.Z * scale);
                    if (camera)
                        GL.Scale(4,4,4);
                    RenderSprite();
                    GL.PopMatrix();
                }
                GL.PopMatrix();
            }
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
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
            GL.Rotate(90,0,1,0);
            RenderBoxFace();
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
                case 18: // Nitro
                    LoadTexture(OldResources.NitroTopTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(OldResources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(OldResources.DetonatorBoxTopTexture);
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
                case 18: // Nitro
                    LoadTexture(OldResources.NitroTexture);
                    break;
                case 23: // Bodyslam
                    LoadTexture(OldResources.BodyslamBoxTexture);
                    break;
                case 24: // Detonator
                    LoadTexture(OldResources.DetonatorBoxTexture);
                    break;
                default:
                    LoadTexture(OldResources.UnknownBoxTexture);
                    break;
            }
        }
    }
}
