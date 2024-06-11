namespace CrashEdit.Crash
{
    public abstract class GOOLFrameGroup<T> : GOOLFrameGroupWithChunk where T : class
    {
        public short FrameCount => (short)Frames.Count;
        public List<T> Frames { get; set; }

        public GOOLFrameGroup(List<T> frames, int eid) : base(eid)
        {
            Frames = frames;
        }
    }
}
