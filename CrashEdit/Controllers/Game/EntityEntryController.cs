using Crash;
using Crash.Game;

namespace CrashEdit
{
    public sealed class EntityEntryController : EntryController
    {
        private EntityEntry entityentry;

        public EntityEntryController(EntryChunkController entrychunkcontroller,EntityEntry entityentry) : base(entrychunkcontroller,entityentry)
        {
            this.entityentry = entityentry;
            Node.Text = "Entity Entry";
            Node.ImageKey = "entityentry";
            Node.SelectedImageKey = "entityentry";
            foreach (Entity entity in entityentry.Entities)
            {
                AddNode(new EntityController(this,entity));
            }
        }

        public EntityEntry EntityEntry
        {
            get { return entityentry; }
        }
    }
}
