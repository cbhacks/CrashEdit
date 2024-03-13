using CrashEdit.Crash;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace CrashEdit.CE
{
    public abstract class BaseSceneryEntryViewer<T> : GLViewer where T : class, IEntry
    {
        private List<int> worlds;

        private static VBO vboWorld;
        private static VBO vboSky;
        protected VAO _vao;
        private VAO vaoWorld;
        private VAO vaoSky;
        protected Vector3 world_offset;

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
            vboSky = new VBO();
        }

        protected override void LoadGL()
        {
            base.LoadGL();

            vaoWorld = new(shaders.GetShader("crash1"), PrimitiveType.Triangles, vboWorld);
            if (HasSky)
            {
                vaoSky = new(shaders.GetShader("crash1"), PrimitiveType.Triangles, vboSky);
                vaoSky.ZBufDisableWrite = true;
            }

            _vao = vaoWorld;
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

        protected virtual bool HasSky => true;

        protected abstract void SetWorldOffset(T world);

        protected override void Render()
        {
            base.Render();

            // setup textures
            CollectTPAGs();
            UploadTPAGs();

            // render skies first, then everything else
            for (int i = HasSky ? 0 : 1; i < 2; ++i)
            {
                bool sky = i == 0;
                _vao = sky ? vaoSky : vaoWorld;
                RenderWorlds(sky);
            }

            _vao = vaoWorld;
        }

        protected abstract void RenderWorlds(bool sky);

        protected abstract void RenderWorld(T world);

        protected void RenderPasses()
        {
            _vao.BlendModes |= BlendMode.Solid;

            // render passes
            RenderWorldPass(BlendMode.Solid);
            if (render.EnableTexture)
            {
                RenderWorldPass(BlendMode.Trans);
                RenderWorldPass(BlendMode.Subtractive);
                RenderWorldPass(BlendMode.Additive);
            }

            // dump all verts, we rendered them!
            _vao.DiscardVerts();
            _vao.BlendModes = BlendMode.None;
        }

        protected void RenderWorldPass(BlendMode pass)
        {
            if ((pass & _vao.BlendModes) != BlendMode.None)
            {
                SetBlendMode(pass);
                _vao.BlendMask = BlendModeIndex(pass);
                _vao.Render(render);
            }
        }

        protected override void Dispose(bool disposing)
        {
            vaoWorld?.Dispose();
            vaoSky?.Dispose();

            base.Dispose(disposing);
        }
    }
}
