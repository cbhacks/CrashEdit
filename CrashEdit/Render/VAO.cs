using OpenTK.Graphics.OpenGL4;
using OpenTK.Graphics;
using OpenTK;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public class VAO : IDisposable
    {
        public int ID { get; }

        private Dictionary<string, int> Buffers { get; } = new Dictionary<string, int>();

        public Shader Shader { get; }

        public PrimitiveType Primitive { get; set; }
        public int VertCount { get; set; }

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

        public void UpdatePositions(Vector4[] positions, int eltc = -1) => UpdateAttrib(0, "position", positions, 16, 4, eltc);
        public void UpdatePositions(Vector3[] positions, int eltc = -1) => UpdateAttrib(0, "position", positions, 12, 3, eltc);
        public void UpdateNormals(Vector3[] positions, int eltc = -1) => UpdateAttrib(0, "normal", positions, 12, 3, eltc);
        public void UpdateColors(Color4[] colors, int eltc = -1) => UpdateAttrib(0, "color", colors, 16, 4, eltc);
        public void UpdateUVs(Vector2[] uvs, int eltc = -1) => UpdateAttrib(0, "uv", uvs, 8, 2, eltc);

        public void Dispose()
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
            Shader.Render(ri, this);

            // This draws the triangle.
            GL.DrawArrays(Primitive, 0, vertcount == -1 ? VertCount : vertcount);
        }

        #region USER DATA (these can be whatever you want)
        public Vector3 UserTrans;
        public Vector3 UserRot;
        public Vector3 UserScale;
        public Quaternion UserQuat;
        public Vector4 UserAxis;
        public Color4 UserColor1;
        public Color4 UserColor2;
        public Matrix3 UserMat3;
        public Vector3 UserColorAmb;
        public Vector3 UserColorDiff;
        public float UserScaleScalar;
        public int UserCullMode; // 0 - default, 1 - backface, 2 - no cull

        public enum ColorModeEnum { Default = 0, GradientY = 1, Solid = 2 };
        public ColorModeEnum ColorMode;

        public enum ArtTypeEnum { Crash1World = 0, Crash1Anim = 1 };
        public ArtTypeEnum ArtType;

        public int BlendMask;
        #endregion
    }
}
