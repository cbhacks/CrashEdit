using Crash;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class ProtoZoneEntryController : EntryController
    {
        public ProtoZoneEntryController(EntryChunkController entrychunkcontroller,ProtoZoneEntry zoneentry)
            : base(entrychunkcontroller,zoneentry)
        {
            ProtoZoneEntry = zoneentry;
            AddNode(new ItemController(null,zoneentry.Header));
            AddNode(new ItemController(null,zoneentry.Layout));
            foreach (OldCamera camera in zoneentry.Cameras)
            {
                AddNode(new OldCameraController(this,camera));
            }
            foreach (ProtoEntity entity in zoneentry.Entities)
            {
                AddNode(new ProtoEntityController(this,entity));
            }
            AddMenu("Export as Crash 1 ZDAT", Menu_ExportAsC1);
            InvalidateNode();
            InvalidateNodeImage();
        }

        public override void InvalidateNode()
        {
            Node.Text = string.Format(Crash.UI.Properties.Resources.ProtoZoneEntryController_Text,ProtoZoneEntry.EName);
        }

        public override void InvalidateNodeImage()
        {
            Node.ImageKey = "violetb";
            Node.SelectedImageKey = "violetb";
        }

        protected override Control CreateEditor()
        {
            int linkedsceneryentrycount = BitConv.FromInt32(ProtoZoneEntry.Header,0);
            ProtoSceneryEntry[] linkedsceneryentries = new ProtoSceneryEntry[linkedsceneryentrycount];
            TextureChunk[][] totaltexturechunks = new TextureChunk[linkedsceneryentrycount][];
            for (int i = 0; i < linkedsceneryentrycount; i++)
            {
                linkedsceneryentries[i] = FindEID<ProtoSceneryEntry>(BitConv.FromInt32(ProtoZoneEntry.Header,4 + i * 64));
                TextureChunk[] texturechunks = new TextureChunk[BitConv.FromInt32(linkedsceneryentries[i].Info, 0x18)];
                for (int j = 0; j < texturechunks.Length; ++j)
                {
                    texturechunks[j] = FindEID<TextureChunk>(BitConv.FromInt32(linkedsceneryentries[i].Info, 0x20 + j * 4));
                }
                totaltexturechunks[i] = texturechunks;
            }
            for (int i = 0; i < linkedsceneryentrycount; i++)
            {
                linkedsceneryentries[i] = FindEID<ProtoSceneryEntry>(BitConv.FromInt32(ProtoZoneEntry.Header,4 + i * 64));
            }
            int linkedzoneentrycount = BitConv.FromInt32(ProtoZoneEntry.Header,528);
            ProtoZoneEntry[] linkedzoneentries = new ProtoZoneEntry[linkedzoneentrycount];
            for (int i = 0; i < linkedzoneentrycount; i++)
            {
                linkedzoneentries[i] = FindEID<ProtoZoneEntry>(BitConv.FromInt32(ProtoZoneEntry.Header,532 + i * 4));
            }
            return new UndockableControl(new ProtoZoneEntryViewer(ProtoZoneEntry,linkedsceneryentries,totaltexturechunks,linkedzoneentries));
        }

        public ProtoZoneEntry ProtoZoneEntry { get; }

        private void Menu_ExportAsC1()
        {
            byte[] header = new byte[0x378];
            // convert header
            Array.Copy(ProtoZoneEntry.Header,0,header,0,0x228);
            Array.Copy(ProtoZoneEntry.Header,0x228,header,0x234,0xB0);
            Array.Copy(ProtoZoneEntry.Header,0x2EC,header,0x318,0x60);
            BitConv.ToInt32(header,0x304,Entry.NullEID);
            // convert layout
            short xmax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x1E);
            short ymax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x20);
            short zmax = BitConv.FromInt16(ProtoZoneEntry.Layout,0x22);
            if (ymax == 0) ymax = xmax;
            if (zmax == 0) zmax = ymax;
            byte[] layout = new byte[ProtoZoneEntry.Layout.Length];
            ProtoZoneEntry.Layout.CopyTo(layout,0);
            BitConv.ToInt16(layout,0x1E,xmax);
            BitConv.ToInt16(layout,0x20,ymax);
            BitConv.ToInt16(layout,0x22,zmax);
            // convert entities - cameras have the same format so no conversion is necessary
            List<OldEntity> entities = new List<OldEntity>();
            foreach (ProtoEntity protoentity in ProtoZoneEntry.Entities)
            {
                List<EntityPosition> pos = new List<EntityPosition>();
                short x = (short)(protoentity.StartX/4);
                short y = (short)(protoentity.StartY/4);
                short z = (short)(protoentity.StartZ/4);
                pos.Add(new EntityPosition(x,y,z));
                foreach (ProtoEntityPosition delta in protoentity.Deltas)
                {
                    x += (short)(delta.X*2);
                    y += (short)(delta.Y*2);
                    z += (short)(delta.Z*2);
                    pos.Add(new EntityPosition(x,y,z));
                }
                entities.Add(new OldEntity(protoentity.Flags,protoentity.Spawn,protoentity.Unk,(short)(protoentity.ID+5),protoentity.VecX,protoentity.VecY,protoentity.VecZ,protoentity.Type,protoentity.Subtype,pos,protoentity.Nullfield1));
            }
            OldZoneEntry newzone = new OldZoneEntry(header,layout,ProtoZoneEntry.Cameras,entities,ProtoZoneEntry.EID);
            FileUtil.SaveFile(newzone.Save(),FileFilters.NSEntry,FileFilters.Any);
        }
    }
}
