using Crash;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public class OctreeRenderer : IDisposable
    {
        public static Dictionary<short, Rgba[]> nodeColors = new();

        public bool enable;
        public int node_filter;
        public bool outline;
        public byte alpha;
        public bool show_neighbor_zones;

        private readonly GLViewer viewer;

        private Form octree_form;
        private bool form_want_update = false;
        private bool form_want_select = false;

        public OctreeRenderer(GLViewer viewer)
        {
            enable = false;
            node_filter = -1;
            outline = true;
            show_neighbor_zones = false;
            this.viewer = viewer;
        }

        public void UpdateForm()
        {
            if (octree_form != null)
            {
                if (!octree_form.Visible)
                {
                    octree_form.Show();
                }
                if (form_want_update)
                {
                    UpdateOctreeFormList();
                }
                if (form_want_select)
                {
                    octree_form.Select();
                }
            }
            form_want_update = false;
            form_want_select = false;
        }

        public string PrintHelp()
        {
            return GLViewer.KeyboardControls.ToggleZoneOctree.Print(GLViewer.BoolToEnable(enable))
                + (!enable ? "" :
                   GLViewer.KeyboardControls.ToggleZoneOctreeOutline.Print(GLViewer.BoolToEnable(outline))
                 + GLViewer.KeyboardControls.ToggleZoneOctreeNeighbors.Print(GLViewer.BoolToEnable(show_neighbor_zones))
                 + GLViewer.KeyboardControls.OpenOctreeWindow.Print()
                    );
        }

        public void RunLogic()
        {
            if (viewer.KPress(GLViewer.KeyboardControls.ToggleZoneOctree)) enable = !enable;
            if (enable)
            {
                if (viewer.KPress(GLViewer.KeyboardControls.ToggleZoneOctreeOutline)) outline = !outline;
                if (viewer.KPress(GLViewer.KeyboardControls.ToggleZoneOctreeNeighbors)) show_neighbor_zones = !show_neighbor_zones;
                if (viewer.KPress(GLViewer.KeyboardControls.OpenOctreeWindow))
                {
                    if (octree_form == null || octree_form.IsDisposed)
                    {
                        octree_form = new Form();
                        octree_form.FormClosing += (sender, e) =>
                        {
                            node_filter = -1;
                            octree_form = null;
                        };
                        form_want_update = true;
                    }
                    else
                    {
                        form_want_select = true;
                    }
                }
            }
        }

        internal void UpdateOctreeFormList()
        {
            if (octree_form != null && !octree_form.IsDisposed)
            {
                octree_form.Controls.Clear();
                ListView lst = new()
                {
                    Dock = DockStyle.Fill
                };
                foreach (var color in nodeColors)
                {
                    ListViewItem lsi = new();
                    lsi.Text = string.Format("{2:X2}:{1:X2}:{0:X1}", color.Key >> 1 & 0x7, color.Key >> 4 & 0x3F, color.Key >> 10 & 0x3F);
                    lsi.BackColor = (Color)(Color4)color.Value[0];
                    lsi.ForeColor = lsi.BackColor.GetBrightness() >= 0.5 ? Color.Black : Color.White;
                    lsi.Tag = color.Key;
                    lst.Items.Add(lsi);
                }
                lst.SelectedIndexChanged += (sender, e) =>
                {
                    if (lst.SelectedItems.Count == 0)
                    {
                        node_filter = -1;
                    }
                    else
                    {
                        node_filter = (ushort)(short)lst.SelectedItems[0].Tag;
                    }
                };
                octree_form.Controls.Add(lst);
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
                if (node_filter != -1 && node_filter != value)
                    return;
                Rgba[] colors;
                if (!nodeColors.TryGetValue((short)value, out colors))
                {
                    byte[] colorbuf = new byte[3];
                    Random random = new Random(value);
                    random.NextBytes(colorbuf);
                    colors = new Rgba[8];
                    for (int i = 0; i < colors.Length; ++i)
                    {
                        int inc = i * 3;
                        colors[i] = new((byte)Math.Min(colorbuf[0] + inc, 255),
                                        (byte)Math.Min(colorbuf[1] + inc, 255),
                                        (byte)Math.Min(colorbuf[2] + inc, 255),
                                        255);
                    }
                    nodeColors.Add((short)value, colors);
                }
                for (int i = 0; i < colors.Length; ++i)
                {
                    colors[i].a = alpha;
                }
                viewer.AddBox(new Vector3(x, y, z), new Vector3(w, h, d), colors, outline);
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

        public void Dispose()
        {
            octree_form?.Close();
        }
    }
}
