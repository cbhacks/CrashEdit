
using System.Collections.Generic;

namespace Crash
{
    public sealed class NewZoneEntry : Entry
    {
        private byte[] unknown1;
        private byte[] unknown2;
        private List<Entity> entities;

        public NewZoneEntry(byte[] unknown1,byte[] unknown2,IEnumerable<Entity> entities,int eid,int size) : base(eid,size)
        {
            this.unknown1 = unknown1;
            this.unknown2 = unknown2;
            this.entities = new List<Entity>(entities);
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

        public IList<Entity> Entities
        {
            get { return entities; }
        }

        public override UnprocessedEntry Unprocess()
        {
            byte[][] items = new byte [2 + entities.Count][];
            items[0] = unknown1;
            items[1] = unknown2;
            for (int i = 0;i < entities.Count;i++)
            {
                items[2 + i] = entities[i].Save();
            }
            return new UnprocessedEntry(items,EID,Type,Size);
        }
    }
}
