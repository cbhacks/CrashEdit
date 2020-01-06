using Crash;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class PaletteEntryBox : UserControl
    {
        public PaletteEntryBox(PaletteEntry entry)
        {
            Bitmap bitmap = new Bitmap(256,entry.Palettes.Length,PixelFormat.Format16bppArgb1555);
            Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
            BitmapData bdata = bitmap.LockBits(brect,ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
            try
            {
                for (int y = 0; y < entry.Palettes.Length; ++y)
                {
                    for (int x = 0; x < 256; ++x)
                    {
                        short color = BitConv.FromInt16(entry.Palettes[y],x * 2);
                        PixelConv.Unpack1555(color, out byte alpha, out byte blue, out byte green, out byte red);
                        color = PixelConv.Pack1555(1, red, green, blue);
                        System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,x * 2 + y * bdata.Stride,color);
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bdata);
            }
            PictureBox picture = new PictureBox();
            picture.Dock = DockStyle.Fill;
            picture.Image = bitmap;
            Controls.Add(picture);
        }
    }
}
