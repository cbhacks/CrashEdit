
using System.Collections.Generic;

namespace Crash
{
    public sealed class NewZoneEntry : Entry
    {
        private List<Entity> entities;

        public NewZoneEntry(byte[] header, byte[] layout,IEnumerable<Entity> entities,int eid,int size) : base(eid,size)
        {
            Header = header;
            Layout = layout;
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
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
