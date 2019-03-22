using Crash;

namespace CrashHacks.Scripts.Scenery
{
    public abstract class FXScript : Script
    {
        private FXScript()
        {
        }

        public override string Name
        {
            get { return string.Format("Set scenery FX to mode {0}",Mode); }
        }

        public override string Description
        {
            get { return "Sets the FX mode for each scenery vertex to the specified value. Each vertex has an FX mode in the range 0...3 (0...1 for CB1) which is used as an input to the vertex shader. The behavior of the different modes depends on which vertex shader is in use (which is determined by the active camera). Effects include lightning, water waving, water electrification (only visually), strobe effects, etc."; }
        }

        public override string Author
        {
            get { return "chekwob"; }
        }

        public override string Category
        {
            get { return "scenery"; }
        }

        public abstract int Mode
        {
            get;
        }

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            switch (gameversion)
            {
                case GameVersion.Crash1:
                case GameVersion.Crash1BetaMAR08:
                case GameVersion.Crash1BetaMAY11:
                    return SupportLevel.Unsupported;
                case GameVersion.Crash2:
                    return SupportLevel.Supported;
                default:
                    return SupportLevel.Untested;
            }
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is SceneryEntry)
            {
                SceneryEntry entry = (SceneryEntry)value;
                for (int i = 0;i < entry.Vertices.Count;i++)
                {
                    SceneryVertex vertex = entry.Vertices[i];
                    int x = vertex.X;
                    int y = vertex.Y;
                    int z = vertex.Z;
                    int unknownx = vertex.UnknownX;
                    int unknowny = vertex.UnknownY;
                    int unknownz = vertex.UnknownZ;
                    int color = vertex.Color;
                    unknowny &= 3;
                    unknowny |= Mode << 2;
                    entry.Vertices[i] = new SceneryVertex(x,y,z,unknownx,unknowny,unknownz);
                }
            }
        }

        public sealed class Mode0 : FXScript
        {
            public override int Mode
            {
                get { return 0; }
            }
        }

        public sealed class Mode1 : FXScript
        {
            public override int Mode
            {
                get { return 1; }
            }
        }

        public sealed class Mode2 : FXScript
        {
            public override int Mode
            {
                get { return 2; }
            }
        }

        public sealed class Mode3 : FXScript
        {
            public override int Mode
            {
                get { return 3; }
            }
        }
    }
}
