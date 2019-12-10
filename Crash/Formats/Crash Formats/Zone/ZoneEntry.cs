using System.Collections.Generic;

namespace Crash
{
    public sealed class ZoneEntry : Entry
    {
        private List<Entity> entities;

        public ZoneEntry(byte[] unknown1,byte[] unknown2,IEnumerable<Entity> entities,int eid) : base(eid)
        {
            Header = unknown1;
            Layout = unknown2;
            this.entities = new List<Entity>(entities);
        }

        public override int Type => 7;
        public byte[] Header { get; }
        public byte[] Layout { get; }
        public IList<Entity> Entities => entities;

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2 + entities.Count][];
            items[0] = Header;
            items[1] = Layout;
            for (int i = 0;i < entities.Count;i++)
            {
                items[2 + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type);
        }
    }
}
