using Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class EntityController : Controller
    {
        private EntityEntryController entityentrycontroller;
        private Entity entity;

        public EntityController(EntityEntryController entityentrycontroller,Entity entity)
        {
            this.entityentrycontroller = entityentrycontroller;
            this.entity = entity;
            if (entity.Name != null)
            {
                Node.Text = string.Format("Entity ({0})",entity.Name);
            }
            else
            {
                Node.Text = "Entity";
            }
            Node.ImageKey = "entity";
            Node.SelectedImageKey = "entity";
            AddMenu("Duplicate Entity",Menu_Duplicate);
        }

        protected override Control CreateEditor()
        {
            return new EntityBox(entity);
        }

        public EntityEntryController EntityEntryController
        {
            get { return entityentrycontroller; }
        }

        public Entity Entity
        {
            get { return entity; }
        }

        private void Menu_Duplicate()
        {
            if (!entity.ID.HasValue)
            {
                throw new GUIException("Only entities with ID's can be duplicated.");
            }
            int maxid = 1;
            List<EntityPropertyRow<int>> drawlists = new List<EntityPropertyRow<int>>();
            foreach (Chunk chunk in entityentrycontroller.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is EntityEntry)
                        {
                            foreach (Entity otherentity in ((EntityEntry)entry).Entities)
                            {
                                if (otherentity.ID.HasValue)
                                {
                                    if (otherentity.ID.Value > maxid)
                                    {
                                        maxid = otherentity.ID.Value;
                                    }
                                    if (otherentity.AlternateID.HasValue && otherentity.AlternateID.Value > maxid)
                                    {
                                        maxid = otherentity.AlternateID.Value;
                                    }
                                }
                                if (otherentity.DrawListA != null)
                                {
                                    drawlists.AddRange(otherentity.DrawListA.Rows);
                                }
                                if (otherentity.DrawListB != null)
                                {
                                    drawlists.AddRange(otherentity.DrawListB.Rows);
                                }
                            }
                        }
                    }
                }
            }
            maxid++;
            int newindex = entityentrycontroller.EntityEntry.Entities.Count;
            newindex -= BitConv.FromInt32(entityentrycontroller.EntityEntry.Unknown1,0x188);
            int entitycount = BitConv.FromInt32(entityentrycontroller.EntityEntry.Unknown1,0x18C);
            BitConv.ToInt32(entityentrycontroller.EntityEntry.Unknown1,0x18C,entitycount + 1);
            Entity newentity = Entity.Load(entity.Save());
            newentity.ID = maxid;
            newentity.AlternateID = null;
            entityentrycontroller.EntityEntry.Entities.Add(newentity);
            entityentrycontroller.AddNode(new EntityController(entityentrycontroller,newentity));
            foreach (EntityPropertyRow<int> drawlist in drawlists)
            {
                foreach (int value in drawlist.Values)
                {
                    if ((value & 0xFFFF00) >> 8 == entity.ID.Value)
                    {
                        unchecked
                        {
                            drawlist.Values.Add((value & 0xFF) | (maxid << 8) | (newindex << 24));
                        }
                        break;
                    }
                }
                if (drawlist.Values.Contains(entity.ID.Value))
                {
                    drawlist.Values.Add(maxid);
                }
            }
        }
    }
}
