using System.Collections.Generic;

namespace Crash
{
    public sealed class OldZoneEntry : Entry
    {
        private List<OldCamera> cameras;
        private List<OldEntity> entities;

        public OldZoneEntry(byte[] unknown1,byte[] unknown2,IEnumerable<OldCamera> cameras,IEnumerable<OldEntity> entities,int camcount,int eid,int size)
            : base(eid,size)
        {
            Unknown1 = unknown1;
            Layout = unknown2;
            this.cameras = new List<OldCamera>(cameras);
            this.entities = new List<OldEntity>(entities);
            CamCount = camcount;
        }

        public override int Type => 7;
        public byte[] Unknown1 { get; }
        public byte[] Layout { get; }
        public IList<OldCamera> Cameras => cameras;
        public IList<OldEntity> Entities => entities;
        public int CamCount { get; set; }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[2 + entities.Count + CamCount][];
            items[0] = Unknown1;
            items[1] = Layout;
            for (int i = 0; i < CamCount; i++)
            {
                items[2 + i] = cameras[i].Save();
            }
            for (int i = 0; i < entities.Count; i++)
            {
                items[2 + CamCount + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
