using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CrashEdit
{
    public class VAO : IDisposable
    {
        private Vertex[] verts;
        private Stopwatch watch = new();

        public int ID { get; }
        public int Buffer { get; }

        public Shader Shader { get; }

        public PrimitiveType Primitive { get; set; }
        public Vertex[] Verts { get => verts; }
        public int VertCount { get; set; }

        internal void EnableAttrib(string attrib_name, int size, VertexAttribPointerType type, bool normalized, string field_name)
        {

            int temp = GL.GetAttribLocation(Shader.ID, attrib_name);
            if (temp == -1)
            {
                Console.WriteLine($"in shader {Shader.Name} did not find attrib {attrib_name}");
            }
            else
            {
                GL.EnableVertexAttribArray(temp);
                GL.VertexAttribPointer(temp, size, type, normalized, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf<Vertex>(field_name));
            }
        }

        internal void EnableAttribI(string attrib_name, int size, VertexAttribIntegerType type, string field_name)
        {

            int temp = GL.GetAttribLocation(Shader.ID, attrib_name);
            if (temp == -1)
            {
                Console.WriteLine($"in shader {Shader.Name} did not find attrib {attrib_name}");
            }
            else
            {
                GL.EnableVertexAttribArray(temp);
                GL.VertexAttribIPointer(temp, size, type, Marshal.SizeOf<Vertex>(), Marshal.OffsetOf<Vertex>(field_name));
            }
        }

        public VAO(ShaderContext shaders, string shadername, PrimitiveType prim, int vert_count = 1024)
        {
            Shader = shaders.GetShader(shadername);
            Primitive = prim;
            verts = new Vertex[vert_count];

            // Create the vertex array object (VAO) and VBO for the program.
            ID = GL.GenVertexArray();
            Buffer = GL.GenBuffer();

            // set up the array
            GL.BindVertexArray(ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vert_count * Marshal.SizeOf<Vertex>(), IntPtr.Zero, BufferUsageHint.DynamicDraw);
            EnableAttrib("position", 3, VertexAttribPointerType.Float, false, "trans");
            EnableAttrib("uv", 2, VertexAttribPointerType.Float, false, "st");
            EnableAttrib("normal", 3, VertexAttribPointerType.Float, false, "normal");
            EnableAttrib("color", 4, VertexAttribPointerType.UnsignedByte, true, "rgba");
            EnableAttribI("tex", 1, VertexAttribIntegerType.Int, "tex");
        }

        public void TestRealloc() => TestRealloc(VertCount);
        public void TestRealloc(int vert_count)
        {
            if (vert_count >= Verts.Length)
            {
                Console.WriteLine($"Realloc buffer {Verts.Length} -> {Verts.Length * 2}");
                Array.Resize(ref verts, Verts.Length * 2);
                TestRealloc(vert_count);
            }
        }

        public void CopyAttrib(int idx)
        {
            TestRealloc();
            Verts[VertCount] = Verts[idx];
            VertCount++;
        }

        public void PushAttrib(Vector3? trans = null, Vector3? normal = null, Vector2? st = null, Rgba? rgba = null, TexInfoUnpacked? tex = null)
        {
            TestRealloc();
            if (trans != null)
                Verts[VertCount].trans = trans.Value;
            if (normal != null)
                Verts[VertCount].normal = normal.Value;
            if (st != null)
                Verts[VertCount].st = st.Value;
            if (rgba != null)
                Verts[VertCount].rgba = rgba.Value;
            if (tex != null)
                Verts[VertCount].tex = (int)tex.Value;
            VertCount++;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Buffer);
            GL.DeleteVertexArray(ID);
        }

        public void DiscardVerts()
        {
            VertCount = 0;
        }

        public void Render(RenderInfo ri)
        {
            if (ri == null)
            {
                throw new ArgumentException("null render context");
            }

            if (VertCount <= 0)
                return;

            watch.Restart();

            var backup_state = GLViewer.glDebugContextString;
            GLViewer.glDebugContextString = "vao " + Shader.Name;

            GL.GetBoolean(GetPName.DepthWritemask, out bool glZBufWrite);
            GL.GetBoolean(GetPName.DepthTest, out bool glZBufRead);
            GL.GetFloat(GetPName.LineWidth, out float glLineWidth);
            if (glZBufWrite && ZBufDisable)
            {
                GL.DepthMask(false);
            }
            if (glZBufRead && ZBufDisableRead)
            {
                GL.Disable(EnableCap.DepthTest);
            }
            if (LineWidth > 0)
            {
                GL.LineWidth(LineWidth);
            }

            // Bind the VAO
            GL.BindVertexArray(ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, VertCount * Marshal.SizeOf<Vertex>(), Verts, BufferUsageHint.DynamicDraw);

            Shader.Render(ri, this);
            GL.DrawArrays(Primitive, 0, VertCount);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            if (glZBufWrite && ZBufDisable)
            {
                GL.DepthMask(true);
            }
            if (glZBufRead && ZBufDisableRead)
            {
                GL.Enable(EnableCap.DepthTest);
            }
            if (LineWidth > 0)
            {
                GL.LineWidth(glLineWidth);
            }

            GLViewer.glDebugContextString = backup_state;

            ri.DebugRenderMs += watch.StopAndElapsedMillisecondsFull();
        }

        public void RenderAndDiscard(RenderInfo ri)
        {
            Render(ri);
            DiscardVerts();
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
        public int UserCullMode; // 0 - no cull, 1 - backface (default), 2 - frontface

        public enum ColorModeEnum { Default = 0, GradientY = 1, Solid = 2 };
        public ColorModeEnum ColorMode;

        public int BlendMask;

        public bool ZBufDisable;
        public bool ZBufDisableRead;
        public bool ZBufDisableWrite;
        public float LineWidth;
        #endregion
    }
}
