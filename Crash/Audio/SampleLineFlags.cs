using System;

namespace Crash.Audio
{
    [Flags]
    public enum SampleLineFlags : byte
    {
        None = 0,
        LoopEnd = 1,
        Unknown = 2,
        LoopStart = 4
    }
}
