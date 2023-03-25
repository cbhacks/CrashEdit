using System.Diagnostics;

namespace CrashEdit
{
    public static class StopwatchExt
    {
        public static long ElapsedFrames(this Stopwatch watch) => watch.ElapsedTicks / TicksPerFrame;
        public static double ElapsedFramesFull(this Stopwatch watch) => watch.ElapsedTicks / (double)TicksPerFrame;
        public static long ElapsedFrameTime(this Stopwatch watch) => watch.ElapsedTicks % TicksPerFrame;
        public static long StopAndElapsed(this Stopwatch watch)
        {
            watch.Stop();
            return watch.ElapsedTicks;
        }

        public static long TicksPerFrame => Stopwatch.Frequency / 60;
    }
}
