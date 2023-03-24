using Crash;
using System;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class DummyTopGLViewer : GLViewer, IDisposable
    {

        protected override bool UseGrid => true;

        public DummyTopGLViewer() : base()
        {
        }
        protected override IEnumerable<IPosition> CorePositions
        {
            get
            {
                yield return new Position(0, 0, 0);
            }
        }
    }
}
