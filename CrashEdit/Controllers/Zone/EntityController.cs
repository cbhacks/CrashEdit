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
                Node.Text = string.Format("{0} - ID {1}",Entity.Name,Entity.ID);
            }
            else if (Entity.ID != null)
            {
                Node.Text = string.Format("{1} ID {0}",Entity.ID,Crash.UI.Properties.Resources.EntityController_Text);
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
            foreach (Chunk chunk in ZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk entrychunk)
                {
                    foreach (Entry entry in entrychunk.Entries)
                    {
                        if (entry is ZoneEntry zone)
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
            if (Entity.ID.HasValue)
            {
                foreach (Chunk chunk in ZoneEntryController.EntryChunkController.NSFController.NSF.Chunks)
                {
                    if (chunk is EntryChunk entrychunk)
                    {
                        foreach (Entry entry in entrychunk.Entries)
                        {
                            if (entry is ZoneEntry zone)
                            {
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
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (ZoneEntry.Entities.IndexOf(Entity) < ZoneEntry.CameraCount)
                --ZoneEntry.CameraCount;
            else
                --ZoneEntry.EntityCount;
            ZoneEntry.Entities.Remove(Entity);
            Dispose();
        }
    }
}
