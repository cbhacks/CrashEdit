using Crash;
using CrashEdit.Properties;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CrashEdit
{
    public class OctreeRenderer : IDisposable
    {
        public static Rgba[] node_colors = new Rgba[0x8000];
        private const float COLOR_RANGE = 0.75f;

        // settings
        public bool enable;
        public int node_filter;
        public bool outline;
        public byte alpha;
        public bool show_neighbor_zones;

        private readonly GLViewer viewer;

        private Form octree_form;
        private bool form_want_update = false;
        private bool form_want_select = false;

        private int[] nodes;
        private int nodes_x;
        private int nodes_y;
        private int nodes_z;
        private readonly List<ushort> node_types = new();

        private static int texColors = 0;
        private static int texNodes = 0;
        private static bool colors_uploaded = false;

        public OctreeRenderer(GLViewer viewer)
        {
            enable = false;
            node_filter = 0;
            outline = false;
            show_neighbor_zones = false;
            this.viewer = viewer;
        }

        static OctreeRenderer()
        {
            GenerateColors();
        }

        public static void GenerateColors()
        {
            int range = (int)(255 * COLOR_RANGE);
            int min = 255 - range;
            Random random = new(0x666EDD1E);
            for (int i = 0; i < 0x8000; ++i)
            {
                int r = random.Next(range) + min;
                int g = random.Next(range) + min;
                int b = random.Next(range) + min;
                node_colors[i] = new((byte)r, (byte)g, (byte)b, 255);
            }
            colors_uploaded = false;
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
            return GLViewer.KeyboardControls.ToggleZoneOctree.Print(GLViewer.OnOffName(enable))
                + (!enable ? "" :
                   GLViewer.KeyboardControls.ToggleZoneOctreeOutline.Print(GLViewer.OnOffName(outline))
                 + GLViewer.KeyboardControls.ToggleZoneOctreeNeighbors.Print(GLViewer.OnOffName(show_neighbor_zones))
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
                            node_filter = 0;
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
                foreach (var node in node_types)
                {
                    ListViewItem lsi = new();
                    lsi.Text = string.Format("{2:X2}:{1:X2}:{0:X1}", node >> 1 & 0x7, node >> 4 & 0x3F, node >> 10 & 0x3F);
                    lsi.BackColor = (Color)(Color4)node_colors[node >> 1];
                    lsi.ForeColor = lsi.BackColor.GetBrightness() >= 0.5 ? Color.Black : Color.White;
                    lsi.Tag = node;
                    lst.Items.Add(lsi);
                }
                lst.SelectedIndexChanged += (sender, e) =>
                {
                    if (lst.SelectedItems.Count == 0)
                    {
                        node_filter = 0;
                    }
                    else
                    {
                        node_filter = (ushort)lst.SelectedItems[0].Tag;
                    }
                };
                octree_form.Controls.Add(lst);
            }
        }

        private int GetNodeArrayIndex(int x, int y, int z) => x + y * nodes_x + z * nodes_y * nodes_x;

        public void RenderOctree(byte[] data, int start_offset, Vector3 trans, Vector3 size, int xmax, int ymax, int zmax)
        {
            nodes_x = 1 << xmax;
            nodes_y = 1 << ymax;
            nodes_z = 1 << zmax;

            if (nodes == null || nodes.Length < nodes_x * nodes_y * nodes_z)
                nodes = new int[nodes_x * nodes_y * nodes_z];
            else
                nodes.Initialize();

            if (texColors == 0) texColors = GL.GenTexture();
            if (texNodes == 0) texNodes = GL.GenTexture();

            if (!colors_uploaded)
            {
                GL.ActiveTexture(TextureUnit.Texture3);
                GL.BindTexture(TextureTarget.Texture2D, texColors);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, 256, 128, 0, PixelFormat.Rgba, PixelType.UnsignedByte, node_colors);
                colors_uploaded = true;
            }

            RenderOctreeNode(data, start_offset, 0, 0, 0, trans, size, xmax, ymax, zmax);

            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture3D, texNodes);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexImage3D(TextureTarget.Texture3D, 0, PixelInternalFormat.R32i, nodes_x, nodes_y, nodes_z, 0, PixelFormat.RedInteger, PixelType.Int, nodes);

            if (outline)
            {
                Vector3 cur_size = new(size.X, 0, 0);
                Vector3w cur_nodes = new(nodes_x, 0, 0);
                for (int i = 0; i <= nodes_y; ++i)
                {
                    for (int ii = 0; ii <= nodes_z; ++ii)
                    {
                        Vector3 cur_trans = new(trans.X, trans.Y + size.Y / nodes_y * i, trans.Z + size.Z / nodes_z * ii);
                        if (i < nodes_y && ii < nodes_z)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(0, i, ii), cur_nodes);
                        if (i > 0 && ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(0, i - 1, ii - 1), cur_nodes);
                        if (i > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(0, i - 1, ii), cur_nodes);
                        if (ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(0, i, ii - 1), cur_nodes);
                    }
                }
                cur_size = new(0, size.Y, 0);
                cur_nodes = new(0, nodes_y, 0);
                for (int i = 0; i <= nodes_x; ++i)
                {
                    for (int ii = 0; ii <= nodes_z; ++ii)
                    {
                        Vector3 cur_trans = new(trans.X + size.X / nodes_x * i, trans.Y, trans.Z + size.Z / nodes_z * ii);
                        if (i < nodes_x && ii < nodes_z)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i, 0, ii), cur_nodes);
                        if (i > 0 && ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i - 1, 0, ii - 1), cur_nodes);
                        if (i > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i - 1, 0, ii), cur_nodes);
                        if (ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i, 0, ii - 1), cur_nodes);
                    }
                }
                cur_size = new(0, 0, size.Z);
                cur_nodes = new(0, 0, nodes_z);
                for (int i = 0; i <= nodes_x; ++i)
                {
                    for (int ii = 0; ii <= nodes_y; ++ii)
                    {
                        Vector3 cur_trans = new(trans.X + size.X / nodes_x * i, trans.Y + size.Y / nodes_y * ii, trans.Z);
                        if (i < nodes_x && ii < nodes_y)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i, ii, 0), cur_nodes);
                        if (i > 0 && ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i - 1, ii - 1, 0), cur_nodes);
                        if (i > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i - 1, ii, 0), cur_nodes);
                        if (ii > 0)
                            viewer.AddOctreeLine(cur_trans, cur_size, new Vector3w(i, ii - 1, 0), cur_nodes);
                    }
                }

                viewer.OctreeSetNodeShadeMax(0);
            }
            else
            {
                for (int i = 0; i <= nodes_x; ++i)
                {
                    Vector3 cur_trans = new(trans.X + size.X / nodes_x * i, trans.Y, trans.Z);
                    if (i < nodes_x)
                        viewer.AddOctreeX(cur_trans, new Vector3(0, size.Y, size.Z), i, new Vector3w(0, nodes_y, nodes_z));
                    if (i > 0)
                        viewer.AddOctreeX(cur_trans, new Vector3(0, size.Y, size.Z), i - 1, new Vector3w(0, nodes_y, nodes_z));
                }
                for (int i = 0; i <= nodes_y; ++i)
                {
                    Vector3 cur_trans = new(trans.X, trans.Y + size.Y / nodes_y * i, trans.Z);
                    if (i < nodes_y)
                        viewer.AddOctreeY(cur_trans, new Vector3(size.X, 0, size.Z), i, new Vector3w(nodes_x, 0, nodes_z));
                    if (i > 0)
                        viewer.AddOctreeY(cur_trans, new Vector3(size.X, 0, size.Z), i - 1, new Vector3w(nodes_x, 0, nodes_z));
                }
                for (int i = 0; i <= nodes_z; ++i)
                {
                    Vector3 cur_trans = new(trans.X, trans.Y, trans.Z + size.Z / nodes_z * i);
                    if (i < nodes_z)
                        viewer.AddOctreeZ(cur_trans, new Vector3(size.X, size.Y, 0), i, new Vector3w(nodes_x, nodes_y, 0));
                    if (i > 0)
                        viewer.AddOctreeZ(cur_trans, new Vector3(size.X, size.Y, 0), i - 1, new Vector3w(nodes_x, nodes_y, 0));
                }

                viewer.OctreeSetNodeShadeMax(Settings.Default.NodeShadeMax);
            }

            viewer.OctreeSetNodeAlpha(alpha / 255f);
            viewer.OctreeSetOutline(outline);

            viewer.RenderOctree();
        }

        public void RenderOctreeNode(byte[] data, int offset, int x, int y, int z, Vector3 trans, Vector3 size, int xmax, int ymax, int zmax)
        {
            void fill_nodes(ushort value)
            {
                for (int i = x; i < x + (1 << xmax); ++i)
                {
                    for (int ii = y; ii < y + (1 << ymax); ++ii)
                    {
                        for (int iii = z; iii < z + (1 << zmax); ++iii)
                        {
                            // pack the node resolution into the upper 16 bits (5-5-5 currently) so the node renderer can merge them
                            // don't do it on null nodes so we can just check for 0
                            if (value != 0)
                                nodes[GetNodeArrayIndex(i, ii, iii)] = value | (xmax << 16) | (ymax << 21) | (zmax << 26);
                        }
                    }
                }
            }

            void render_x(byte[] data, ref int offset, int x, int y, int z, int xmax, int ymax, int zmax)
            {
                if (xmax > 0)
                {
                    // have X-axis subdivisions remaining, split into two nodes
                    --xmax;
                    render_y(data, ref offset, x, y, z, xmax, ymax, zmax);
                    render_y(data, ref offset, x + (1 << xmax), y, z, xmax, ymax, zmax);
                }
                else
                {
                    render_y(data, ref offset, x, y, z, xmax, ymax, zmax);
                }
            }

            void render_y(byte[] data, ref int offset, int x, int y, int z, int xmax, int ymax, int zmax)
            {
                if (ymax > 0)
                {
                    // have Y-axis subdivisions remaining, split into two nodes
                    --ymax;
                    render_z(data, ref offset, x, y, z, xmax, ymax, zmax);
                    render_z(data, ref offset, x, y + (1 << ymax), z, xmax, ymax, zmax);
                }
                else
                {
                    render_z(data, ref offset, x, y, z, xmax, ymax, zmax);
                }
            }

            void render_z(byte[] data, ref int offset, int x, int y, int z, int xmax, int ymax, int zmax)
            {
                if (zmax > 0)
                {
                    // have Z-axis subdivisions remaining, split into two nodes
                    --zmax;
                    RenderOctreeNode(data, offset, x, y, z, trans, size, xmax, ymax, zmax);
                    offset += 2;
                    RenderOctreeNode(data, offset, x, y, z + (1 << zmax), trans, size, xmax, ymax, zmax);
                    offset += 2;
                }
                else
                {
                    RenderOctreeNode(data, offset, x, y, z, trans, size, xmax, ymax, zmax);
                    offset += 2;
                }
            }

            // render a single leaf node, or child nodes
            int value = BitConv.FromUInt16(data, offset);
            if ((value & 1) != 0)
            {
                // leaf node
                if (!node_types.Contains((ushort)value))
                {
                    node_types.Add((ushort)value);
                }

                if (node_filter != 0 && node_filter != value)
                    return;
                // viewer.AddBox(new Vector3(x, y, z), new Vector3(w, h, d), colors, outline);
                fill_nodes((ushort)value);
            }
            else if (value != 0)
            {
                // 
                if (value == offset) throw new ApplicationException("RenderOctree: Infinitely-recursive node");
                render_x(data, ref value, x, y, z, xmax, ymax, zmax);
            }
            else
            {
                // empty node, it's just air!
            }
        }

        public void Dispose()
        {
            octree_form?.Close();
        }
    }
}
