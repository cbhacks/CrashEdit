using Crash;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit
{
    public sealed class NewEntityController : Controller
    {
        private NewZoneEntryController zoneentrycontroller;
        private Entity entity;

        public NewEntityController(NewZoneEntryController zoneentrycontroller,Entity entity)
        {
            this.zoneentrycontroller = zoneentrycontroller;
            this.entity = entity;
            AddMenu("Duplicate Entity",Menu_Duplicate);
            Node.ImageKey = "arrow";
            Node.SelectedImageKey = "arrow";
            InvalidateNode();
        }

        public override void InvalidateNode()
        {
            if (entity.Name != null && entity.ID != null)
            {
                Node.Text = string.Format("{0} - ID {1}", entity.Name, entity.ID);
            }
            else if (entity.ID != null)
            {
                Node.Text = string.Format("Entity ID {0}", entity.ID);
            }
            else
            {
                Node.Text = "Entity";
            }
        }

        protected override Control CreateEditor()
        {
            return new NewEntityBox(this);
        }

        public NewZoneEntryController NewZoneEntryController
        {
            get { return zoneentrycontroller; }
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
            foreach (Chunk chunk in zoneentrycontroller.EntryChunkController.NSFController.NSF.Chunks)
            {
                if (chunk is EntryChunk)
                {
                    foreach (Entry entry in ((EntryChunk)chunk).Entries)
                    {
                        if (entry is NewZoneEntry)
                        {
                            foreach (Entity otherentity in ((NewZoneEntry)entry).Entities)
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
            int newindex = zoneentrycontroller.NewZoneEntry.Entities.Count;
            newindex -= BitConv.FromInt32(zoneentrycontroller.NewZoneEntry.Unknown1, 0x188);
            int entitycount = BitConv.FromInt32(zoneentrycontroller.NewZoneEntry.Unknown1, 0x18C);
            BitConv.ToInt32(zoneentrycontroller.NewZoneEntry.Unknown1, 0x18C, entitycount + 1);
            Entity newentity = Entity.Load(entity.Save());
            newentity.ID = maxid;
            newentity.AlternateID = null;
            zoneentrycontroller.NewZoneEntry.Entities.Add(newentity);
            zoneentrycontroller.AddNode(new NewEntityController(zoneentrycontroller, newentity));
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
