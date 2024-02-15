using OpenTK;
using OpenTK.Graphics;
using System.Runtime.InteropServices;

namespace CrashEdit
{
    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct Rgba
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public Rgba(byte red, byte green, byte blue, byte alpha)
        {
            r = red;
            g = green;
            b = blue;
            a = alpha;
        }

        public Rgba(Rgba other, byte alpha)
        {
            r = other.r;
            g = other.g;
            b = other.b;
            a = alpha;
        }

        public Rgba(Color4 other, byte alpha) : this((Rgba)other, alpha)
        {
        }

        public static explicit operator Rgba(Color4 c)
        {
            return new Rgba((byte)(c.R * 255),
                            (byte)(c.G * 255),
                            (byte)(c.B * 255),
                            (byte)(c.A * 255));
        }

        public static explicit operator Color4(Rgba c)
        {
            return new Color4(c.r, c.g, c.b, c.a);
        }
    }

    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct Vector3w
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3w(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3w operator +(Vector3w left, Vector3w right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        public readonly Vector4 ToVec4() => new(X, Y, Z, 0);
    }

    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct VertexTexInfo
    {
        private int info;

        public VertexTexInfo()
        {
            info = 0;
            Page = -1;
        }

        public VertexTexInfo(short page, int color = 0, int blend = 0, int clutx = 0, int cluty = 0, int face = 0)
        {
            info = 0;
            Page = page;
            Color = color;
            Blend = blend;
            ClutX = clutx;
            ClutY = cluty;
            Face = face;
        }

        public VertexTexInfo(int info)
        {
            this.info = info;
        }

        public int Color { readonly get => (info >> 0) & 0x3; set => info = (info & ~(0x3 << 0)) | (value << 0); }
        public int Blend { readonly get => (info >> 2) & 0x3; set => info = (info & ~(0x3 << 2)) | (value << 2); }
        public int ClutX { readonly get => (info >> 4) & 0xF; set => info = (info & ~(0xF << 4)) | (value << 4); }
        public int ClutY { readonly get => (info >> 8) & 0x7F; set => info = (info & ~(0x7F << 8)) | (value << 8); }
        public int Face { readonly get => (info >> 15) & 0x1; set => info = (info & ~(0x1 << 15)) | (value << 15); }
        public short Page { readonly get => (short)(info >> 16); set => info = (info & 0xFFFF) | (value << 16); }
        public readonly bool Enable => Page >= 0;

        public static implicit operator VertexTexInfo(int v)
        {
            return new VertexTexInfo(v);
        }

        public static implicit operator int(VertexTexInfo p)
        {
            return p.info;
        }

        public readonly int Pack() => info;

        public static GLViewer.BlendMode GetBlendMode(int blend)
        {
            switch (blend)
            {
                case 0: return GLViewer.BlendMode.Trans;
                case 1: return GLViewer.BlendMode.Additive;
                case 2: return GLViewer.BlendMode.Subtractive;
                case 3:
                default: return GLViewer.BlendMode.Solid;
            }
        }

        public readonly GLViewer.BlendMode GetBlendMode()
        {
            return GetBlendMode(Blend);
        }
    }


    [StructLayout(LayoutKind.Explicit, Size = SIZEOF)]
    public struct Vertex
    {
        public const int SIZEOF = 64;
        [FieldOffset(00)] public Vector3 trans;
        [FieldOffset(12)] public Rgba rgba;
        [FieldOffset(16)] public Vector2 st;
        [FieldOffset(24)] public int normal;
        [FieldOffset(28)] public int tex;
        [FieldOffset(32)] public Vector4 misc;
        // 48 - 16 bytes free

        public static Vector3 UnpackNormal(int normal)
        {
            return new Vector3((((normal >> 0) & 0x3FF) - 512) / 511f, (((normal >> 10) & 0x3FF) - 512) / 511f, (((normal >> 20) & 0x3FF) - 512) / 511f);
        }

        public static int PackNormal(Vector3 normal)
        {
            return (((int)(normal.X * 511) + 512) << 0) | (((int)(normal.Y * 511) + 512) << 10) | (((int)(normal.Z * 511) + 512) << 20);
        }
    }
}
