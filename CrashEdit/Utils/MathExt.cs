﻿using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public static class MathExt
    {
        public static float TruncatePart(this float v)
        {
            return v - (float)Math.Truncate(v);
        }

        public static Vector3 Div(Vector3 a, Vector3 b)
        {
            a.X /= b.X;
            a.Y /= b.Y;
            a.Z /= b.Z;
            return a;
        }

        public static float Lerp(float a, float b, float amt)
        {
            if (amt <= 0) return a;
            if (amt >= 1) return b;
            return (b - a) * amt + a;
        }

        public static void Lerp(ref Vector3 a, Vector3 b, float amt)
        {
            if (amt <= 0) return;
            if (amt >= 1)
            {
                a.X = b.X;
                a.Y = b.Y;
                a.Z = b.Z;
            }
            else
            {
                a.X += (b.X - a.X) * amt;
                a.Y += (b.Y - a.Y) * amt;
                a.Z += (b.Z - a.Z) * amt;
            }
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

        public static void Lerp(ref Rgba a, Rgba b, float amt)
        {
            if (amt <= 0) return;
            if (amt >= 1)
            {
                a.r = b.r;
                a.g = b.g;
                a.b = b.b;
            }
            else
            {
                a.r += (byte)((b.r - a.r) * amt);
                a.g += (byte)((b.g - a.g) * amt);
                a.b += (byte)((b.b - a.b) * amt);
            }
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float amt)
        {
            Lerp(ref a, b, amt);
            return a;
        }

        public static Rgba Lerp(Rgba a, Rgba b, float amt)
        {
            Lerp(ref a, b, amt);
            return a;
        }

        public static Vector3 LerpScale(Vector3 a, Vector3 b, float amt, float amt_min, float amt_max)
        {
            Lerp(ref a, b, (amt - amt_min) / (amt_max - amt_min));
            return a;
        }

        public static Color4 LerpScale(Color4 a, Color4 b, float amt, float amt_min, float amt_max)
        {
            return Lerp(a, b, (amt - amt_min) / (amt_max - amt_min));
        }

        public static float LerpScale(float a, float b, float amt, float amt_min, float amt_max)
        {
            return Lerp(a, b, (amt - amt_min) / (amt_max - amt_min));
        }

        public static void Zero(ref Vector3 v)
        {
            v.X = 0;
            v.Y = 0;
            v.Z = 0;
        }

        public static Vector3 Abs(this Vector3 v)
        {
            v.X = Math.Abs(v.X);
            v.Y = Math.Abs(v.Y);
            v.Z = Math.Abs(v.Z);
            return v;
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

        public static double Pythagoras(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }

        public static double Sec(double theta)
        {
            return Pythagoras(Math.Tan(theta), 1);
        }
    }
}
