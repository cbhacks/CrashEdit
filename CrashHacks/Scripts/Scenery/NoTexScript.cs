using Crash;

namespace CrashHacks.Scripts.Scenery
{
    public sealed class NoTexScript : Script
    {
        public override string Name => "Make all scenery polygons untextured";
        public override string Author => "mdude";
        public override string Category => "scenery";

        public override SupportLevel CheckCompatibility(GameVersion gameversion)
        {
            switch (gameversion)
            {
                case GameVersion.Crash1:
                case GameVersion.Crash1BetaMAR08:
                case GameVersion.Crash1BetaMAY11:
                case GameVersion.Crash3:
                    return SupportLevel.Unsupported;
                case GameVersion.Crash2:
                    return SupportLevel.Supported;
                default:
                    return SupportLevel.Untested;
            }
        }

        public override void Run(object value,GameVersion gameversion)
        {
            if (value is SceneryEntry e)
            {
                for (int i = 0;i < e.Triangles.Count;i++)
                {
                    SceneryTriangle tri = e.Triangles[i];
                    int vertexa = tri.VertexA;
                    int vertexb = tri.VertexB;
                    int vertexc = tri.VertexC;
                    short texture = 0;
                    bool animated = false;
                    e.Triangles[i] = new SceneryTriangle(vertexa,vertexb,vertexc,texture,animated);
                }
                for (int i = 0;i < e.Quads.Count;i++)
                {
                    SceneryQuad quad = e.Quads[i];
                    int vertexa = quad.VertexA;
                    int vertexb = quad.VertexB;
                    int vertexc = quad.VertexC;
                    int vertexd = quad.VertexD;
                    short texture = 0;
                    byte unknown = 0;
                    bool animated = false;
                    e.Quads[i] = new SceneryQuad(vertexa,vertexb,vertexc,vertexd,texture,unknown,animated);
                }
            }
        }
    }
}
