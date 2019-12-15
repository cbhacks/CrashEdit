using Crash;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrashEdit
{
    public sealed class GOOLBox : UserControl
    {
        private GOOLEntry goolentry;

        private ListBox lstCode;

        public GOOLBox(GOOLEntry goolentry)
        {
            this.goolentry = goolentry;
            lstCode = new ListBox
            {
                Dock = DockStyle.Fill
            };
            int interruptcount = BitConv.FromInt32(goolentry.Header,16);
            lstCode.Items.Add($"Type: {BitConv.FromInt32(goolentry.Header,0)}");
            lstCode.Items.Add($"Category: {BitConv.FromInt32(goolentry.Header,4) / 0x100}");
            lstCode.Items.Add($"Format: {goolentry.Format}");
            lstCode.Items.Add(string.Format("Stack Start: {0} ({1})",(ObjectFields)BitConv.FromInt32(goolentry.Header,12),(BitConv.FromInt32(goolentry.Header,12)*4+GOOLInterpreter.GetProcessOff(goolentry.Version)).TransformedString()));
            lstCode.Items.Add($"Interrupt Count: {interruptcount}");
            lstCode.Items.Add($"unknown: {BitConv.FromInt32(goolentry.Header,20)}");
            if (BitConv.FromInt32(goolentry.Header, 8) == 1)
            {
                lstCode.Items.Add("");
                lstCode.Items.Add("Interrupts:");
                for (int i = 0; i < interruptcount; ++i)
                {
                    if (goolentry.StateMap[i] == 255)
                        continue;
                    else if ((goolentry.StateMap[i] & 0x8000) != 0)
                        lstCode.Items.Add($"\tInterrupt {i}: Sub_{goolentry.StateMap[i] & 0x3FFF}");
                    else
                        lstCode.Items.Add($"\tInterrupt {i}: State_{goolentry.StateMap[i]}");
                }

                lstCode.Items.Add($"Available Subtypes: {goolentry.StateMap.Length - interruptcount}");
                for (int i = interruptcount; i < goolentry.StateMap.Length; ++i)
                {
                    if (i > interruptcount && i+1 == goolentry.StateMap.Length && goolentry.StateMap[i] == 0) continue;
                    lstCode.Items.Add($"\tSubtype {i - interruptcount}: {(goolentry.StateMap[i] == 255 ? "invalid" : $"State_{goolentry.StateMap[i]}")}");
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
                        lstCode.Items.Add($"\tEPC: {epc}");
                    else
                        lstCode.Items.Add("\tEvent block unavailable.");
                    if (tpc != 0x3FFF)
                        lstCode.Items.Add($"\tTPC: {tpc}");
                    else
                        lstCode.Items.Add("\tTrans block unavailable.");
                    if (cpc != 0x3FFF)
                        lstCode.Items.Add($"\tCPC: {cpc}");
                    else
                        lstCode.Items.Add("\tCode block unavailable.");
                }
            }

            lstCode.Items.Add("");
            bool returned = false;
            int mipscount = 0;
            int goolcount = 0;
            string str;
            for (short i = 0; i < goolentry.Instructions.Count; ++i)
            {
                for (int j = 0; j < goolentry.StateDescriptors.Count; ++j)
                {
                    GOOLStateDescriptor desc = goolentry.StateDescriptors[j];
                    int cpc = desc.CPC & 0x3FFF;
                    int tpc = desc.TPC & 0x3FFF;
                    int epc = desc.EPC & 0x3FFF;
                    if (cpc == i && cpc != 0x3FFF)
                        lstCode.Items.Add($"State_{j}_cpc:");
                    if (tpc == i && tpc != 0x3FFF)
                        lstCode.Items.Add($"State_{j}_tpc:");
                    if (epc == i && epc != 0x3FFF)
                        lstCode.Items.Add($"State_{j}_epc:");
                }
                if (returned)
                {
                    lstCode.Items.Add($"Sub_{i}:");
                    returned = false;
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
                str = string.Format("Instructions: {0:0.00}% GOOL", goolcount * 100F / goolentry.Instructions.Count);
                if (mipscount > 0)
                    str += string.Format(", {0:0.00}% MIPS", mipscount * 100F / goolentry.Instructions.Count);
                if (goolentry.Instructions.Count - mipscount - goolcount > 0)
                    str += string.Format(", {0:0.00}% invalid", (goolentry.Instructions.Count - mipscount - goolcount) * 100F / goolentry.Instructions.Count);
                lstCode.Items.Add(str);
            }

            Controls.Add(lstCode);
        }
    }
}
