﻿using OpenTK;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public class RenderInfo
    {
        public ProjectionInfo Projection;
        public ShaderContext ShaderContext;

        public const float InitialDistance = 10;
        public const float MinDistance = 2;
        public const float MaxDistance = 40;

        public const float BaseRot = 0;
        public const float MinRot = BaseRot - MathHelper.PiOver2;
        public const float MaxRot = BaseRot + MathHelper.PiOver2;

        public float Distance { get; set; }

        public void Reset()
        {
            Distance = InitialDistance;
            Projection.Trans = new Vector3(0, Distance, 0);
            Projection.Rot = new Vector3(BaseRot, 0, 0);
            Projection.Scale = new Vector3(1);
        }

        private bool masterexit;
        private long _framecounter;
        private long _framehits;
        private readonly Task _frametask;
        private readonly Timer _frametimer;

        public long MissedFrames => _framecounter - _framehits;
        public long CurrentFrame => _framehits;
        public long RealCurrentFrame => _framecounter;

        public readonly object mLock = new object();

        public RenderInfo(GLViewer parent = null)
        {
            ShaderContext = new ShaderContext();

            // window update
            _frametimer = new Timer();
            _frametimer.Interval = 10;
            _frametimer.Tick += (sender, e) =>
            {
                parent?.Invalidate();
                _frametimer.Enabled = !masterexit;
            };

            // logic update
            _frametask = new Task(() =>
            {
                var cur_rate = OldMainForm.GetRate();
                long nextframetick = StopwatchExt.TicksPerFrame;
                Stopwatch watch = Stopwatch.StartNew();
                while (!masterexit)
                {
                    while (watch.ElapsedTicks <= nextframetick) ;
                    nextframetick += StopwatchExt.TicksPerFrame;

                    lock (mLock)
                    {
                        _framehits++;
                        _framecounter = watch.ElapsedFrames();
                        //Console.WriteLine(string.Format("{0} (f {1})", _framehits, _framecounter));

                        // reset frame counters when rate changes
                        if (cur_rate != OldMainForm.GetRate())
                        {
                            watch.Restart();
                            nextframetick = StopwatchExt.TicksPerFrame;
                            _framehits = 0;
                            cur_rate = OldMainForm.GetRate();
                        }

                        if (parent != null)
                        {
                            parent.RunLogic();
                        }
                    }
                }
            });

            Projection.ColorModeStack = new Stack<ProjectionInfo.ColorModeEnum>();

            Reset();

            Start();
        }

        public void Start()
        {
            _frametask.Start();
            _frametimer.Enabled = true;
        }

        ~RenderInfo()
        {
            masterexit = true;
        }
    }
}
