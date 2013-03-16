using System.Collections.Generic;

namespace Crash.Game
{
    public sealed class EntityEntry : Entry
    {
        private byte[] unknown1;
        private byte[] unknown2;
        private List<Entity> entities;

        public EntityEntry(byte[] unknown1,byte[] unknown2,IEnumerable<Entity> entities,int eid) : base(eid)
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

        public override byte[] Save()
        {
            byte[][] items = new byte [2 + entities.Count][];
            items[0] = unknown1;
            items[1] = unknown2;
            for (int i = 0;i < entities.Count;i++)
            {
                items[2 + i] = entities[i].Save();
            }
            return Save(items);
        }
    }
}
