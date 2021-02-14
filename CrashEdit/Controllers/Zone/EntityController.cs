using Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class EntityController : Controller
    {
        public EntityController(ZoneEntryController zoneentrycontroller,Entity entity)
        {
            ZoneEntryController = zoneentrycontroller;
            Entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            AddMenu("Delete Entity",Menu_Delete);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            if (Entity.Name != null && Entity.ID != null)
            {
                Node.Text = $"{Entity.Name} [ID {Entity.ID}]";
            }
            else if (Entity.ID != null)
            {
                Node.Text = $"{Crash.UI.Properties.Resources.EntityController_Text} [ID {Entity.ID}]";
            }
            else
            {
                Node.Text = Crash.UI.Properties.Resources.EntityController_Text;
            }
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
        }

        protected override Control CreateEditor()
        {
            return new EntityBox(this);
        }

        public ZoneEntryController ZoneEntryController { get; }
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
            foreach (ZoneEntry zone in ZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<ZoneEntry>())
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
            ZoneEntryController.AddNode(new EntityController(ZoneEntryController,newentity));
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
                foreach (ZoneEntry zone in ZoneEntryController.EntryChunkController.NSFController.NSF.GetEntries<ZoneEntry>())
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
            Dispose();
        }
    }
}
