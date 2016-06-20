namespace Crash
{
    public struct SceneryColor
    {
        private byte red;
        private byte blue;
        private byte green;
        private byte extra;

        public SceneryColor(byte red,byte green,byte blue) : this(red,green,blue,0)
        {
        }

        public SceneryColor(byte red,byte green,byte blue,byte extra)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
            this.extra = extra;
        }

        public byte Red
        {
            get { return red; }
        }

        public byte Green
        {
            get { return green; }
        }

        public byte Blue
        {
            get { return blue; }
        }

        public byte Extra
        {
            get { return extra; }
        }
    }
}
