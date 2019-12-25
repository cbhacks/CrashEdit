using System.Collections.Generic;

namespace Crash
{
    public sealed class ProtoZoneEntry : Entry
    {
        private List<OldCamera> cameras;
        private List<ProtoEntity> entities;

        public ProtoZoneEntry(byte[] header,byte[] layout,IEnumerable<OldCamera> cameras,IEnumerable<ProtoEntity> entities,int eid)
            : base(eid)
        {
            Header = header;
            Layout = layout;
            this.cameras = new List<OldCamera>(cameras);
            this.entities = new List<ProtoEntity>(entities);
        }

        public override int Type => 7;
        public byte[] Header { get; }
        public byte[] Layout { get; }
        public IList<OldCamera> Cameras => cameras;
        public IList<ProtoEntity> Entities => entities;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte[2 + entities.Count + cameras.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0; i < cameras.Count; i++)
            {
                items[2 + i] = cameras[i].Save();
            }
            for (int i = 0; i < entities.Count; i++)
            {
                items[2 + cameras.Count + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
