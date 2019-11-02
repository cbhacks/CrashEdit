using Crash;

namespace CrashHacks.Scripts.Scenery.Color
{
    public sealed class GrayscaleScript : BaseScript
    {
        public override string Name
        {
            get { return "Make scenery colors grayscale"; }
        }

        public override string Description
        {
            get { return "Changes the colors used in scenery vertex coloring to grayscale by setting each color channel to the average of all of the color channels. Color introduced by textures is unaffected."; }
        }

        public override string Author
        {
            get { return "chekwob"; }
        }

        public override void Run(ref byte r,ref byte g,ref byte b)
        {
            byte avg = (byte)((r + g + b) / 3);
            r = avg;
            g = avg;
            b = avg;
        }
    }
}
