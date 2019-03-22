using System;
using Crash;

namespace CrashHacks.Scripts.Scenery.Color
{
    public sealed class RandomScript : BaseScript
    {
        private Random random;

        public RandomScript()
        {
            this.random = new Random();
        }

        public override string Name
        {
            get { return "Rainbow scenery colors"; }
        }

        public override string Description
        {
            get { return "Fills the color channels for scenery vertex coloring with random values."; }
        }

        public override string Author
        {
            get { return "chekwob"; }
        }

        public override void Run(ref byte r,ref byte g,ref byte b)
        {
            r = (byte)random.Next(0,256);
            g = (byte)random.Next(0,256);
            b = (byte)random.Next(0,256);
        }
    }
}
