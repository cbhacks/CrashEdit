using Crash;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    public partial class TextureViewer : Form
    {
        internal enum TextureType
        {
            Crash1,
            Crash2
        }

        private TextureChunk chunk;
        private OldModelTexture c1tex;
        private ModelTexture c2tex;
        private TextureType textype;
        private Rectangle selectedregion;

        public TextureViewer(TextureChunk texturechunk)
        {
            chunk = texturechunk;
            textype = TextureType.Crash2;

            Text = string.Format("Texture Viewer [{0}] - Right-click to save texture region to file", texturechunk.EName);

            InitializeComponent();
            UpdateC1Tab();
            UpdateC2Tab();

            tabC1.Enter += delegate (object sender, EventArgs e)
            {
                textype = TextureType.Crash1;
                UpdatePicture();
            };
            tabC2.Enter += delegate (object sender, EventArgs e)
            {
                textype = TextureType.Crash2;
                UpdatePicture();
            };
            tabControl1.SelectedTab = tabC2;

            pictureBox1.MouseClick += delegate (object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right && pictureBox1.Image != null && pictureBox1.Image is Bitmap)
                {
                    MemoryStream w = new MemoryStream();
                    ((Bitmap)pictureBox1.Image).Clone(selectedregion, PixelFormat.Format32bppArgb).Save(w, ImageFormat.Png);
                    FileUtil.SaveFile($"{chunk.EName}_{TexCY}_{TexCX}", w.ToArray(), FileFilters.PNG);
                }
            };

            foreach (ComboBox dpd in this.GetAll(typeof(ComboBox)))
            {
                dpd.SelectedIndex = 0;
            }
        }

        internal int TexPWidth => textype == TextureType.Crash1 ? (int)c1tex.PageWidth : (int)c2tex.PageWidth;
        internal int TexColorMode => textype == TextureType.Crash1 ? c1tex.ColorMode : c2tex.ColorMode;
        internal int TexBlendMode => textype == TextureType.Crash1 ? c1tex.BlendMode : c2tex.BlendMode;
        internal int TexX => textype == TextureType.Crash1 ? c1tex.XOff : (int)C2numX.Value;
        internal int TexY => textype == TextureType.Crash1 ? c1tex.YOff : (int)C2numY.Value;
        internal int TexW => textype == TextureType.Crash1 ? 4 << C1dpdW.SelectedIndex : (int)C2numW.Value;
        internal int TexH => textype == TextureType.Crash1 ? 4 << C1dpdH.SelectedIndex : (int)C2numH.Value;
        internal int TexCX => textype == TextureType.Crash1 ? c1tex.ClutX : c2tex.ClutX;
        internal int TexCY => textype == TextureType.Crash1 ? c1tex.ClutY : c2tex.ClutY;

        private void UpdatePicture()
        {
            int pw = TexPWidth;
            int ph = 128;
            Bitmap bitmap = new Bitmap(pw + 64, ph + 64, PixelFormat.Format32bppArgb); // we give the image some buffer space for the selection graphic
            Rectangle brect = new Rectangle(Point.Empty, bitmap.Size);
            BitmapData bdata = bitmap.LockBits(brect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            short[] palette = null;
            if (TexColorMode == 0)
            {
                palette = new short[16];
                for (int x = 0; x < 16; ++x)
                {
                    palette[x] = BitConv.FromInt16(chunk.Data, TexCY * 512 + (TexCX * 16 + x) * 2);
                }
            }
            else if (TexColorMode == 1)
            {
                palette = new short[256];
                for (int x = 0; x < 256; ++x)
                {
                    palette[x] = BitConv.FromInt16(chunk.Data, TexCY * 512 + x * 2);
                }
            }
            try
            {
                for (int y = 0; y < ph; y++)
                {
                    for (int x = 0; x < pw; x++)
                    {
                        short color = TexColorMode == 0 ? palette[chunk.Data[x / 2 + y * 512] >> ((x & 1) == 0 ? 0 : 4) & 0xF] :
                                      TexColorMode == 1 ? palette[chunk.Data[x + y * 512]] :
                                      TexColorMode == 2 ? BitConv.FromInt16(chunk.Data, x * 2 + y * 512)
                                      : (short)0;
                        System.Runtime.InteropServices.Marshal.WriteInt32(bdata.Scan0, x * 4 + y * bdata.Stride, PixelConv.Convert5551_8888(color, TexBlendMode));
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bdata);
            }
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int x = TexX;
                int y = TexY;
                int w = TexW;
                int h = TexH;
                using (var brush = new SolidBrush(Color.FromArgb(127, 0, 0, 0)))
                using (var pen = new Pen(Color.Black))
                {
                    int minh = Math.Min(h, ph - y);
                    g.FillRectangles(brush, new Rectangle[4]
                    {
                        new Rectangle(0, 0, pw, y),
                        new Rectangle(0, y, x, minh),
                        new Rectangle(x+w, y, Math.Max(pw-(x+w),0), minh),
                        new Rectangle(0, y+h, pw, Math.Max(ph-(y+h),0))
                    });
                    g.DrawRectangles(pen, new Rectangle[2]
                    {
                        new Rectangle(x-1,y-1,w+1,h+1),
                        new Rectangle(x-3,y-3,w+5,h+5)
                    });
                    pen.Color = Color.White;
                    g.DrawRectangle(pen, new Rectangle(x-2,y-2,w+3,h+3));
                }
                selectedregion.X = x;
                selectedregion.Y = y;
                selectedregion.Width = w;
                selectedregion.Height = h;
            }
            pictureBox1.Image = bitmap;
            pictureBox1.Size = bitmap.Size;
            Width = pw + 16;
        }

        private void UpdateC1Tab()
        {
            c1tex.ColorMode = (byte)C1dpdColor.SelectedIndex;
            c1tex.BlendMode = (byte)C1dpdBlend.SelectedIndex;
            c1tex.ClutX = (byte)C1numCX.Value;
            c1tex.ClutY = (byte)C1numCY.Value;
            c1tex.Segment = (byte)(C1numX.Value / 32);
            c1tex.XOffU = (byte)(C1numX.Value % 32);
            c1tex.YOffU = (byte)C1numY.Value;
            c1tex.UVIndex = C1dpdW.SelectedIndex + 5 * C1dpdH.SelectedIndex;
            UpdatePicture();
        }

        private void C1dpdCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1tex.ColorMode = (byte)C1dpdColor.SelectedIndex;
            C1numCX.Enabled = c1tex.ColorMode <= 0;
            C1numCY.Enabled = c1tex.ColorMode <= 1;
            UpdatePicture();
        }

        private void C1numX_ValueChanged(object sender, EventArgs e)
        {
            c1tex.Segment = (byte)(C1numX.Value / 32);
            c1tex.XOffU = (byte)(C1numX.Value % 32);
            UpdatePicture();
        }

        private void C1numY_ValueChanged(object sender, EventArgs e)
        {
            c1tex.YOffU = (byte)C1numY.Value;
            UpdatePicture();
        }

        private void C1dpdW_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1tex.UVIndex = C1dpdW.SelectedIndex + 5 * C1dpdH.SelectedIndex;
            UpdatePicture();
        }

        private void C1dpdH_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1tex.UVIndex = C1dpdW.SelectedIndex + 5 * C1dpdH.SelectedIndex;
            UpdatePicture();
        }

        private void C1numCX_ValueChanged(object sender, EventArgs e)
        {
            c1tex.ClutX = (byte)C1numCX.Value;
            UpdatePicture();
        }

        private void C1numCY_ValueChanged(object sender, EventArgs e)
        {
            c1tex.ClutY = (byte)C1numCY.Value;
            UpdatePicture();
        }

        private void C1dpdBlend_SelectedIndexChanged(object sender, EventArgs e)
        {
            c1tex.BlendMode = (byte)C1dpdBlend.SelectedIndex;
            UpdatePicture();
        }

        private void UpdateC2Tab()
        {
            c2tex.ColorMode = (byte)C2dpdColor.SelectedIndex;
            c2tex.BlendMode = (byte)C2dpdBlend.SelectedIndex;
            c2tex.ClutX = (byte)C2numCX.Value;
            byte cy = (byte)C2numCY.Value;
            c2tex.ClutY1 = (byte)((cy & 0x3) << 2);
            c2tex.ClutY2 = (byte)(cy >> 0x2 & 0x3F);
            UpdatePicture();
        }

        private void C2numX_ValueChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void C2numY_ValueChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void C2numW_ValueChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void C2numH_ValueChanged(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void C2numCX_ValueChanged(object sender, EventArgs e)
        {
            c2tex.ClutX = (byte)C2numCX.Value;
            UpdatePicture();
        }

        private void C2numCY_ValueChanged(object sender, EventArgs e)
        {
            byte cy = (byte)C2numCY.Value;
            c2tex.ClutY1 = (byte)((cy & 0x3) << 2);
            c2tex.ClutY2 = (byte)(cy >> 0x2 & 0x3F);
            UpdatePicture();
        }

        private void C2dpdColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            c2tex.ColorMode = (byte)C2dpdColor.SelectedIndex;
            C2numCX.Enabled = c2tex.ColorMode <= 0;
            C2numCY.Enabled = c2tex.ColorMode <= 1;
            UpdatePicture();
        }

        private void C2dpdBlend_SelectedIndexChanged(object sender, EventArgs e)
        {
            c2tex.BlendMode = (byte)C2dpdBlend.SelectedIndex;
            UpdatePicture();
        }
    }
}
