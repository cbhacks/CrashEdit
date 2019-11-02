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
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            if (Entity.Name != null && Entity.ID != null)
            {
                Node.Text = string.Format("{0} - ID {1}",Entity.Name,Entity.ID);
            }
            else if (Entity.ID != null)
            {
                Node.Text = string.Format("Entity ID {0}",Entity.ID);
            }
            else if (Node.Text != "Entity")
            {
                Node.Text = "Entity";
            }
        }

        protected override Control CreateEditor()
        {
            return new EntityBox(this);
        }

        public ZoneEntryController ZoneEntryController { get; }

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
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is ZoneEntry)
                        {
                            foreach (Entity otherentity in ((ZoneEntry)entry).Entities)
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
            int newindex = ZoneEntryController.ZoneEntry.Entities.Count;
            newindex -= BitConv.FromInt32(ZoneEntryController.ZoneEntry.Unknown1,0x188);
            int entitycount = BitConv.FromInt32(ZoneEntryController.ZoneEntry.Unknown1,0x18C);
            BitConv.ToInt32(ZoneEntryController.ZoneEntry.Unknown1,0x18C,entitycount + 1);
            Entity newentity = Entity.Load(Entity.Save());
            newentity.ID = maxid;
            newentity.AlternateID = null;
            ZoneEntryController.ZoneEntry.Entities.Add(newentity);
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
    }
}
