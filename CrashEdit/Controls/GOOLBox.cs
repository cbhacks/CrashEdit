using CrashEdit.Crash;

namespace CrashEdit.CE
{
    public sealed class GOOLBox : UserControl
    {
        private readonly ListBox lstCode;
        private GOOLEntry gool;

        public GOOLBox(GOOLEntry goolentry)
        {
            gool = goolentry;
            lstCode = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Cascadia Code SemiLight", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0)
            };
            lstCode.Items.Add($"Type: {goolentry.ID}");
            lstCode.Items.Add($"Class: {goolentry.Class / 0x100}");
            lstCode.Items.Add($"Format: {goolentry.Format}");
            lstCode.Items.Add(string.Format("Heap Base: {0} ({1})", (ObjectFields)goolentry.HeapBase, (goolentry.HeapBase * 4 + GOOLInterpreter.GetProcessOff(goolentry.Version)).TransformedString()));
            lstCode.Items.Add($"Interrupt Count: {goolentry.EventCount}");
            lstCode.Items.Add($"Entry Count: {goolentry.EntryCount}");
            Dictionary<int, List<string>> labels = [];
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
                            lstCode.Items.Add($"    Interrupt {i}: Sub_{goolentry.StateMap[i] & 0x3FFF}");
                        else
                            lstCode.Items.Add($"    Interrupt {i}: State_{goolentry.StateMap[i]}");
                    }
                }

                lstCode.Items.Add($"Available Subtypes: {goolentry.StateMap.Length - goolentry.EventCount}");
                for (int i = goolentry.EventCount; i < goolentry.StateMap.Length; ++i)
                {
                    if (i > goolentry.EventCount && i + 1 == goolentry.StateMap.Length && goolentry.StateMap[i] == 0) continue;
                    lstCode.Items.Add($"    Subtype {i - goolentry.EventCount}: {(goolentry.StateMap[i] == 255 ? "invalid" : $"State_{goolentry.StateMap[i]}")}");
                }

                lstCode.Items.Add("");
                for (int i = 0; i < goolentry.StateDescriptors.Count; ++i)
                {
                    short epc = (short)(goolentry.StateDescriptors[i].EventHook & 0x3FFF);
                    short tpc = (short)(goolentry.StateDescriptors[i].TransHook & 0x3FFF);
                    short cpc = (short)(goolentry.StateDescriptors[i].CodeHook & 0x3FFF);
                    int stategooleid = goolentry.Data[goolentry.StateDescriptors[i].GOOLIndex];
                    lstCode.Items.Add($"State_{i} [{Entry.EIDToEName(stategooleid)}] (State Flags: {string.Format("0x{0:X}", goolentry.StateDescriptors[i].StateFlags)} | Block Flags: {string.Format("0x{0:X}", goolentry.StateDescriptors[i].BlockFlags)})");
                    if (epc != 0x3FFF)
                        lstCode.Items.Add($"    Event: {epc}" + ((goolentry.StateDescriptors[i].EventHook & 0x4000) != 0 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("      (no event hook)");
                    if (cpc != 0x3FFF)
                        lstCode.Items.Add($"    Code: {cpc}" + ((goolentry.StateDescriptors[i].CodeHook & 0x4000) != 0 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("      ERROR! No code thread! This state will not work.");
                    if (tpc != 0x3FFF)
                        lstCode.Items.Add($"    Trans: {tpc}" + ((goolentry.StateDescriptors[i].TransHook & 0x4000) != 0 ? " (external)" : ""));
                    else
                        lstCode.Items.Add("      (no trans hook)");

                    if (stategooleid == goolentry.EID)
                    {
                        if (cpc != 0x3FFF)
                        {
                            if (!labels.ContainsKey(cpc))
                                labels.Add(cpc, new());
                            labels[cpc].Add($"State_{i}_code:");
                        }
                        if (epc != 0x3FFF)
                        {
                            if (!labels.ContainsKey(epc))
                                labels.Add(epc, new());
                            labels[epc].Add($"State_{i}_event:");
                        }
                        if (tpc != 0x3FFF)
                        {
                            if (!labels.ContainsKey(tpc))
                                labels.Add(tpc, new());
                            labels[tpc].Add($"State_{i}_trans:");
                        }
                    }
                }
            }

            lstCode.Items.Add("");
            bool returned = true;
            int mipscount = 0;
            int goolcount = 0;
            string str;
            for (short i = 0; i < goolentry.Instructions.Count; ++i)
            {
                if (labels.ContainsKey(i))
                {
                    foreach (string label in labels[i])
                    {
                        lstCode.Items.Add(label);
                    }
                    returned = false;
                }
                if (returned)
                {
                    lstCode.Items.Add($"Sub_{i}:");
                }
                GOOLInstruction ins = goolentry.Instructions[i];
                if (ins is MIPSInstruction)
                {
                    returned = goolentry.Instructions[i - 1].Value == 0x03E00008 && goolentry.Instructions[i - 1] is MIPSInstruction;
                    ++mipscount;
                }
                else
                {
                    returned = GOOLInterpreter.IsReturnInstruction(ins);
                    if (ins is not GOOLUnknownInstruction)
                        ++goolcount;
                }
                string name = ins.GetName();
                string args = ins.Arguments;
                string comment = ins.GetComment();
                lstCode.Items.Add(string.Format("{0,-6} {1,-6} {2,-28} {3}", i, name, args, !string.IsNullOrWhiteSpace(comment) ? $"# {comment}" : ""));
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            gool.Decompile();
        }
    }
}
