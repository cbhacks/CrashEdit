using Crash;

namespace CrashHacks.Scripts.Scenery.Color
{
    public sealed class NoBlueScript : BaseScript
    {
        public override string Name
        {
            get { return "Remove blue color from scenery"; }
        }

        public override string Description
        {
            get { return "Zeroes out the blue color channel used in scenery vertex coloring. Color introduced by textures is unaffected."; }
        }

        public override string Author
        {
            get { return "chekwob"; }
        }

        public override void Run(ref byte r,ref byte g,ref byte b)
        {
            b = 0;
        }
    }
}
