﻿using OpenTK.Graphics;
using OpenTK;

namespace CrashEdit
{
    public static class MathExt
    {
        public static float Lerp (float a, float b, float amt)
        {
            if (amt <= 0) return a;
            if (amt >= 1) return b;
            return (b - a) * amt + a;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float amt)
        {
            if (amt <= 0) return a;
            if (amt >= 1) return b;
            return a + (b - a) * amt;
        }
        public static Vector3 Div(Vector3 a, Vector3 b)
        {
            a.X /= b.X;
            a.Y /= b.Y;
            a.Z /= b.Z;
            return a;
        }

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

        public static Rgba Lerp(Rgba col1, Rgba col2, float amt)
        {
            if (amt <= 0) return col1;
            if (amt >= 1) return col2;
            return new Rgba(
                (byte)(col1.r + (col2.r - col1.r) * amt),
                (byte)(col1.g + (col2.r - col1.g) * amt),
                (byte)(col1.b + (col2.r - col1.b) * amt),
                (byte)(col1.a + (col2.r - col1.a) * amt)
                );
        }

        public static int Log2(int v)
        {
            for (int i = 31; i-- > 0;)
            {
                if (v >= (1 << i)) return i;
            }
            return -1;
        }

        public static uint Log2(uint v)
        {
            for (int i = 32; i-- > 0;)
            {
                if (v >= (1 << i)) return (uint)i;
            }
            return uint.MaxValue;
        }
    }
}
