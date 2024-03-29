using CrashEdit.Crash;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(ProtoZoneEntry))]
    public sealed class ProtoZoneEntryController : EntryController
    {
        public ProtoZoneEntryController(ProtoZoneEntry zoneentry, SubcontrollerGroup parentGroup)
            : base(zoneentry, parentGroup)
        {
            ProtoZoneEntry = zoneentry;
            AddMenu("Export as Crash 1 ZDAT", Menu_ExportAsC1);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new ProtoZoneEntryViewer(GetNSF(), Entry.EID);
        }

        public ProtoZoneEntry ProtoZoneEntry { get; }

        private void Menu_ExportAsC1()
        {
            byte[] header = new byte[0x378];
            // convert header
            Array.Copy(ProtoZoneEntry.Header, 0, header, 0, 0x228);
            Array.Copy(ProtoZoneEntry.Header, 0x228, header, 0x234, 0xB0);
            Array.Copy(ProtoZoneEntry.Header, 0x2EC, header, 0x318, 0x60);
            BitConv.ToInt32(header, 0x304, Entry.NullEID);
            // convert layout
            short xmax = BitConv.FromInt16(ProtoZoneEntry.Layout, 0x1E);
            short ymax = BitConv.FromInt16(ProtoZoneEntry.Layout, 0x20);
            short zmax = BitConv.FromInt16(ProtoZoneEntry.Layout, 0x22);
            if (ymax == 0) ymax = xmax;
            if (zmax == 0) zmax = ymax;
            byte[] layout = new byte[ProtoZoneEntry.Layout.Length];
            ProtoZoneEntry.Layout.CopyTo(layout, 0);
            BitConv.ToInt16(layout, 0x1E, xmax);
            BitConv.ToInt16(layout, 0x20, ymax);
            BitConv.ToInt16(layout, 0x22, zmax);
            // convert entities - cameras have the same format so no conversion is necessary
            List<OldEntity> entities = new List<OldEntity>();
            foreach (ProtoEntity protoentity in ProtoZoneEntry.Entities)
            {
                List<EntityPosition> pos = new List<EntityPosition>();
                short x = (short)(protoentity.StartX / 4);
                short y = (short)(protoentity.StartY / 4);
                short z = (short)(protoentity.StartZ / 4);
                pos.Add(new EntityPosition(x, y, z));
                foreach (ProtoEntityPosition delta in protoentity.Deltas)
                {
                    x += (short)(delta.X * 2);
                    y += (short)(delta.Y * 2);
                    z += (short)(delta.Z * 2);
                    pos.Add(new EntityPosition(x, y, z));
                }
                entities.Add(new OldEntity(protoentity.Flags, protoentity.Spawn, protoentity.Unk, (short)(protoentity.ID + 5), protoentity.VecX, protoentity.VecY, protoentity.VecZ, protoentity.Type, protoentity.Subtype, pos, protoentity.Nullfield1));
            }
            OldZoneEntry newzone = new OldZoneEntry(header, layout, ProtoZoneEntry.Cameras, entities, ProtoZoneEntry.EID);
            FileUtil.SaveFile(newzone.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
