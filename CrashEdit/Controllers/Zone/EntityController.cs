using CrashEdit.Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(Entity))]
    public sealed class EntityController : LegacyController
    {
        public EntityController(Entity entity, SubcontrollerGroup parentGroup) : base(parentGroup, entity)
        {
            Entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new EntityBox(this);
        }

        public ZoneEntryController ZoneEntryController => Modern.Parent.Legacy as ZoneEntryController;
        public ZoneEntry ZoneEntry => ZoneEntryController.ZoneEntry;
        public NewZoneEntryController NewZoneEntryController => Modern.Parent.Legacy as NewZoneEntryController;
        public NewZoneEntry NewZoneEntry => NewZoneEntryController.NewZoneEntry;

        public Entity Entity { get; }

        private void Menu_Duplicate()
        {
            if (ZoneEntryController != null)
                Menu_DuplicateC2();
            else if (NewZoneEntryController != null)
                Menu_DuplicateC3();
        }

        private void Menu_DuplicateC2()
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

        private void Menu_DuplicateC3()
        {
            if (!Entity.ID.HasValue)
            {
                throw new GUIException("Only entities with ID's can be duplicated.");
            }
            int maxid = 1;
            List<EntityPropertyRow<int>> drawlists = new List<EntityPropertyRow<int>>();
            foreach (NewZoneEntry zone in GetEntries<NewZoneEntry>())
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
            int newindex = NewZoneEntry.Entities.Count - NewZoneEntry.CameraCount;
            ++NewZoneEntry.EntityCount;
            Entity newentity = Entity.Load(Entity.Save());
            newentity.ID = maxid;
            newentity.AlternateID = null;
            NewZoneEntry.Entities.Add(newentity);
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

        private void Menu_Delete()
        {
            if (ZoneEntryController != null)
                Menu_DeleteC2();
            else if (NewZoneEntryController != null)
                Menu_DeleteC3();
        }

        private void Menu_DeleteC2()
        {
            int index = -1;
            if (ZoneEntry.Entities.IndexOf(Entity) < ZoneEntry.CameraCount)
            {
                --ZoneEntry.CameraCount;
            }
            else
            {
                index = ZoneEntry.Entities.IndexOf(Entity) - ZoneEntry.CameraCount;
                --ZoneEntry.EntityCount;
            }
            if (Entity.ID.HasValue)
            {
                foreach (ZoneEntry zone in GetEntries<ZoneEntry>())
                {
                    int zoneindex = -1;
                    for (int z = 0, s = BitConv.FromInt32(zone.Header,0x190); z < s; ++z)
                    {
                        if (BitConv.FromInt32(zone.Header,0x194+z*4) == ZoneEntry.EID)
                        {
                            zoneindex = z;
                            break;
                        }
                    }
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.DrawListA != null)
                        {
                            foreach (EntityPropertyRow<int> row in otherentity.DrawListA.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == Entity.ID.Value)
                                        row.Values.RemoveAt(i);
                                    else if ((row.Values[i] & 0xFF) == zoneindex && ((row.Values[i] & 0xFF000000) >> 24) > index)
                                    {
                                        int newindex = (int)(row.Values[i] & 0xFF000000) >> 24;
                                        row.Values[i] &= 0xFFFFFF;
                                        row.Values[i] |= --newindex << 24;
                                    }
                                }
                            }
                        }
                        if (otherentity.DrawListB != null)
                        {
                            foreach (EntityPropertyRow<int> row in otherentity.DrawListB.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == Entity.ID.Value)
                                        row.Values.RemoveAt(i);
                                    else if ((row.Values[i] & 0xFF) == zoneindex && ((row.Values[i] & 0xFF000000) >> 24) > index)
                                    {
                                        int newindex = (int)(row.Values[i] & 0xFF000000) >> 24;
                                        row.Values[i] &= 0xFFFFFF;
                                        row.Values[i] |= --newindex << 24;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ZoneEntry.Entities.Remove(Entity);
        }

        private void Menu_DeleteC3()
        {
            int index = -1;
            if (NewZoneEntry.Entities.IndexOf(Entity) < NewZoneEntry.CameraCount)
            {
                --NewZoneEntry.CameraCount;
            }
            else
            {
                index = NewZoneEntry.Entities.IndexOf(Entity) - NewZoneEntry.CameraCount;
                --NewZoneEntry.EntityCount;
            }
            if (Entity.ID.HasValue)
            {
                foreach (NewZoneEntry zone in GetEntries<NewZoneEntry>())
                {
                    int zoneindex = -1;
                    for (int z = 0, s = BitConv.FromInt32(zone.Header, 0x190); z < s; ++z)
                    {
                        if (BitConv.FromInt32(zone.Header, 0x194 + z * 4) == NewZoneEntry.EID)
                        {
                            zoneindex = z;
                            break;
                        }
                    }
                    foreach (Entity otherentity in zone.Entities)
                    {
                        if (otherentity.DrawListA != null)
                        {
                            foreach (EntityPropertyRow<int> row in otherentity.DrawListA.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == Entity.ID.Value)
                                        row.Values.RemoveAt(i);
                                    else if ((row.Values[i] & 0xFF) == zoneindex && ((row.Values[i] & 0xFF000000) >> 24) > index)
                                    {
                                        int newindex = (int)(row.Values[i] & 0xFF000000) >> 24;
                                        row.Values[i] &= 0xFFFFFF;
                                        row.Values[i] |= --newindex << 24;
                                    }
                                }
                            }
                        }
                        if (otherentity.DrawListB != null)
                        {
                            foreach (EntityPropertyRow<int> row in otherentity.DrawListB.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == Entity.ID.Value)
                                        row.Values.RemoveAt(i);
                                    else if ((row.Values[i] & 0xFF) == zoneindex && ((row.Values[i] & 0xFF000000) >> 24) > index)
                                    {
                                        int newindex = (int)(row.Values[i] & 0xFF000000) >> 24;
                                        row.Values[i] &= 0xFFFFFF;
                                        row.Values[i] |= --newindex << 24;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            NewZoneEntry.Entities.Remove(Entity);
        }
    }
}
