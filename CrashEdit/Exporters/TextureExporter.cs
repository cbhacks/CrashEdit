using System.Drawing;
using System.Drawing.Imaging;
using CrashEdit.CE;

namespace CrashEdit.Exporters;

public class TextureExporter
{
    public static Bitmap Create4bpp (byte [] data, VertexTexInfo info)
    {
        Bitmap bmp = new Bitmap (1024, 128);

        // ClutX really contains the count of palettes available
        // so multiplying by 16 gives the right amount of info for it
        int clutx = info.ClutX * 16;
        
        // write the texture somewhere for now, testing
        for (int x = 0; x < 1024; x++)
        {
            for (int y = info.ClutY; y < 128; y++)
            {
                byte entry = data [y * 512 + (x / 2)];

                if ((x % 2) == 0)
                    entry = (byte) (entry & 0xF);
                else
                    entry = (byte) ((entry >> 4) & 0xF);
                
                // now read the Color from the palette
                ushort color = (ushort) (
                    data [info.ClutY * 512 + ((clutx + entry) * 2)] |
                    (data [info.ClutY * 512 + ((clutx + entry) * 2 + 1)] << 8)
                );

                int red = (int) (((color & 0x1F) / 31F) * 255);
                int green = (int) ((((color >> 5) & 0x1F) / 31F) * 255);
                int blue = (int) ((((color >> 10) & 0x1F) / 31F) * 255);
                int a = ((color >> 15) & 0x1);
                int alpha = 255;

                if (info.Blend == 0 && (info.Color == 0 || info.Color == 1))
                {
                    if (a == 1)
                        alpha = 127;
                    else
                        alpha = 255;

                    if (red == 0 && green == 0 && blue == 0 && a == 0)
                        alpha = 0;
                }
                else if (info.Blend == 1 && (info.Color == 0 || info.Color == 1))
                {
                    if (red == 0 && green == 0 && blue == 0 && a == 0)
                        alpha = 0;
                }

                // Colors are in rgb15 format
                Color c = Color.FromArgb (
                    alpha,
                    red,
                    green,
                    blue
                );
                
                bmp.SetPixel (x, y, c);
            }
        }

        return bmp;
    }

    public static Bitmap Create8bpp (byte [] data, VertexTexInfo info)
    {
        Bitmap bmp = new Bitmap (512, 128);

        // ClutX really contains the count of palettes available
        // so multiplying by 16 gives the right amount of info for it
        int clutx = info.ClutX * 16;
        
        // write the texture somewhere for now, testing
        for (int x = 0; x < 512; x++)
        {
            for (int y = info.ClutY; y < 128; y++)
            {
                byte entry = data [y * 512 + x];

                // now read the Color from the palette
                ushort color = (ushort) (
                    data [info.ClutY * 512 + ((clutx + entry) * 2)] |
                    (data [info.ClutY * 512 + ((clutx + entry) * 2 + 1)] << 8)
                );

                int red = (int) (((color & 0x1F) / 31F) * 255);
                int green = (int) ((((color >> 5) & 0x1F) / 31F) * 255);
                int blue = (int) ((((color >> 10) & 0x1F) / 31F) * 255);
                int a = ((color >> 15) & 0x1);
                int alpha = 255;

                if (info.Blend == 0 && (info.Color == 0 || info.Color == 1))
                {
                    if (a == 1)
                        alpha = 127;
                    else
                        alpha = 255;

                    if (red == 0 && green == 0 && blue == 0 && a == 0)
                        alpha = 0;
                }
                else if (info.Blend == 1 && (info.Color == 0 || info.Color == 1))
                {
                    if (red == 0 && green == 0 && blue == 0 && a == 0)
                        alpha = 0;
                }

                // Colors are in rgb15 format
                Color c = Color.FromArgb (
                    alpha,
                    red,
                    green,
                    blue
                );
                
                bmp.SetPixel (x, y, c);
            }
        }

        return bmp;
    }

    public static Bitmap Create16bpp (byte [] data, VertexTexInfo info)
    {
        Bitmap bmp = new Bitmap (256, 128);

        // ClutX really contains the count of palettes available
        // so multiplying by 16 gives the right amount of info for it
        int clutx = info.ClutX * 16;
        
        // write the texture somewhere for now, testing
        for (int x = 0; x < 256; x++)
        {
            for (int y = info.ClutY; y < 128; y++)
            {
                // now read the Color from the palette
                ushort color = (ushort) (
                    data [(info.ClutY + y) * 512 + ((clutx + x) * 2)] |
                    (data [(info.ClutY + y) * 512 + ((clutx + x) * 2 + 1)] << 8)
                );

                // this is not 100% precise, but we can fix these manually
                // as they're usually not that big
                // Colors are in rgb15 format
                Color c = Color.FromArgb (
                    ((color >> 15) & 0x1) * 255,
                    (int) (((color & 0x1F) / 31F) * 255),
                    (int) ((((color >> 5) & 0x1F) / 31F) * 255),
                    (int) ((((color >> 10) & 0x1F) / 31F) * 255)
                );
                
                bmp.SetPixel (x, y, c);
            }
        }

        return bmp;
    }

    public static Bitmap CreateTexture (byte [] data, VertexTexInfo info)
    {
        return info.Color switch
        {
            0 => TextureExporter.Create4bpp (data, info),
            1 => TextureExporter.Create8bpp (data, info),
            _ => TextureExporter.Create16bpp (data, info)
        };
    }
}