using CrashEdit.CE.Properties;
using CrashEdit.Crash;
using OpenTK.Graphics.OpenGL4;

namespace CrashEdit.CE
{
    public abstract class BaseAnimationEntryViewer : GLViewer
    {
        protected int animId;
        protected int animFrame;
        protected int modelId;

        protected int _curframe = 0;
        protected int _cullmode = 1;
        protected bool _collision = Settings.Default.DisplayFrameCollision;
        protected bool _interpolate = true;

        private static VBO[] vboModel = new VBO[2];
        protected VAO[] vaoModel = new VAO[2];

        public BaseAnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1, int model_eid = Entry.NullEID) : base(nsf)
        {
            animId = anim_eid;
            animFrame = frame;
            modelId = model_eid;
        }

        private static void LoadGLStatic()
        {
            for (int i = 0; i < vboModel.Length; ++i)
            {
                vboModel[i] = new();
            }
        }

        protected override void LoadGL()
        {
            base.LoadGL();

            for (int i = 0; i < vaoModel.Length; ++i)
            {
                vaoModel[i] = new(shaders.GetShader("crash1"), PrimitiveType.Triangles, vboModel[i]);
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleCollisionAnim.Print(OnOffName(_collision));
            con_help += KeyboardControls.ToggleLerp.Print(OnOffName(_interpolate));
            con_help += KeyboardControls.ChangeCullMode.Print(CullModeName(_cullmode));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleCollisionAnim)) _collision = !_collision;
            if (KPress(KeyboardControls.ToggleLerp)) _interpolate = !_interpolate;
            if (KPress(KeyboardControls.ChangeCullMode)) _cullmode = ++_cullmode % 3;
        }

        protected override void CollectTPAGs()
        {
            throw new NotImplementedException();
        }

        protected void RenderPasses()
        {
            // note: only buffer 0 is rendered
            vaoModel[0].UserCullMode = _cullmode;
            vaoModel[0].BlendModes |= BlendMode.Solid;
            RenderFramePass(BlendMode.Solid);
            if (render.EnableTexture)
            {
                RenderFramePass(BlendMode.Trans);
                RenderFramePass(BlendMode.Subtractive);
                RenderFramePass(BlendMode.Additive);
            }

            vaoModel[0].DiscardVerts();
            vaoModel[1].DiscardVerts();
            vaoModel[0].BlendModes = BlendMode.None;
            vaoModel[1].BlendModes = BlendMode.None;
        }

        protected void RenderFramePass(BlendMode pass)
        {
            if ((pass & vaoModel[0].BlendModes) != BlendMode.None)
            {
                SetBlendMode(pass);
                vaoModel[0].BlendMask = BlendModeIndex(pass);
                vaoModel[0].Render(render);
            }
        }

        protected override void Dispose(bool disposing)
        {
            for (var i = 0; i < vaoModel.Length; ++i)
            {
                vaoModel[i]?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
