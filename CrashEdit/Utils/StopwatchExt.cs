using System.Diagnostics;

namespace CrashEdit
{
    public static class StopwatchExt
    {
        public static long ElapsedFrames(this Stopwatch watch) => watch.ElapsedTicks / TicksPerFrame;
        public static double ElapsedFramesFull(this Stopwatch watch) => watch.ElapsedTicks / (double)TicksPerFrame;
        public static long ElapsedFrameTime(this Stopwatch watch) => watch.ElapsedTicks % TicksPerFrame;
        public static double ElapsedMillisecondsFull(this Stopwatch watch) => watch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
        public static long StopAndElapsed(this Stopwatch watch)
        {
            watch.Stop();
            return watch.ElapsedTicks;
        }
        public static double StopAndElapsedMillisecondsFull(this Stopwatch watch)
        {
            watch.Stop();
            return watch.ElapsedMillisecondsFull();
        }

        public static long TicksPerFrame => Stopwatch.Frequency / 60;
    }
}
