namespace Crash
{
    public struct SceneryColor
    {
        public SceneryColor(byte red,byte green,byte blue) : this(red,green,blue,0)
        {
        }

        public SceneryColor(byte red,byte green,byte blue,byte extra)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Extra = extra;
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Extra { get; }
    }
}
