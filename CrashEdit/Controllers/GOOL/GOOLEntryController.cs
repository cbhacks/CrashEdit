using CrashEdit.Crash;
using CrashEdit.Crash.GOOLIns;
using System.Windows.Forms;

namespace CrashEdit.CE
{
    [OrphanLegacyController(typeof(GOOLEntry))]
    public sealed class GOOLEntryController : EntryController
    {
        public GOOLEntryController(GOOLEntry goolentry, SubcontrollerGroup parentGroup) : base(goolentry, parentGroup)
        {
            GOOLEntry = goolentry;
            foreach (int ext_eid in goolentry.Externals)
            {
                GOOLEntry ext_gool = GetEntry<GOOLEntry>(ext_eid);
                if (ext_gool != null)
                {
                    ext_gool.ParentGOOL = goolentry;
                }
            }
            //if (GOOLEntry.Version == GOOLVersion.Version0)
            //    AddMenu("Export as Crash 1 GOOL", Menu_ExportAsC1);
        }

        public override bool EditorAvailable => true;

        public override Control CreateEditor()
        {
            return new GOOLBox(GOOLEntry);
        }

        public GOOLEntry GOOLEntry { get; }

        public void Menu_ExportAsC1()
        {
            byte[] newinstructions = new byte [GOOLEntry.Instructions.Count * 4];
            for (int i = 0; i < GOOLEntry.Instructions.Count; ++i)
            {
                if (GOOLEntry.Instructions[i] is Cfl_95 cfl)
                {
                    int newval = cfl.Save();
                    newval = (newval & ~0x3FF) | cfl.Args['I'].Value & 0x3FF;
                    newval = (newval & ~0b11110000000000) | (cfl.Args['V'].Value << 10);
                    BitConv.ToInt32(newinstructions,i*4, newval);
                }
                else
                    BitConv.ToInt32(newinstructions,i*4,GOOLEntry.Instructions[i].Save());
            }
            GOOLEntry newgool = new GOOLEntry(GOOLVersion.Version1,GOOLEntry.Header,newinstructions,GOOLEntry.Data,GOOLEntry.StateMap,GOOLEntry.StateDescriptors,GOOLEntry.Anims,GOOLEntry.EID);
            FileUtil.SaveFile(newgool.Save(), FileFilters.NSEntry, FileFilters.Any);
        }
    }
}
