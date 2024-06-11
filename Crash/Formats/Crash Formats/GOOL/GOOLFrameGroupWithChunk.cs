namespace CrashEdit.Crash
{
    public abstract class GOOLFrameGroupWithChunk : GOOLFrameGroupBase
    {
        public abstract short Type();

        public int EID { get; set; }

        public GOOLFrameGroupWithChunk(int eid)
        {
            EID = eid;
        }
    }
}
