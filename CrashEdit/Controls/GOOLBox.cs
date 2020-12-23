using Crash;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class GOOLBox : UserControl
    {
        private ListBox lstCode;

        public GOOLBox(GOOLEntry goolentry)
        {
            lstCode = new ListBox
            {
                Dock = DockStyle.Fill
            };
            lstCode.Items.Add($"Type: {goolentry.ID}");
            lstCode.Items.Add($"Category: {goolentry.Category / 0x100}");
            lstCode.Items.Add($"Format: {goolentry.Format}");
            lstCode.Items.Add(string.Format("Stack Start: {0} ({1})",(ObjectFields)goolentry.StackStart,(goolentry.StackStart*4+GOOLInterpreter.GetProcessOff(goolentry.Version)).TransformedString()));
            lstCode.Items.Add($"Interrupt Count: {goolentry.EventCount}");
            lstCode.Items.Add($"Entry Count: {goolentry.EntryCount}");
            if (goolentry.Format == 1)
            {
                lstCode.Items.Add("");
                bool addedinterrupts = false;
                for (int i = 0; i < goolentry.EventCount; ++i)
                {
                    if (goolentry.StateMap[i] == 255)
                        continue;
                    else
                    {
                        if (!addedinterrupts)
                        {
                            lstCode.Items.Add("Interrupts:");
                            addedinterrupts = true;
                        }
                        if ((goolentry.StateMap[i] & 0x8000) != 0)
                            lstCode.Items.Add($"\tInterrupt {i}: Sub_{goolentry.StateMap[i] & 0x3FFF}");
                        else
                            lstCode.Items.Add($"\tInterrupt {i}: State_{goolentry.StateMap[i]}");
                    }
                }

                lstCode.Items.Add($"Available Subtypes: {goolentry.StateMap.Length - goolentry.EventCount}");
                for (int i = goolentry.EventCount; i < goolentry.StateMap.Length; ++i)
                {
                    if (i > goolentry.EventCount && i+1 == goolentry.StateMap.Length && goolentry.StateMap[i] == 0) continue;
                    lstCode.Items.Add($"\tSubtype {i - goolentry.EventCount}: {(goolentry.StateMap[i] == 255 ? "invalid" : $"State_{goolentry.StateMap[i]}")}");
                }

                lstCode.Items.Add("");
                for (int i = 0; i < goolentry.StateDescriptors.Count; ++i)
                {
                    short epc = (short)(goolentry.StateDescriptors[i].EPC & 0x3FFF);
                    short tpc = (short)(goolentry.StateDescriptors[i].TPC & 0x3FFF);
                    short cpc = (short)(goolentry.StateDescriptors[i].CPC & 0x3FFF);
                    int stategooleid = goolentry.Data[goolentry.StateDescriptors[i].GOOLID];
                    lstCode.Items.Add($"State_{i} [{Entry.EIDToEName(stategooleid)}] (State Flags: {string.Format("0x{0:X}",goolentry.StateDescriptors[i].StateFlags)} | C-Flags: {string.Format("0x{0:X}",goolentry.StateDescriptors[i].CFlags)})");
                    if (epc != 0x3FFF)
                        lstCode.Items.Add($"\tEPC: {epc}" + ((goolentry.StateDescriptors[i].EPC & 0x4000) == 0x4000 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("\tEvent block unavailable.");
                    if (tpc != 0x3FFF)
                        lstCode.Items.Add($"\tTPC: {tpc}" + ((goolentry.StateDescriptors[i].TPC & 0x4000) == 0x4000 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("\tTrans block unavailable.");
                    if (cpc != 0x3FFF)
                        lstCode.Items.Add($"\tCPC: {cpc}" + ((goolentry.StateDescriptors[i].CPC & 0x4000) == 0x4000 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("\tCode block unavailable.");
                }
            }

            lstCode.Items.Add("");
            bool returned = true;
            int mipscount = 0;
            int goolcount = 0;
            string str;
            for (short i = 0; i < goolentry.Instructions.Count; ++i)
            {
                if (goolentry.StateDescriptors != null)
                {
                    for (int j = 0; j < goolentry.StateDescriptors.Count; ++j)
                    {
                        GOOLStateDescriptor desc = goolentry.StateDescriptors[j];
                        if (goolentry.Data[desc.GOOLID] != goolentry.EID)
                            continue;
                        int cpc = desc.CPC & 0x3FFF;
                        int tpc = desc.TPC & 0x3FFF;
                        int epc = desc.EPC & 0x3FFF;
                        if (cpc == i && cpc != 0x3FFF)
                        {
                            lstCode.Items.Add($"State_{j}_cpc:");
                            returned = false;
                        }
                        if (tpc == i && tpc != 0x3FFF)
                        {
                            lstCode.Items.Add($"State_{j}_tpc:");
                            returned = false;
                        }
                        if (epc == i && epc != 0x3FFF)
                        {
                            lstCode.Items.Add($"State_{j}_epc:");
                            returned = false;
                        }
                    }
                }
                if (returned)
                {
                    lstCode.Items.Add($"Sub_{i}:");
                }
                GOOLInstruction ins = goolentry.Instructions[i];
                if (ins is MIPSInstruction)
                {
                    returned = goolentry.Instructions[i-1].Value == 0x03E00008 && goolentry.Instructions[i-1] is MIPSInstruction;
                    ++mipscount;
                }
                else
                {
                    returned = GOOLInterpreter.IsReturnInstruction(ins);
                    if (!(ins is GOOLInvalidInstruction))
                        ++goolcount;
                }
                string comment = ins.Comment;
                lstCode.Items.Add(string.Format("{0,-05}\t{1,-4}\t{2,-30}\t{3}",i,ins.Name,ins.Arguments,!string.IsNullOrWhiteSpace(comment) ? $"# {comment}" : ""));
            }

            if (goolcount != goolentry.Instructions.Count)
            {
                lstCode.Items.Add("");
                str = string.Format("Instructions: {0:P} GOOL", (float)goolcount / goolentry.Instructions.Count);
                if (mipscount > 0)
                    str += string.Format(", {0:P} MIPS", (float)mipscount / goolentry.Instructions.Count);
                if (goolentry.Instructions.Count - mipscount - goolcount > 0)
                    str += string.Format(", {0:P} invalid", (float)(goolentry.Instructions.Count - mipscount - goolcount) / goolentry.Instructions.Count);
                lstCode.Items.Add(str);
            }

            Controls.Add(lstCode);
        }
    }
}
