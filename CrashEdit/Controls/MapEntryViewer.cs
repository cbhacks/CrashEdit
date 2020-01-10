using Crash;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class MapEntryViewer : UserControl
    {
        public MapEntryViewer(OldT17EntryController ec)
        {
            Dock = DockStyle.Fill;
            List<byte[]> palettes = new List<byte[]>();
            for (int i = 0; i < 4; ++i)
            {
                PaletteEntry ipal = ec.EntryChunkController.NSFController.NSF.FindEID<PaletteEntry>(BitConv.FromInt32(ec.MapEntry.Header,0x78+i*4));
                if (ipal != null)
                {
                    palettes.AddRange(ipal.Palettes);
                }
            }
            int imagecount = 0;
            int size = BitConv.FromInt32(ec.MapEntry.Header,0x4);
            for (int i = 0, s = BitConv.FromInt32(ec.MapEntry.Header,0); i < s; ++i)
            {
                ImageEntry imag = ec.EntryChunkController.NSFController.NSF.FindEID<ImageEntry>(BitConv.FromInt32(ec.MapEntry.Header,0x1B0+i*4));
                if (imag != null)
                {
                    foreach (byte[] frame in imag.Items)
                    {
                        int palette = BitConv.FromInt32(frame,0);
                        if (palette == -2) // null
                        {
                            ++imagecount;
                            continue;
                        }
                        else if (palette == -1) // 16bpp
                        {
                            Bitmap bitmap = new Bitmap(16,16,PixelFormat.Format16bppArgb1555);
                            Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
                            BitmapData bdata = bitmap.LockBits(brect,ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
                            try
                            {
                                for (int y = 0; y < 16; ++y)
                                {
                                    for (int x = 0; x < 16; ++x)
                                    {
                                        short color = BitConv.FromInt16(frame,(y*16+x) * 2 + 4);
                                        PixelConv.Unpack1555(color,out byte a,out byte b,out byte g,out byte r);
                                        color = PixelConv.Pack1555(1,r,g,b);
                                        System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,x*2 + y*bdata.Stride,color);
                                    }
                                }
                            }
                            finally
                            {
                                bitmap.UnlockBits(bdata);
                            }
                            PictureBox picture = new PictureBox
                            {
                                Location = new Point(imagecount / size * 16,imagecount % size * 16),
                                Size = new Size(16,16),
                                Image = bitmap
                            };
                            Controls.Add(picture);
                            ++imagecount;
                        }
                        else // 8bpp
                        {
                            Bitmap bitmap = new Bitmap(16,16,PixelFormat.Format16bppArgb1555);
                            Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
                            BitmapData bdata = bitmap.LockBits(brect,ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
                            try
                            {
                                for (int y = 0; y < 16; ++y)
                                {
                                    for (int x = 0; x < 16; ++x)
                                    {
                                        short color = BitConv.FromInt16(palettes[palette],frame[(y*16+x) + 4]*2);
                                        PixelConv.Unpack1555(color,out byte a,out byte b,out byte g,out byte r);
                                        color = PixelConv.Pack1555(1,r,g,b);
                                        System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,x*2 + y*bdata.Stride,color);
                                    }
                                }
                            }
                            finally
                            {
                                bitmap.UnlockBits(bdata);
                            }
                            PictureBox picture = new PictureBox
                            {
                                Location = new Point(imagecount / size * 16,imagecount % size * 16),
                                Size = new Size(16,16),
                                Image = bitmap
                            };
                            Controls.Add(picture);
                            ++imagecount;
                        }
                    }
                }
            }
            AutoScroll = true;
        }
    }
}
