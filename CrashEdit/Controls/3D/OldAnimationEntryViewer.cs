using CrashEdit.CE.Properties;
using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed class OldAnimationEntryViewer : BaseAnimationEntryViewer
    {
        private readonly OldAnimationRenderer animation_renderer;

        private bool colored;
        private bool _normals = Settings.Default.DisplayNormals;

        public OldAnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1) : base(nsf, anim_eid, frame)
        {
            animation_renderer = new() { TPages = tpages, Render = render };
            animation_renderer.DisableLighting = true;
        }

        private List<OldFrame>? GetFrames()
        {
            List<OldFrame>? frames = null;
            {
                var entry = nsf.GetEntry<Entry>(animId);
                if (entry is OldAnimationEntry svtx)
                {
                    frames = svtx.Frames;
                    colored = false;
                }
                else if (entry is ColoredAnimationEntry cvtx)
                {
                    frames = cvtx.Frames;
                    colored = true;
                }
            }
            return frames;
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var frames = GetFrames();
                if (frames != null)
                {
                    var usedframes = new List<OldFrame>();
                    if (animFrame != -1)
                        usedframes.Add(frames[animFrame]);
                    else
                        usedframes.AddRange(frames);

                    foreach (OldFrame frame in usedframes)
                    {
                        var model = nsf.GetEntry<OldModelEntry>(frame.ModelEID);
                        float mx = 1 / 128f;
                        float my = 1 / 128f;
                        float mz = 1 / 128f;
                        if (model != null)
                        {
                            mx = model.ScaleX / GameScales.ModelC1 / GameScales.AnimC1;
                            my = model.ScaleY / GameScales.ModelC1 / GameScales.AnimC1;
                            mz = model.ScaleZ / GameScales.ModelC1 / GameScales.AnimC1;
                        }
                        var frame_offset = new Position(frame.XOffset, frame.YOffset, frame.ZOffset) - new Position(128, 128, 128);
                        var scale = new Position(mx, my, mz);
                        foreach (var vert in frame.Vertices)
                        {
                            yield return (new Position(vert.X, vert.Y, vert.Z) + frame_offset) * scale;
                        }
                    }
                }

                yield return new Position(0, 0, 0);
            }
        }

        protected override void Render()
        {
            base.Render();

            animation_renderer.Setup(_interpolate);

            if (animation_renderer.RenderAnimFrame(new Vector3(0), vaoModel, nsf.GetEntry<Entry>(animId), animFrame != -1 ? animFrame : render.FullCurrentFrame / 2, x => nsf.GetEntry<OldModelEntry>(x.ModelEID)))
            {
                UploadTPAGs();

                vaoModel[0].BlendModes |= animation_renderer.BlendMask;

                if (_normals && !colored)
                {
                    for (int i = 0; i < vaoModel[0].CurVert; ++i)
                    {
                        vaoLines.PushAttrib(trans: vaoModel[0].Verts[i].trans, rgba: (Rgba)Color4.Red);
                        vaoLines.PushAttrib(trans: vaoModel[0].Verts[i].trans + Vertex.UnpackNormal(vaoModel[0].Verts[i].normal) * 0.1f, rgba: (Rgba)Color4.Cyan);
                    }
                }

                RenderPasses();

                if (_collision)
                {
                    var frame = animation_renderer.BaseFrame!;
                    var c1 = new Vector3(frame.collision.X1, frame.collision.Y1, frame.collision.Z1) / GameScales.CollisionC1;
                    var c2 = new Vector3(frame.collision.X2, frame.collision.Y2, frame.collision.Z2) / GameScales.CollisionC1;
                    var ct = new Vector3(frame.collision.XOffset, frame.collision.YOffset, frame.collision.ZOffset) / GameScales.CollisionC1;
                    var pos = c1 + ct;
                    var size = c2 - c1;
                    AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                    AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleNormals.Print(OnOffName(_normals));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleNormals)) _normals = !_normals;
        }
    }
}
