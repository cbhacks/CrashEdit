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
        private TextureType textype;
        private Rectangle selectedregion;

        public TextureViewer(TextureChunk texturechunk)
        {
            chunk = texturechunk;
            textype = TextureType.Crash2;

            Text = string.Format("Texture Viewer [{0}] - Right-click to save texture region to file", texturechunk.EName);

            InitializeComponent();

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
                if (e.Button == MouseButtons.Right && pictureBox1.Image != null && pictureBox1.Image is Bitmap bmp)
                {
                    using (MemoryStream w = new MemoryStream())
                    {
                        bmp.Clone(selectedregion, PixelFormat.Format32bppArgb).Save(w, ImageFormat.Png);
                        FileUtil.SaveFile($"{chunk.EName}_{TexCY}_{TexCX}", w.ToArray(), FileFilters.PNG);
                    }
                }
            };

            foreach (ComboBox dpd in this.GetAll(typeof(ComboBox)))
            {
                dpd.SelectedIndex = 0;
            }

            C1dpdColor.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C1dpdBlend.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C1dpdW.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C1dpdH.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C2dpdColor.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C2dpdBlend.SelectedIndexChanged += new EventHandler(Control_UpdatePicture);
            C1numX.ValueChanged += new EventHandler(Control_UpdatePicture);
            C1numY.ValueChanged += new EventHandler(Control_UpdatePicture);
            C1numCX.ValueChanged += new EventHandler(Control_UpdatePicture);
            C1numCY.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numX.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numY.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numCX.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numCY.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numW.ValueChanged += new EventHandler(Control_UpdatePicture);
            C2numH.ValueChanged += new EventHandler(Control_UpdatePicture);

            UpdatePicture();

            groupBox1.Text = Properties.Resources.TextureViewer_groupBox1;
            groupBox2.Text = Properties.Resources.TextureViewer_groupBox2;
            groupBox4.Text = Properties.Resources.TextureViewer_groupBox4;
            groupBox5.Text = Properties.Resources.TextureViewer_groupBox5;
            groupBox6.Text = Properties.Resources.TextureViewer_groupBox5;
            groupBox7.Text = Properties.Resources.TextureViewer_groupBox4;
            groupBox9.Text = Properties.Resources.TextureViewer_groupBox2;
            groupBox10.Text = Properties.Resources.TextureViewer_groupBox1;
        }

        internal int TexColorMode => textype == TextureType.Crash1 ? C1dpdColor.SelectedIndex : C2dpdColor.SelectedIndex;
        internal int TexBlendMode => textype == TextureType.Crash1 ? C1dpdBlend.SelectedIndex : C2dpdBlend.SelectedIndex;
        internal int TexX => textype == TextureType.Crash1 ? (2 << (2-C1dpdColor.SelectedIndex)) * (int)C1numX.Value : (int)C2numX.Value;
        internal int TexY => textype == TextureType.Crash1 ? (int)C1numY.Value * 4 : (int)C2numY.Value;
        internal int TexW => textype == TextureType.Crash1 ? 4 << C1dpdW.SelectedIndex : (int)C2numW.Value;
        internal int TexH => textype == TextureType.Crash1 ? 4 << C1dpdH.SelectedIndex : (int)C2numH.Value;
        internal int TexCX => textype == TextureType.Crash1 ? (int)C1numCX.Value : (int)C2numCX.Value;
        internal int TexCY => textype == TextureType.Crash1 ? (int)C1numCY.Value : (int)C2numCY.Value;

        private void Control_UpdatePicture(object sender, EventArgs e)
        {
            UpdatePicture();
        }

        private void UpdatePicture()
        {
            int pw = 256 << (2-TexColorMode);
            int ph = 128;
            Bitmap bitmap = new Bitmap(pw + 64, ph + 64, PixelFormat.Format32bppArgb); // we give the image some buffer space for the selection graphic
            Rectangle brect = new Rectangle(Point.Empty, bitmap.Size);
            BitmapData bdata = bitmap.LockBits(brect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int[] palette = null;
            int colormode = TexColorMode;
            int blendmode = TexBlendMode;
            if (colormode == 0)
            {
                int clutx = TexCX;
                int cluty = TexCY;
                palette = new int[16];
                for (int x = 0; x < 16; ++x)
                {
                    palette[x] = PixelConv.Convert5551_8888(BitConv.FromInt16(chunk.Data, cluty * 512 + (clutx * 16 + x) * 2), blendmode);
                }
            }
            else if (colormode == 1)
            {
                int cluty = TexCY;
                palette = new int[256];
                for (int x = 0; x < 256; ++x)
                {
                    palette[x] = PixelConv.Convert5551_8888(BitConv.FromInt16(chunk.Data, cluty * 512 + x * 2), blendmode);
                }
            }
            try
            {
                for (int y = 0; y < ph; y++)
                {
                    for (int x = 0; x < pw; x++)
                    {
                        int pixel = colormode == 0 ? palette[chunk.Data[x / 2 + y * 512] >> ((x & 1) == 0 ? 0 : 4) & 0xF] :
                                    colormode == 1 ? palette[chunk.Data[x + y * 512]] :
                                    colormode == 2 ? PixelConv.Convert5551_8888(BitConv.FromInt16(chunk.Data, x * 2 + y * 512), blendmode)
                                    : throw new Exception("invalid colormode");
                        System.Runtime.InteropServices.Marshal.WriteInt32(bdata.Scan0, x * 4 + y * bdata.Stride, pixel);
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
            if (Width != pw + 16)
                Width = pw + 16;
        }
    }
}
