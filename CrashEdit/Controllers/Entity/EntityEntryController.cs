using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class EntityEntryController : EntryController
    {
        private EntityEntry entityentry;

        public EntityEntryController(EntryChunkController entrychunkcontroller,EntityEntry entityentry) : base(entrychunkcontroller,entityentry)
        {
            this.entityentry = entityentry;
            Node.Text = string.Format("Entity Entry ({0})",entityentry.EIDString);
            Node.ImageKey = "entityentry";
            Node.SelectedImageKey = "entityentry";
            AddNode(new ItemController(null,entityentry.Unknown1));
            AddNode(new ItemController(null,entityentry.Unknown2));
            foreach (Entity entity in entityentry.Entities)
            {
                AddNode(new EntityController(this,entity));
            }
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(entityentry.Unknown1,0);
            SceneryEntry[] linkedsceneryentries = new SceneryEntry [linkedsceneryentrycount];
            for (int i = 0;i < linkedsceneryentrycount;i++)
            {
                linkedsceneryentries[i] = FindEID<SceneryEntry>(BitConv.FromInt32(entityentry.Unknown1,4 + i * 48));
            }
            int linkedentityentrycount = BitConv.FromInt32(entityentry.Unknown1,400);
            EntityEntry[] linkedentityentries = new EntityEntry [linkedentityentrycount];
            for (int i = 0;i < linkedentityentrycount;i++)
            {
                linkedentityentries[i] = FindEID<EntityEntry>(BitConv.FromInt32(entityentry.Unknown1,404 + i * 4));
            }
            return new UndockableControl(new EntityEntryViewer(entityentry,linkedsceneryentries,linkedentityentries));
        }

        public EntityEntry EntityEntry
        {
            get { return entityentry; }
        }
    }
}
