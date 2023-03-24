using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;
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
    }

    [StructLayout(LayoutKind.Explicit, Size = 64)]
    public struct Vertex
    {
        [FieldOffset(00)] public Vector3 trans;
        [FieldOffset(12)] public Rgba rgba;
        [FieldOffset(16)] public Vector3 normal;
        [FieldOffset(32)] public Vector2 st;
        [FieldOffset(40)] public int tex;
    }
}
