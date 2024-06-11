using CrashEdit.CE.Properties;
using CrashEdit.Crash;
using OpenTK.Mathematics;

namespace CrashEdit.CE
{
    public sealed class AnimationEntryViewer : BaseAnimationEntryViewer
    {
        private readonly AnimationRenderer animation_renderer;

        private bool _halfspeed = false;
        private bool _modelautocycle = false;
        private int _modelforceindex = 0;

        public AnimationEntryViewer(NSF nsf, int anim_eid, int frame = -1) : base(nsf, anim_eid, frame)
        {
            animation_renderer = new() { TPages = tpages, Render = render };
        }

        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                var anim = nsf.GetEntry<AnimationEntry>(animId);
                var frames = anim?.Frames;
                if (frames != null)
                {
                    // try to guess if this is a 'one model per frame' animation
                    if (anim.IsNew && frames.Count > 1 && frames.Count == GetCrash3ModelList(anim).Count)
                        _modelautocycle = true;
                    // guess if it's lerped
                    if (AnimIsLerped(anim))
                        _halfspeed = true;

                    var usedframes = new List<Frame>();
                    if (animFrame != -1)
                        usedframes.Add(frames[animFrame]);
                    else
                        usedframes.AddRange(frames);

                    foreach (Frame frame in usedframes)
                    {
                        var model = nsf.GetEntry<ModelEntry>(GetModelEID(anim, frame));
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

        private bool AnimIsLerped(AnimationEntry anim)
        {
            if (anim != null)
            {
                foreach (var gool in nsf.GetEntries<GOOLEntry>())
                {
                    foreach (var group in gool.FrameGroups)
                    {
                        if (group is VertexGroup3 vgroup3)
                        {
                            if (anim.EID == vgroup3.EID)
                            {
                                return vgroup3.Interpolated;
                            }
                        }
                        else if (group is VertexGroup2 vgroup2)
                        {
                            if (anim.EID == vgroup2.EID)
                            {
                                return vgroup2.Interpolated;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private List<int> GetCrash3ModelList(AnimationEntry anim)
        {
            List<int> models = new();
            if (anim != null && anim.IsNew)
            {
                foreach (var gool in nsf.GetEntries<GOOLEntry>())
                {
                    foreach (var group in gool.FrameGroups)
                    {
                        if (group is VertexGroup3 vgroup)
                        {
                            if (anim.EID == vgroup.EID)
                            {
                                if (!models.Contains(vgroup.ModelEID))
                                {
                                    models.Add(vgroup.ModelEID);
                                }
                            }
                        }
                    }
                }
            }
            return models;
        }

        private int GetModelEID(AnimationEntry anim, Frame frame)
        {
            if (anim.IsNew)
            {
                var models = GetCrash3ModelList(anim);
                if (models.Count == 0)
                    return Entry.NullEID;

                if (_modelautocycle)
                    return models[anim.Frames.IndexOf(frame) % models.Count];
                else
                    return models[_modelforceindex % models.Count];
            }
            return frame.ModelEID;
        }

        protected override void Render()
        {
            base.Render();

            animation_renderer.Setup(_interpolate, _halfspeed);

            var anim = nsf.GetEntry<AnimationEntry>(animId);
            if (animation_renderer.RenderAnimFrame(new Vector3(0), vaoModel, anim, animFrame != -1 ? animFrame : render.FullCurrentFrame / 2, x => nsf.GetEntry<ModelEntry>(GetModelEID(anim, x))))
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

        protected override void PrintDebug()
        {
            base.PrintDebug();

            var anim = nsf.GetEntry<AnimationEntry>(animId);
            if (anim != null)
            {
                var models = GetCrash3ModelList(anim);
                con_debug += $"auto? {_modelautocycle} force: {_modelforceindex}\n";
                for (int i = 0; i < models.Count; ++i)
                {
                    con_debug += $"{i}: {Entry.EIDToEName(models[i])}\n";
                }
            }
        }

        protected override void PrintHelp()
        {
            base.PrintHelp();
            con_help += KeyboardControls.ToggleSlowAnim.Print(OnOffName(_halfspeed));
            var anim = nsf.GetEntry<AnimationEntry>(animId);
            if (anim != null && anim.IsNew)
            {
                var models = GetCrash3ModelList(anim);
                if (models.Count > 1)
                {
                    if (anim.Frames.Count > 1 && models.Count == anim.Frames.Count)
                        con_help += KeyboardControls.ToggleModelCycle.Print(OnOffName(_modelautocycle));
                    if (!_modelautocycle)
                        con_help += string.Format(Resources.ViewerControls_PickModel, Entry.EIDToEName(models[_modelforceindex % models.Count]));
                }
            }
        }

        protected override void RunLogic()
        {
            base.RunLogic();
            if (KPress(KeyboardControls.ToggleSlowAnim)) _halfspeed = !_halfspeed;
            var anim = nsf.GetEntry<AnimationEntry>(animId);
            if (anim != null && anim.IsNew)
            {
                var models = GetCrash3ModelList(anim);
                if (models.Count > 1)
                {
                    if (anim.Frames.Count > 1 && models.Count == anim.Frames.Count)
                        if (KPress(KeyboardControls.ToggleModelCycle)) _modelautocycle = !_modelautocycle;
                    if (!_modelautocycle)
                    {
                        if (KPress(Keys.Left))
                            --_modelforceindex;
                        if (KPress(Keys.Right))
                            ++_modelforceindex;
                        while (_modelforceindex < 0)
                        {
                            _modelforceindex += models.Count;
                        }
                        _modelforceindex = _modelforceindex % models.Count;
                    }
                }
            }
        }
    }
}
