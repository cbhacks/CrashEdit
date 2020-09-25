using Crash;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public class CommonZoneEntryViewer
    {
        public Dictionary<short, Color> octreevalues;

        public bool EnableOctree { get; set; }
        public int[] OctreeDisplayLists { get; set; }
        public int OctreeSelection { get; set; }
        public bool DeleteLists { get; set; }
        public bool PolygonMode { get; set; }
        public bool AllEntries { get; set; }
        public Form OctreeForm { get; set; }

        public Entry CurrentEntry { get; set; }
        private string EntryName => CurrentEntry == null ? string.Empty : CurrentEntry.EName;

        public CommonZoneEntryViewer(int displaylistcount)
        {
            EnableOctree = false;
            OctreeDisplayLists = new int[displaylistcount];
            for (int i = 0; i < OctreeDisplayLists.Length; i++)
            {
                OctreeDisplayLists[i] = -1;
            }
            octreevalues = new Dictionary<short, Color>();
            OctreeSelection = -1;
            DeleteLists = false;
            PolygonMode = false;
            AllEntries = false;
        }

        public virtual bool? IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.X:
                case Keys.C:
                case Keys.R:
                case Keys.V:
                case Keys.F:
                    return true;
                default:
                    return null;
            }
        }

        public virtual void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.X:
                    EnableOctree = !EnableOctree;
                    break;
                case Keys.C:
                    if (OctreeForm == null || OctreeForm.IsDisposed)
                    {
                        OctreeForm = new Form();
                        OctreeForm.FormClosed += (object sender, FormClosedEventArgs ee) =>
                        {
                            OctreeSelection = -1;
                            DeleteLists = true;
                        };
                        UpdateOctreeFormList();
                        OctreeForm.Show();
                    }
                    else
                    {
                        OctreeForm.Select();
                    }
                    break;
                case Keys.R:
                    DeleteLists = true;
                    UpdateOctreeFormList();
                    break;
                case Keys.V:
                    PolygonMode = !PolygonMode;
                    break;
                case Keys.F:
                    AllEntries = !AllEntries;
                    UpdateOctreeFormList();
                    break;
            }
        }

        internal void UpdateOctreeFormList()
        {
            if (OctreeForm != null && !OctreeForm.IsDisposed)
            {
                OctreeForm.Controls.Clear();
                ListView lst = new ListView();
                lst.Dock = DockStyle.Fill;
                foreach (KeyValuePair<short, Color> color in octreevalues)
                {
                    ListViewItem lsi = new ListViewItem();
                    lsi.Text = string.Format("{2:X2}:{1:X2}:{0:X1}", color.Key >> 1 & 0x7, color.Key >> 4 & 0x3F, color.Key >> 10 & 0x3F);
                    lsi.BackColor = color.Value;
                    lsi.ForeColor = color.Value.GetBrightness() >= 0.5 ? Color.Black : Color.White;
                    lsi.Tag = color.Key;
                    lst.Items.Add(lsi);
                }
                lst.SelectedIndexChanged += delegate (object sender, EventArgs ee)
                {
                    if (lst.SelectedItems.Count == 0)
                    {
                        OctreeSelection = -1;
                    }
                    else
                    {
                        OctreeSelection = (ushort)(short)lst.SelectedItems[0].Tag;
                    }
                    DeleteLists = true;
                };
                OctreeForm.Controls.Add(lst);
            }
        }

        public void RenderOctree(byte[] data, int offset, double x, double y, double z, double w, double h, double d, int xmax, int ymax, int zmax)
        {
            int value = (ushort)BitConv.FromInt16(data, offset);
            if ((value & 1) != 0)
            {
                if (OctreeSelection != -1 && OctreeSelection != value)
                    return;
                Color color;
                if (!octreevalues.TryGetValue((short)value, out color))
                {
                    byte[] colorbuf = new byte[3];
                    Random random = new Random(value);
                    random.NextBytes(colorbuf);
                    color = Color.FromArgb(255, colorbuf[0], colorbuf[1], colorbuf[2]);
                    octreevalues.Add((short)value, color);
                }
                Color c1 = Color.FromArgb(Math.Min(color.R+ 0, 0xFF), Math.Min(color.G+ 0, 0xFF), Math.Min(color.B+ 0, 0xFF));
                Color c2 = Color.FromArgb(Math.Min(color.R+ 4, 0xFF), Math.Min(color.G+ 4, 0xFF), Math.Min(color.B+ 4, 0xFF));
                Color c3 = Color.FromArgb(Math.Min(color.R+ 8, 0xFF), Math.Min(color.G+ 8, 0xFF), Math.Min(color.B+ 8, 0xFF));
                Color c4 = Color.FromArgb(Math.Min(color.R+12, 0xFF), Math.Min(color.G+12, 0xFF), Math.Min(color.B+12, 0xFF));
                GL.Color3(color);
                GL.Begin(PrimitiveType.Quads);
                // Bottom
                GL.Color3(c1);
                GL.Vertex3(x + 0, y + 0, z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w, y + 0, z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w, y + 0, z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0, y + 0, z + d);

                // Top
                GL.Color3(c1);
                GL.Vertex3(x + 0, y + h, z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w, y + h, z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w, y + h, z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0, y + h, z + d);

                // Left
                GL.Color3(c1);
                GL.Vertex3(x + 0, y + 0, z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + 0, y + h, z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + 0, y + h, z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0, y + 0, z + d);

                // Right
                GL.Color3(c1);
                GL.Vertex3(x + w, y + 0, z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w, y + h, z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w, y + h, z + d);
                GL.Color3(c4);
                GL.Vertex3(x + w, y + 0, z + d);

                // Front
                GL.Color3(c1);
                GL.Vertex3(x + 0, y + 0, z + 0);
                GL.Color3(c2);
                GL.Vertex3(x + w, y + 0, z + 0);
                GL.Color3(c3);
                GL.Vertex3(x + w, y + h, z + 0);
                GL.Color3(c4);
                GL.Vertex3(x + 0, y + h, z + 0);

                // Back
                GL.Color3(c1);
                GL.Vertex3(x + 0, y + 0, z + d);
                GL.Color3(c2);
                GL.Vertex3(x + w, y + 0, z + d);
                GL.Color3(c3);
                GL.Vertex3(x + w, y + h, z + d);
                GL.Color3(c4);
                GL.Vertex3(x + 0, y + h, z + d);
                GL.End();
            }
            else if (value != 0)
            {
                if (value == offset) throw new ApplicationException(string.Format("RenderOctree: Infinitely-recursive node ({0})", EntryName));
                RenderOctreeX(data, ref value, x, y, z, w, h, d, xmax, ymax, zmax);
            }
        }

        internal void RenderOctreeX(byte[] data, ref int offset, double x, double y, double z, double w, double h, double d, int xmax, int ymax, int zmax)
        {
            if (xmax > 0)
            {
                RenderOctreeY(data, ref offset, x + 0 / 2, y, z, w / 2, h, d, xmax - 1, ymax, zmax);
                RenderOctreeY(data, ref offset, x + w / 2, y, z, w / 2, h, d, xmax - 1, ymax, zmax);
            }
            else
            {
                RenderOctreeY(data, ref offset, x, y, z, w, h, d, xmax - 1, ymax, zmax);
            }
        }

        internal void RenderOctreeY(byte[] data, ref int offset, double x, double y, double z, double w, double h, double d, int xmax, int ymax, int zmax)
        {
            if (ymax > 0)
            {
                RenderOctreeZ(data, ref offset, x, y + 0 / 2, z, w, h / 2, d, xmax, ymax - 1, zmax);
                RenderOctreeZ(data, ref offset, x, y + h / 2, z, w, h / 2, d, xmax, ymax - 1, zmax);
            }
            else
            {
                RenderOctreeZ(data, ref offset, x, y, z, w, h, d, xmax, ymax - 1, zmax);
            }
        }

        internal void RenderOctreeZ(byte[] data, ref int offset, double x, double y, double z, double w, double h, double d, int xmax, int ymax, int zmax)
        {
            if (zmax > 0)
            {
                RenderOctree(data, offset, x, y, z + 0 / 2, w, h, d / 2, xmax, ymax, zmax - 1);
                offset += 2;
                RenderOctree(data, offset, x, y, z + d / 2, w, h, d / 2, xmax, ymax, zmax - 1);
                offset += 2;
            }
            else
            {
                RenderOctree(data, offset, x, y, z, w, h, d, xmax, ymax, zmax - 1);
                offset += 2;
            }
        }
    }
}
