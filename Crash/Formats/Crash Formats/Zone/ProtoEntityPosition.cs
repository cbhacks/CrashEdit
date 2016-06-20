namespace Crash
{
    public struct ProtoEntityPosition
    {
        private int position;

        public ProtoEntityPosition(int position)
        {
            this.position = position;
        }

        public int Position
        {
            get { return position; }
        }
    }
}
