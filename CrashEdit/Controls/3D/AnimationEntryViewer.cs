using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed class AnimationEntryViewer : BaseAnimationEntryViewer
    {
        private readonly AnimationRenderer animation_renderer;

        private bool _halfspeed = false;

        public AnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1, int model_eid = Entry.NullEID) : base(nsf, anim_eid, frame, model_eid)
        {
            animation_renderer = new() { TPages = tpages, Render = render };
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var frames = nsf.GetEntry<AnimationEntry>(animId)?.Frames;
                if (frames != null)
                {
                    var usedframes = new List<Frame>();
                    if (animFrame != -1)
                        usedframes.Add(frames[animFrame]);
                    else
                        usedframes.AddRange(frames);

                    foreach (Frame frame in usedframes)
                    {
                        var model = nsf.GetEntry<ModelEntry>(GetModelEID(frame));
                        float mx = 1 / 128f;
                        float my = 1 / 128f;
                        float mz = 1 / 128f;
                        if (model != null)
                        {
                            mx = model.ScaleX / GameScales.ModelC1 / GameScales.AnimC1;
                            my = model.ScaleY / GameScales.ModelC1 / GameScales.AnimC1;
                            mz = model.ScaleZ / GameScales.ModelC1 / GameScales.AnimC1;
                        }
                        var frame_offset = new Position(frame.XOffset / 4f, frame.YOffset / 4f, frame.ZOffset / 4f);
                        var scale = new Position(mx, my, mz);
                        foreach (var vert in frame.MakeVertices(model))
                        {
                            yield return (new Position(vert.X, vert.Z, vert.Y) + frame_offset) * scale;
                        }
                    }
                }
            }
        }

        private int GetModelEID(Frame frame)
        {
            return modelId != Entry.NullEID ? modelId : frame.ModelEID;
        }

        protected override void Render()
        {
            base.Render();

            animation_renderer.Setup(_interpolate, _halfspeed);

            if (animation_renderer.RenderAnimFrame(new Vector3(0), vaoModel, nsf.GetEntry<AnimationEntry>(animId), animFrame != -1 ? animFrame : render.FullCurrentFrame / 2, x => nsf.GetEntry<ModelEntry>(GetModelEID(x))))
            {
                UploadTPAGs();

                var uncompressed_verts = animation_renderer.GetUncompressedVerts();
                if (uncompressed_verts != null)
                {
                    for (int i = 0; i < uncompressed_verts.Length; ++i)
                    {
                        AddSprite(uncompressed_verts[i], new Vector2(0.32f), (Rgba)Color4.Magenta, OldResources.PointTexture);
                    }
                }

                vaoModel[0].BlendModes |= animation_renderer.BlendMask;

                RenderPasses();

                if (_collision)
                {
                    foreach (var col in animation_renderer.BaseFrame.Collision)
                    {
                        var c1 = new Vector3(col.X1, col.Y1, col.Z1) / GameScales.CollisionC1;
                        var c2 = new Vector3(col.X2, col.Y2, col.Z2) / GameScales.CollisionC1;
                        var ct = new Vector3(col.XOffset, col.YOffset, col.ZOffset) / GameScales.CollisionC1;
                        var pos = c1 + ct;
                        var size = c2 - c1;
                        AddBox(pos, size, new Rgba(0, 255, 0, 255 / 5), false);
                        AddBox(pos, size, new Rgba(0, 255, 0, 255), true);
                    }
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleSlowAnim.Print(OnOffName(_halfspeed));
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleSlowAnim)) _halfspeed = !_halfspeed;
        }
    }
}
