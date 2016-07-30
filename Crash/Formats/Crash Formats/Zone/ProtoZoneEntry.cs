using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoZoneEntry : Entry
    {
        private byte[] unknown1;
        private byte[] unknown2;
        private List<OldCamera> cameras;
        private List<ProtoEntity> entities;
        private int camcount;

        public ProtoZoneEntry(byte[] unknown1,byte[] unknown2,IEnumerable<OldCamera> cameras,IEnumerable<ProtoEntity> entities,int camcount,int eid,int size)
            : base(eid,size)
        {
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
            this.cameras = new List<OldCamera>(cameras);
            this.entities = new List<ProtoEntity>(entities);
            this.camcount = camcount;
        }

        public override int Type
        {
            get { return 7; }
        }

        public byte[] Unknown1
        {
            get { return unknown1; }
        }

        public byte[] Unknown2
        {
            get { return unknown2; }
        }

        public IList<OldCamera> Cameras
        {
            get { return cameras; }
        }

        public IList<ProtoEntity> Entities
        {
            get { return entities; }
        }

        public int CamCount
        {
            get { return camcount; }
            set { camcount = value; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[2 + entities.Count + camcount][];
            items[0] = unknown1;
            items[1] = unknown2;
            for (int i = 0; i < camcount; i++)
            {
                items[2 + i] = cameras[i].Save();
            }
            for (int i = 0; i < entities.Count; i++)
            {
                items[2 + camcount + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
