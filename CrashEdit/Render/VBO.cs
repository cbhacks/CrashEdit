using OpenTK.Graphics.OpenGL4;

namespace CrashEdit.CE
{
    public sealed class VBO : IDisposable
    {
        private Vertex[] verts;

        public int Buffer { get; }

        public Vertex[] Verts => verts;
        public int CurVert { get; set; }

        public VBO(int initial_vertex_count = 32)
        {
            verts = new Vertex[initial_vertex_count];

            // create VBO
            Buffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, 0, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void TestRealloc() => TestRealloc(CurVert);

        public void TestRealloc(int vert_count)
        {
            if (vert_count >= verts.Length)
            {
                Console.WriteLine($"VBO realloc ({verts.Length} -> {verts.Length * 2})");
                Array.Resize(ref verts, verts.Length * 2);
                TestRealloc(vert_count);
            }
        }

        public void Upload() => Upload(CurVert);

        public void Upload(int vertex_amount)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertex_amount * Vertex.SIZEOF, Verts, BufferUsageHint.DynamicDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Buffer);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Buffer);
        }
    }
}
