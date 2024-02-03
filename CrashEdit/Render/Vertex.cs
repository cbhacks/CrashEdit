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

        public readonly Vector4 ToVec4() => new Vector4(X, Y, Z, 0);
    }

    public struct TexInfoUnpacked
    {
        public bool enable;
        public int color;
        public int blend;
        public int clutx;
        public int cluty;
        public int face;
        public int page;

        public TexInfoUnpacked(bool enable, int color = 0, int blend = 0, int clutx = 0, int cluty = 0, int face = 0, int page = 0)
        {
            this.enable = enable;
            this.color = color;
            this.blend = blend;
            this.clutx = clutx;
            this.cluty = cluty;
            this.face = face;
            this.page = page;
        }

        public static explicit operator TexInfoUnpacked(int v)
        {
            return new TexInfoUnpacked((v & 1) != 0, (v >> 1) & 0x3, (v >> 3) & 0x3, (v >> 5) & 0xf, (v >> 9) & 0x7f, (v >> 16) & 0x1, v >> 17);
        }

        public static int Pack(bool enable, int color = 0, int blend = 0, int clutx = 0, int cluty = 0, int face = 0, int page = 0)
        {
            return (enable ? 1 : 0) | (color << 1) | (blend << 3) | (clutx << 5) | (cluty << 9) | (face << 16) | (page << 17);
        }

        public static explicit operator int(TexInfoUnpacked p)
        {
            return Pack(p.enable, p.color, p.blend, p.clutx, p.cluty, p.face, p.page);
        }

        public static GLViewer.BlendMode GetBlendMode(int blend)
        {
            switch (blend)
            {
                case 0: return GLViewer.BlendMode.Trans;
                case 1: return GLViewer.BlendMode.Additive;
                case 2: return GLViewer.BlendMode.Subtractive;
                default:
                case 3: return GLViewer.BlendMode.Solid;
            }
        }

        public readonly GLViewer.BlendMode GetBlendMode()
        {
            return GetBlendMode(blend);
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct Vertex
    {
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
