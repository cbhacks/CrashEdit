using SharpFont;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrashEdit
{
    public class FontTable : Dictionary<uint, FontTableEntry>
    {
        public Face Face { get; private set; }
        public Bitmap Bitmap { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float Adjust { get; private set; }
        public float LineHeight { get; private set; }

        public Bitmap LoadFont(Library lib, string fname, int size)
        {
            Face = new Face(lib, fname);
            Face.SetCharSize(size, size, 0, 0);

            Width = Face.Size.Metrics.NominalWidth;
            Height = Face.Size.Metrics.NominalHeight;
            Adjust = (float)Face.Size.Metrics.Ascender + (float)Face.Size.Metrics.Descender;
            LineHeight = (float)Face.Height / Face.UnitsPerEM * Width;

            int pad = 1;
            int total_x = 0;
            int glyph_max_w = 0;
            int glyph_max_h = 0;
            for (uint c = Face.GetFirstChar(out uint g); g != 0; c = Face.GetNextChar(c, out g))
            {
                Face.LoadGlyph(g, LoadFlags.Render, LoadTarget.Light);
                var glyph = Face.Glyph;
                Bitmap bmp = null;
                if (glyph.Bitmap.Buffer != IntPtr.Zero)
                {
                    bmp = glyph.Bitmap.ToGdipBitmap();
                    int w = bmp.Width + pad * 2, h = bmp.Height + pad * 2;
                    glyph_max_w = Math.Max(w, glyph_max_w);
                    glyph_max_h = Math.Max(h, glyph_max_h);
                    total_x += w;
                }
                Add(c, new FontTableEntry(bmp, glyph.Metrics, (float)glyph.Advance.X, g));
            }
            int log_full = (int)Math.Ceiling(Math.Log(total_x * glyph_max_h, 2));
            int full_w = 1 << ((log_full + 1) / 2);
            int full_h = 1 << (log_full / 2);
            var full_bmp = new Bitmap(full_w, full_h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int cur_x = 0, cur_y = 0, glyph_max_h_this_line = 0;
            using (var gfx = Graphics.FromImage(full_bmp))
            {
                foreach (var g in Values)
                {
                    if (g.Glyph == null)
                        continue;

                    int w = g.Glyph.Width + pad * 2;
                    int h = g.Glyph.Height + pad * 2;
                    if (cur_x + w > full_w)
                    {
                        cur_x = 0;
                        cur_y += glyph_max_h_this_line - pad;
                        glyph_max_h_this_line = 0;
                    }
                    g.Left = (cur_x + pad) / (float)full_bmp.Width;
                    g.Top = (cur_y + pad) / (float)full_bmp.Height;
                    g.Right = (cur_x + pad + g.Glyph.Width) / (float)full_bmp.Width;
                    g.Bottom = (cur_y + pad + g.Glyph.Height) / (float)full_bmp.Height;
                    gfx.DrawImageUnscaled(g.Glyph, new Point(cur_x + pad, cur_y + pad));
                    //gfx.DrawImage(g.Glyph, new Rectangle(cur_x, cur_y + pad, pad, g.Glyph.Height), new Rectangle(0, 0, 1, g.Glyph.Height), GraphicsUnit.Pixel);
                    //gfx.DrawImage(g.Glyph, new Rectangle(cur_x + pad + g.Glyph.Width, cur_y + pad, pad, g.Glyph.Height), new Rectangle(g.Glyph.Width - 1, 0, 1, g.Glyph.Height), GraphicsUnit.Pixel);
                    //gfx.DrawImage(g.Glyph, new Rectangle(cur_x + pad, cur_y, g.Glyph.Width, pad), new Rectangle(0, 0, g.Glyph.Width, 1), GraphicsUnit.Pixel);
                    //gfx.DrawImage(g.Glyph, new Rectangle(cur_x + pad, cur_y + pad + g.Glyph.Height, g.Glyph.Width, pad), new Rectangle(0, g.Glyph.Height - 1, g.Glyph.Width, 1), GraphicsUnit.Pixel);

                    cur_x += w;
                    glyph_max_h_this_line = Math.Max(glyph_max_h_this_line, h);
                }
                cur_y += glyph_max_h_this_line;
            }
            while (Math.Ceiling(Math.Log(cur_y, 2)) < MathExt.Log2(full_bmp.Height))
            {
                full_bmp = full_bmp.Clone(new Rectangle(Point.Empty, new Size(full_bmp.Width, full_bmp.Height / 2)), full_bmp.PixelFormat);
                foreach (var g in Values)
                {
                    g.Top *= 2;
                    g.Bottom *= 2;
                }
            }
            full_bmp.Save("test-full.png");
            Console.WriteLine($"created bitmap for {Face.FamilyName} {Face.StyleName} {size}pt ({full_bmp.Width}x{full_bmp.Height})");

            Bitmap = full_bmp;
            return Bitmap;
        }
    }

    public sealed class FontTableEntry
    {
        public float Left { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public float Bottom { get; set; }
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float AdvanceX { get; private set; }
        public float BearingX { get; private set; }
        public float BearingY { get; private set; }
        public Bitmap Glyph { get; private set; }
        public uint GlyphID { get; private set; }

        public FontTableEntry(Bitmap bmp, GlyphMetrics metrics, float advance, uint glyph_id)
        {
            AdvanceX = advance;
            Width = (float)metrics.Width;
            Height = (float)metrics.Height;
            BearingX = (float)metrics.HorizontalBearingX;
            BearingY = (float)metrics.HorizontalBearingY;
            Glyph = bmp;
            GlyphID = glyph_id;
        }
    }
}
