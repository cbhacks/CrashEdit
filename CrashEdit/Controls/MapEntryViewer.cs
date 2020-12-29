using Crash;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class MapEntryViewer : UserControl
    {
        public MapEntryViewer(MapEntryController ec)
        {
            Dock = DockStyle.Fill;
            List<byte[]> palettes = new List<byte[]>();
            for (int i = 0; i < 4; ++i)
            {
                PaletteEntry ipal = ec.EntryChunkController.NSFController.NSF.GetEntry<PaletteEntry>(BitConv.FromInt32(ec.MapEntry.Header,0x78+i*4));
                if (ipal != null)
                {
                    palettes.AddRange(ipal.Palettes);
                }
            }
            int imagecount = 0;
            for (int i = 0, s = BitConv.FromInt32(ec.MapEntry.Header,0); i < s; ++i)
            {
                ImageEntry imag = ec.EntryChunkController.NSFController.NSF.GetEntry<ImageEntry>(BitConv.FromInt32(ec.MapEntry.Header,0x1B0+i*4));
                imagecount += imag.Items.Count;
            }
            int size = BitConv.FromInt32(ec.MapEntry.Header,0x4);
            Bitmap bitmap = new Bitmap((int)(System.Math.Ceiling(imagecount/(double)size)*16),size*16,PixelFormat.Format16bppArgb1555);
            Rectangle brect = new Rectangle(Point.Empty,bitmap.Size);
            BitmapData bdata = bitmap.LockBits(brect, ImageLockMode.WriteOnly,PixelFormat.Format16bppArgb1555);
            imagecount = 0;
            try
            {
            for (int i = 0, s = BitConv.FromInt32(ec.MapEntry.Header,0); i < s; ++i)
            {
                ImageEntry imag = ec.EntryChunkController.NSFController.NSF.GetEntry<ImageEntry>(BitConv.FromInt32(ec.MapEntry.Header,0x1B0+i*4));
                if (imag != null)
                {
                    foreach (byte[] frame in imag.Items)
                    {
                        int palette = BitConv.FromInt32(frame,0);
                        if (palette == -2 || palette == -3 || palette == -4) // null, font, icon
                        {
                            ++imagecount;
                        }
                        else if (palette == -1) // 16bpp
                        {
                                for (int y = 0; y < 16; ++y)
                                {
                                    for (int x = 0; x < 16; ++x)
                                    {
                                        short color = BitConv.FromInt16(frame,(y*16+x) * 2 + 4);
                                        PixelConv.Unpack1555(color,out byte a,out byte b,out byte g,out byte r);
                                        color = PixelConv.Pack1555(1,r,g,b);
                                        System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,(imagecount/size*16+x)*2 + (imagecount%size*16+y)*bdata.Stride,color);
                                    }
                                }
                            ++imagecount;
                        }
                        else // 8bpp
                        {
                                for (int y = 0; y < 16; ++y)
                                {
                                    for (int x = 0; x < 16; ++x)
                                    {
                                        short color = BitConv.FromInt16(palettes[palette],frame[(y*16+x) + 4]*2);
                                        PixelConv.Unpack1555(color,out byte a,out byte b,out byte g,out byte r);
                                        color = PixelConv.Pack1555(1,r,g,b);
                                        System.Runtime.InteropServices.Marshal.WriteInt16(bdata.Scan0,(imagecount/size*16+x)*2 + (imagecount%size*16+y)*bdata.Stride,color);
                                    }
                                }
                            ++imagecount;
                        }
                    }
                }
            }
            }
            finally
            {
                bitmap.UnlockBits(bdata);
            }
            PictureBox picture = new PictureBox
            {
                Size = bitmap.Size,
                Image = bitmap
            };
            picture.MouseClick += delegate (object sender,MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    MemoryStream w = new MemoryStream();
                    bitmap.Save(w,ImageFormat.Png);
                    FileUtil.SaveFile(w.ToArray(),FileFilters.PNG);
                }
            };
            Controls.Add(picture);
            AutoScroll = true;
        }
    }
}
