using OpenTK.Graphics;

namespace CrashEdit
{
    public static class Color4Ext
    {
        public static Color4 Lerp(Color4 col1, Color4 col2, float amt)
        {
            if (amt <= 0) return col1;
            if (amt >= 1) return col2;
            return new Color4(
                col1.R + (col2.R - col1.R) * amt,
                col1.G + (col2.G - col1.G) * amt,
                col1.B + (col2.B - col1.B) * amt,
                col1.A + (col2.A - col1.A) * amt
                );
        }
    }
}
