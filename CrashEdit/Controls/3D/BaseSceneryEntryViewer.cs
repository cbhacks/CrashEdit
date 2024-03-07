using CrashEdit.Crash;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace CrashEdit.CE
{
    public abstract class BaseSceneryEntryViewer<T> : GLViewer where T : class, IEntry
    {
        private List<int> worlds;

        private static VBO vboWorld;
        protected VAO vaoWorld;
        protected Vector3 world_offset;
        protected BlendMode blend_mask;

        public BaseSceneryEntryViewer(NSF nsf, int world) : base(nsf)
        {
            worlds = [world];
        }

        public BaseSceneryEntryViewer(NSF nsf, IEnumerable<int> worlds) : base(nsf)
        {
            this.worlds = new(worlds);
        }

        private static void LoadGLStatic()
        {
            vboWorld = new VBO();
        }

        protected override void LoadGL()
        {
            base.LoadGL();

            vaoWorld = new(shaders.GetShader("crash1"), PrimitiveType.Triangles, vboWorld);
        }

        protected void SetWorlds(IEnumerable<int> worlds)
        {
            this.worlds = new(worlds);
        }

        protected List<T?> GetWorlds()
        {
            var list = new List<T?>();
            foreach (int eid in worlds)
            {
                T? world = nsf.GetEntry<T>(eid);
                if (!list.Contains(world))
                    list.Add(world);
            }
            return list;
        }

        protected abstract void SetWorldOffset(T world);

        protected override void Render()
        {
            base.Render();

            // setup textures
            CollectTPAGs();
            UploadTPAGs();

            blend_mask = BlendMode.Solid;
        }

        protected abstract void RenderWorld(T world);

        protected void RenderPasses()
        {
            // render passes
            RenderWorldPass(BlendMode.Solid);
            if (render.EnableTexture)
            {
                RenderWorldPass(BlendMode.Trans);
                RenderWorldPass(BlendMode.Subtractive);
                RenderWorldPass(BlendMode.Additive);
            }
        }

        protected void RenderWorldPass(BlendMode pass)
        {
            if ((pass & blend_mask) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoWorld.BlendMask = BlendModeIndex(pass);
                vaoWorld.Render(render);
            }
        }

        protected override void Dispose(bool disposing)
        {
            vaoWorld?.Dispose();

            base.Dispose(disposing);
        }
    }
}
