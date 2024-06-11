namespace CrashEdit.Crash
{
    public abstract class GOOLChunkFrameGroup : GOOLProtoFrameGroup
    {
        public abstract short Type();

        public int EID { get; set; }

        public GOOLChunkFrameGroup(int eid)
        {
            EID = eid;
        }
    }
}
