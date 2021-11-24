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
        public int VertCount { get; private set; }

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

        public void UpdateAttrib<T>(string name, T[] data, int eltsize, int eltcount) where T : struct
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
                GL.BufferData(BufferTarget.ArrayBuffer, data.Length * eltsize, data, BufferUsageHint.DynamicDraw);
                // setup the position attribute.
                GL.VertexAttribPointer(loc, eltcount, VertexAttribPointerType.Float, false, 0, 0);
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
            UpdateAttrib("position", positions, 16, 4);
            VertCount = positions.Length;
        }

        public void UpdatePositions(Vector3[] positions)
        {
            UpdateAttrib("position", positions, 12, 3);
            VertCount = positions.Length;
        }

        public void UpdateNormals(Vector3[] positions)
        {
            UpdateAttrib("normal", positions, 12, 3);
            VertCount = positions.Length;
        }

        public void UpdateColors(Color4[] colors)
        {
            UpdateAttrib("color", colors, 16, 4);
            VertCount = colors.Length;
        }

        public void UpdateUVs(Vector2[] uvs)
        {
            UpdateAttrib("uv", uvs, 8, 2);
            VertCount = uvs.Length;
        }

        public void GLDispose()
        {
            foreach (var buf in Buffers.Values)
                GL.DeleteBuffer(buf);
            GL.DeleteVertexArray(ID);
        }

        public void Render(RenderInfo ri)
        {
            // Bind the VAO
            GL.BindVertexArray(ID);

            // Use/Bind the program
            Shader.Render(ri);

            // This draws the triangle.
            GL.DrawArrays(Primitive, 0, VertCount);

            if (ri.Projection.ColorModeStack.Count > 0)
            {
                ri.Projection.PopColorMode();
            }
        }
    }
}
