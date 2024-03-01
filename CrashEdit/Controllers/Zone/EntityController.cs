using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(Entity))]
    public sealed class EntityController : LegacyController
    {
        public EntityController(Entity entity, SubcontrollerGroup parentGroup) : base(parentGroup, entity)
        {
            Entity = entity;
            AddMenu("Duplicate Entity", Menu_Duplicate);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new EntityBox(this);
        }

        public ZoneEntryController ZoneEntryController => Modern.Parent.Legacy as ZoneEntryController;
        public ZoneEntry ZoneEntry => ZoneEntryController.ZoneEntry;

        public Entity Entity { get; }

        private void Menu_Duplicate()
        {
            if (!Entity.ID.HasValue)
            {
                throw new GUIException("Only entities with ID's can be duplicated.");
            }
            int maxid = 1;
            List<EntityPropertyRow<int>> drawlists = new List<EntityPropertyRow<int>>();
            foreach (ZoneEntry zone in GetEntries<ZoneEntry>())
            {
                foreach (Entity otherentity in zone.Entities)
                {
                    if (otherentity.ID.HasValue)
                    {
                        if (otherentity.ID.Value > maxid)
                        {
                            maxid = otherentity.ID.Value;
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
            maxid++;
            int newindex = ZoneEntry.Entities.Count - ZoneEntry.CameraCount;
            ++ZoneEntry.EntityCount;
            Entity newentity = Entity.Load(Entity.Save());
            newentity.ID = maxid;
            newentity.AlternateID = null;
            ZoneEntry.Entities.Add(newentity);
            foreach (EntityPropertyRow<int> drawlist in drawlists)
            {
                foreach (int value in drawlist.Values)
                {
                    if ((value & 0xFFFF00) >> 8 == Entity.ID.Value)
                    {
                        unchecked
                        {
                            drawlist.Values.Add((value & 0xFF) | (maxid << 8) | (newindex << 24));
                        }
                        break;
                    }
                }
                if (drawlist.Values.Contains(Entity.ID.Value))
                {
                    drawlist.Values.Add(maxid);
                }
            }
        }
    }
}
