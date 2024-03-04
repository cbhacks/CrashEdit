using CrashEdit.Crash;

namespace CrashEdit
{
    public sealed class DeleteEntityVerb : DeleteVerb
    {
        public override string Text => "Delete Entity";

        public override string ImageKey => "Erase";

        public override bool ApplicableForSubject(Controller subj)
        {
            ArgumentNullException.ThrowIfNull(subj);

            return (subj.ParentGroup?.CanRemove ?? false) && (subj.Resource.GetType() == typeof(Entity));
        }

        public override void Execute(IUserInterface ui)
        {
            ArgumentNullException.ThrowIfNull(ui);
            if (Subject == null)
                throw new InvalidOperationException();
            if (Subject.ParentGroup == null)
                throw new InvalidOperationException();

            var entity = Subject.Resource as Entity;
            var entityzone = Subject.Parent?.Resource as ZoneEntry;
            var level = Subject.Root!.Resource as LevelWorkspace;

            if (entity == null || entityzone == null || level == null)
                throw new InvalidOperationException();

            int index = -1;
            if (entityzone.Entities.IndexOf(entity) < entityzone.CameraCount)
            {
                --entityzone.CameraCount;
            }
            else
            {
                index = entityzone.Entities.IndexOf(entity) - entityzone.CameraCount;
                --entityzone.EntityCount;
            }
            if (entity.ID.HasValue)
            {
                foreach (var zone in level.NSF!.GetEntries<ZoneEntry>())
                {
                    int zoneindex = -1;
                    for (int z = 0, s = zone.ZoneCount; z < s; ++z)
                    {
                        if (zone.GetLinkedZone(z) == entityzone.EID)
                        {
                            zoneindex = z;
                            break;
                        }
                    }
                    foreach (var otherentity in zone.Entities)
                    {
                        if (otherentity.DrawListA != null)
                        {
                            foreach (var row in otherentity.DrawListA.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == entity.ID.Value)
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
                            foreach (var row in otherentity.DrawListB.Rows)
                            {
                                for (int i = row.Values.Count - 1; i >= 0; --i)
                                {
                                    if ((row.Values[i] & 0xFFFF00) >> 8 == entity.ID.Value)
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

            Subject.ParentGroup.Remove(Subject);
        }
    }
}
