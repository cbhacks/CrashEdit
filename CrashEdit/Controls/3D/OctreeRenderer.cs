using Crash;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public class OctreeRenderer
    {
        public Dictionary<short, Color> octreevalues;

        public bool Enabled { get; set; }
        public int OctreeSelection { get; set; }
        public bool PolygonMode { get; set; }
        public bool AllEntries { get; set; }
        public Form OctreeForm { get; set; }
        private GLViewer Viewer { get; set; }

        public OctreeRenderer(GLViewer viewer)
        {
            Enabled = false;
            octreevalues = new Dictionary<short, Color>();
            OctreeSelection = -1;
            PolygonMode = false;
            AllEntries = false;
            Viewer = viewer;
        }

        public void RunLogic()
        {
            if (Viewer.KPress(Keys.X)) Enabled = !Enabled;
            if (Viewer.KPress(Keys.V)) PolygonMode = !PolygonMode;
            if (Viewer.KPress(Keys.F)) AllEntries = !AllEntries;
            if (Viewer.KPress(Keys.C))
            {
                if (OctreeForm == null || OctreeForm.IsDisposed)
                {
                    OctreeForm = new Form();
                    OctreeForm.FormClosed += (sender, e) =>
                    {
                        OctreeSelection = -1;
                    };
                    UpdateOctreeFormList();
                    OctreeForm.Show();
                }
                else
                {
                    OctreeForm.Select();
                }
            }
        }

        internal void UpdateOctreeFormList()
        {
            if (OctreeForm != null && !OctreeForm.IsDisposed)
            {
                OctreeForm.Controls.Clear();
                ListView lst = new();
                lst.Dock = DockStyle.Fill;
                foreach (KeyValuePair<short, Color> color in octreevalues)
                {
                    ListViewItem lsi = new();
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
                };
                OctreeForm.Controls.Add(lst);
            }
        }

        public void RenderOctreeFull(byte[] data, int start_offset, int x, int y, int z, int w, int h, int d, int xmax, int ymax, int zmax)
        {
            RenderOctree(data, start_offset, x, y, z, w, h, d, xmax, ymax, zmax);
        }

        public void RenderOctree(byte[] data, int offset, float x, float y, float z, float w, float h, float d, int xmax, int ymax, int zmax)
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
                Color4[] all_colors = new Color4[8];
                for (int i = 0; i < all_colors.Length; ++i)
                {
                    all_colors[i] = new(Math.Min(color.R + i * 3, 0xFF)/255f, Math.Min(color.G + i * 3, 0xFF) / 255f, Math.Min(color.B + i * 3, 0xFF) / 255f, 1f);
                }
                Viewer.AddBox(new Vector3(x, y, z), new Vector3(w, h, d), all_colors, !PolygonMode);
            }
            else if (value != 0)
            {
                if (value == offset) throw new ApplicationException(string.Format("RenderOctree: Infinitely-recursive node ({0})", Entry.NullEName));
                RenderOctreeX(data, ref value, x, y, z, w, h, d, xmax, ymax, zmax);
            }
        }

        internal void RenderOctreeX(byte[] data, ref int offset, float x, float y, float z, float w, float h, float d, int xmax, int ymax, int zmax)
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

        internal void RenderOctreeY(byte[] data, ref int offset, float x, float y, float z, float w, float h, float d, int xmax, int ymax, int zmax)
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

        internal void RenderOctreeZ(byte[] data, ref int offset, float x, float y, float z, float w, float h, float d, int xmax, int ymax, int zmax)
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
