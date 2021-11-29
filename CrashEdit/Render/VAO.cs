using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public class VAO : IGLDisposable
    {
        public int ID { get; }

        private Dictionary<string, int> Buffers { get; } = new Dictionary<string, int>();

        public Shader Shader { get; }

        public PrimitiveType Primitive { get; }
        public int VertCount { get; set; }

        public static int GetPrimCount(PrimitiveType primitive, int verts)
        {
            switch (primitive)
            {
                case PrimitiveType.Points: return verts;
                case PrimitiveType.Lines: return verts / 2;
                case PrimitiveType.Triangles: return verts / 3;
                case PrimitiveType.Quads: return verts / 4;
                case PrimitiveType.LineStrip: return verts - 1;
                case PrimitiveType.LineLoop: return verts;
                case PrimitiveType.TriangleFan: return verts - 2;
                case PrimitiveType.TriangleStrip: return verts - 2;
                default: throw new Exception(string.Format("invalid primitive type {0}", primitive));
            }
        }

        public VAO(ShaderContext context, string shadername, PrimitiveType prim)
        {
            Shader = context.GetShader(shadername);
            Primitive = prim;

            // Create the vertex array object (VAO) for the program.
            ID = GL.GenVertexArray();
        }

        public void UpdateAttrib<T>(int etype, string name, T[] data, int eltsize, int components, int eltcount = -1) where T : struct
        {
            int loc = GL.GetAttribLocation(Shader.ID, name);

            if (loc != -1)
            {
                GL.BindVertexArray(ID);

                // Create the vertex buffer object (VBO) for the data.
                if (!Buffers.ContainsKey(name))
                {
                    Buffers.Add(name, GL.GenBuffer());
                }
                int buf = Buffers[name];
                // Bind the VBO and copy the data into it.
                GL.BindBuffer(BufferTarget.ArrayBuffer, buf);
                GL.BufferData(BufferTarget.ArrayBuffer, (eltcount == -1 ? data.Length : eltcount) * eltsize, data, BufferUsageHint.DynamicDraw);
                // setup the position attribute.
                if (etype == 1)
                {
                    GL.VertexAttribIPointer(loc, components, VertexAttribIntegerType.Int, 0, IntPtr.Zero);
                }
                else if (etype == 2)
                {
                    GL.VertexAttribIPointer(loc, components, VertexAttribIntegerType.UnsignedShort, 0, IntPtr.Zero);
                }
                else if (etype == 3)
                {
                    GL.VertexAttribIPointer(loc, components, VertexAttribIntegerType.UnsignedByte, 0, IntPtr.Zero);
                }
                else if (etype == 0)
                {
                    GL.VertexAttribPointer(loc, components, VertexAttribPointerType.Float, false, 0, 0);
                }
                GL.EnableVertexAttribArray(loc);

                GL.BindVertexArray(0);
            }
            else
            {
                Console.WriteLine($"Shader attrib {name} not found in shader {Shader.Name}");
            }
        }

        public void UpdatePositions(Vector4[] positions)
        {
            UpdateAttrib(0, "position", positions, 16, 4);
        }

        public void UpdatePositions(Vector3[] positions)
        {
            UpdateAttrib(0, "position", positions, 12, 3);
        }

        public void UpdateNormals(Vector3[] positions)
        {
            UpdateAttrib(0, "normal", positions, 12, 3);
        }

        public void UpdateColors(Color4[] colors)
        {
            UpdateAttrib(0, "color", colors, 16, 4);
        }

        public void UpdateUVs(Vector2[] uvs)
        {
            UpdateAttrib(0, "uv", uvs, 8, 2);
        }

        public void GLDispose()
        {
            foreach (var buf in Buffers.Values)
                GL.DeleteBuffer(buf);
            GL.DeleteVertexArray(ID);
        }

        public void Render(RenderInfo ri, int vertcount = -1)
        {
            // Bind the VAO
            GL.BindVertexArray(ID);

            // Use/Bind the program
            Shader.Render(ri);

            // This draws the triangle.
            GL.DrawArrays(Primitive, 0, vertcount == -1 ? VertCount : vertcount);

            if (ri.Projection.ColorModeStack.Count > 0)
            {
                ri.Projection.PopColorMode();
            }
        }
    }
}
