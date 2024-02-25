using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CrashEdit.CE
{
    public sealed class VAO : IDisposable
    {
        private readonly Stopwatch watch = new();

        public int ID { get; }

        public VBO VBO { get; }

        public Shader Shader { get; }

        public PrimitiveType Primitive { get; set; }

        public Vertex[] Verts => VBO.Verts;
        public int CurVert { get => VBO.CurVert; set => VBO.CurVert = value; }

        private void EnableAttrib(string attrib_name, int size, VertexAttribPointerType type, bool normalized, string field_name)
        {
            int temp = GL.GetAttribLocation(Shader.ID, attrib_name);
            if (temp == -1)
            {
                Console.WriteLine($"in shader {Shader.Name} did not find attrib {attrib_name}");
            }
            else
            {
                GL.EnableVertexAttribArray(temp);
                GL.VertexAttribPointer(temp, size, type, normalized, Vertex.SIZEOF, Marshal.OffsetOf<Vertex>(field_name));
            }
        }

        private void EnableAttribI(string attrib_name, int size, VertexAttribIntegerType type, string field_name)
        {
            int temp = GL.GetAttribLocation(Shader.ID, attrib_name);
            if (temp == -1)
            {
                Console.WriteLine($"in shader {Shader.Name} did not find attrib {attrib_name}");
            }
            else
            {
                GL.EnableVertexAttribArray(temp);
                GL.VertexAttribIPointer(temp, size, type, Vertex.SIZEOF, Marshal.OffsetOf<Vertex>(field_name));
            }
        }

        public VAO(Shader shader, PrimitiveType prim, VBO buffer)
        {
            Shader = shader;
            Primitive = prim;

            // Create the vertex array object (VAO) and assign VBO
            ID = GL.GenVertexArray();
            VBO = buffer;

            // set up the array
            GL.BindVertexArray(ID);
            VBO.Bind();
            EnableAttrib("position", 3, VertexAttribPointerType.Float, false, "trans");
            EnableAttrib("uv", 2, VertexAttribPointerType.Float, false, "st");
            EnableAttrib("normal", 4, VertexAttribPointerType.Int2101010Rev, true, "normal");
            EnableAttrib("color", 4, VertexAttribPointerType.UnsignedByte, true, "rgba");
            EnableAttribI("tex", 2, VertexAttribIntegerType.Short, "tex");
            EnableAttrib("misc", 4, VertexAttribPointerType.Float, false, "misc");
        }

        public VAO(VAO other)
        {
            Shader = other.Shader;
            Primitive = other.Primitive;

            // Create the vertex array object (VAO), but no buffer (that will be sourced from the other VAO)
            ID = GL.GenVertexArray();
            VBO = other.VBO;

            // set up the array
            GL.BindVertexArray(ID);
            VBO.Bind();
            EnableAttrib("position", 3, VertexAttribPointerType.Float, false, "trans");
            EnableAttrib("uv", 2, VertexAttribPointerType.Float, false, "st");
            EnableAttrib("normal", 4, VertexAttribPointerType.Int2101010Rev, true, "normal");
            EnableAttrib("color", 4, VertexAttribPointerType.UnsignedByte, true, "rgba");
            EnableAttribI("tex", 2, VertexAttribIntegerType.Short, "tex");
            EnableAttrib("misc", 4, VertexAttribPointerType.Float, false, "misc");
        }

        public void TestRealloc() => VBO.TestRealloc();
        public void TestRealloc(int vert_count)
        {
            VBO.TestRealloc(vert_count);
        }

        public void CopyAttrib(int idx)
        {
            TestRealloc();
            Verts[VBO.CurVert] = Verts[idx];
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, Rgba rgba = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].rgba = rgba;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, Vector2 st = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].st = st;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, Vector2 st = default, Rgba rgba = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].st = st;
            Verts[VBO.CurVert].rgba = rgba;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, Vector2 st = default, Rgba rgba = default, Vector4 misc = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].st = st;
            Verts[VBO.CurVert].rgba = rgba;
            Verts[VBO.CurVert].misc = misc;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, Vector4 misc = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].misc = misc;
            VBO.CurVert++;
        }

        public void PushAttrib(Vector3 trans = default, int normal = default, Vector2 st = default, Rgba rgba = default, VertexTexInfo tex = default, Vector4 misc = default)
        {
            TestRealloc();
            Verts[VBO.CurVert].trans = trans;
            Verts[VBO.CurVert].normal = normal;
            Verts[VBO.CurVert].st = st;
            Verts[VBO.CurVert].rgba = rgba;
            Verts[VBO.CurVert].tex = tex;
            Verts[VBO.CurVert].misc = misc;
            VBO.CurVert++;
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(ID);
        }

        public void DiscardVerts()
        {
            VBO.CurVert = 0;
        }

        public void Render(RenderInfo ri)
        {
            if (VBO.CurVert <= 0)
                return;

            if (ri == null)
            {
                throw new ArgumentException("null render context");
            }

            watch.Restart();

            GLViewer.dbgContextDir.Add(Shader.Name);

            GL.GetFloat(GetPName.LineWidth, out float glLineWidth);
            GL.GetBoolean(GetPName.DepthTest, out bool glZBufRead);
            GL.GetBoolean(GetPName.DepthWritemask, out bool glZBufWrite);
            if (LineWidth > 0)
            {
                GL.LineWidth(LineWidth);
            }
            if (glZBufRead && ZBufDisableRead)
            {
                GL.Disable(EnableCap.DepthTest);
            }
            if (glZBufWrite && ZBufDisable)
            {
                GL.DepthMask(false);
            }

            // Bind the VAO
            GL.BindVertexArray(ID);
            VBO.Upload();
            Shader.Render(ri, this);
            GL.DrawArrays(Primitive, 0, VBO.CurVert);

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

            GLViewer.dbgContextDir.RemoveLast();

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
        public Matrix3 UserMat3;
        public Vector3 UserColorAmb;
        public Vector3 UserColorDiff;
        public int UserCullMode; // 0 - no cull, 1 - backface (default), 2 - frontface
        public float UserFloat;
        public float UserFloat2;

        public int BlendMask;

        public bool ZBufDisable;
        public bool ZBufDisableRead;
        public bool ZBufDisableWrite;
        public float LineWidth;
        #endregion
    }
}
