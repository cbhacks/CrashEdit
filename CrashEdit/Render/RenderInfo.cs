﻿using OpenTK;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrashEdit
{
    public enum RendererMoveMode { Free, Anchored }

    public class RenderInfo : IDisposable
    {

        public ProjectionInfo Projection;
        public RendererMoveMode MoveMode;
        public bool EnableTexture = true;

        public const float MinDistance = GLViewer.DefaultZNear;
        public const float MaxDistance = 100;
        public const float InitialDistance = MaxDistance * 0.075f;

        public const float BaseRot = 0;
        public const float MinRot = BaseRot - MathHelper.PiOver2;
        public const float MaxRot = BaseRot + MathHelper.PiOver2;

        public float Distance { get; set; }

        public void Reset()
        {
            Distance = InitialDistance;
            Projection.Trans = new(0, 0, 0);
            Projection.Rot = new(BaseRot, 0, 0);
            Projection.Scale = new(1);
            MoveMode = RendererMoveMode.Anchored;
        }

        private bool masterexit = true;
        private long _framehits;
        private readonly Task _frametask;
        private readonly Timer _frametimer;
        private readonly Stopwatch _framewatch;

        public long CurrentFrame => _framehits;
        public long RealCurrentFrame => _framewatch.ElapsedFrames();
        public double FullCurrentFrame => _framewatch.ElapsedFramesFull();
        public long MissedFrames => RealCurrentFrame - _framehits;

        public readonly object mLock = new();

        public RenderInfo(GLViewer parent = null)
        {

            // window update
            if (parent != null)
            {
                _frametimer = new();
                _frametimer.Interval = 10;
                _frametimer.Tick += (sender, e) =>
                {
                    parent.Invalidate();
                };
                _frametimer.Enabled = true;
            }

            // logic update
            _framewatch = Stopwatch.StartNew();
            masterexit = false;
            _frametask = new(() =>
            {
                var cur_rate = OldMainForm.GetRate();
                long nextframetick = StopwatchExt.TicksPerFrame;

                while (!masterexit)
                {
                    while (_framewatch.ElapsedTicks <= nextframetick)
                    {
                        if (nextframetick - _framewatch.ElapsedTicks > StopwatchExt.TicksPerFrame / 15)
                        {
                            System.Threading.Thread.Sleep(1);
                        }
                    }
                    nextframetick += StopwatchExt.TicksPerFrame;

                    lock (mLock)
                    {
                        _framehits++;

                        // reset frame counters when rate changes
                        if (cur_rate != OldMainForm.GetRate())
                        {
                            _framewatch.Restart();
                            nextframetick = StopwatchExt.TicksPerFrame;
                            _framehits = 0;
                            cur_rate = OldMainForm.GetRate();
                        }

                        if (parent != null)
                        {
                            parent.BeginRunLogic();
                        }
                    }
                }
            });

            Reset();
            Start();
        }

        public void Start()
        {
            _frametask.Start();
        }

        public void Dispose()
        {
            masterexit = true;

            _frametask.Wait();
            _frametask.Dispose();
            _frametimer?.Dispose();
        }
    }
}
