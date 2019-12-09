namespace Crash
{
    public abstract class GOOLAnimation
    {
        public GOOLAnimation(byte type,byte u1,byte count,byte u2)
        {
            Type = type;
            U1 = u1;
            Count = count;
            U2 = u2;
        }

        public byte Type { get; }
        public byte U1 { get; }
        public byte Count { get; }
        public byte U2 { get; }
        
        public abstract byte[] Save();
    }
}
