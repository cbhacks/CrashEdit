using CrashEdit.Crash.GOOLIns;
using System.Reflection;
using System.Windows;
using static CrashEdit.Crash.GOOLDecompBlock;

namespace CrashEdit.Crash
{
    public sealed class GOOLEntry : Entry
    {
        internal static Dictionary<GOOLVersion, Dictionary<int, Type>> opsets;

        static GOOLEntry()
        {
            var assembly_types = Assembly.GetExecutingAssembly().GetTypes();
            opsets = [];
            foreach (Type type in assembly_types)
            {
                if (type.IsSubclassOf(typeof(GOOLInsOpcode)))
                {
                    foreach (GOOLInstructionAttribute attribute in type.GetCustomAttributes(typeof(GOOLInstructionAttribute), false))
                    {
                        if (!opsets.TryGetValue(attribute.Version, out var value))
                        {
                            value = [];
                            opsets.Add(attribute.Version, value);
                        }
                        Dictionary<int, Type> opset = value;
                        opset.TryAdd(attribute.Opcode, type);
                    }
                }
            }
        }

        internal GOOLInstruction LoadInstruction(int ins, bool mips)
        {
            if (!mips)
            {
                if (opsets.TryGetValue(Version, out var value))
                {
                    Dictionary<int, Type> opset = value;
                    int opcode = ins >> 24 & 0xFF;
                    if (opset.TryGetValue(opcode, out Type? opcodetype))
                    {
                        return new GOOLInstruction(ins, this, opcodetype);
                    }
                }
                return new GOOLUnknownInstruction(ins, this);
            }
            else
            {
                return new MIPSInstruction(ins, this);
            }
        }

        private readonly List<GOOLInstruction> instructions;
        private readonly List<GOOLStateDescriptor> statedescriptors;
        private readonly List<GOOLFrameGroupBase> framegroups;
        private readonly List<int> externals;

        public GOOLEntry(GOOLVersion version, byte[] header, byte[] instructions, int[] data, short[] statemap, IEnumerable<GOOLStateDescriptor> statedescriptors, IEnumerable<GOOLFrameGroupBase> fgroups, int eid) : base(eid)
        {
            Version = version;
            Header = header;
            this.instructions = new();
            bool mips = false;
            for (int i = 0; i < instructions.Length / 4; ++i)
            {
                int encins = BitConv.FromInt32(instructions, i * 4);
                GOOLInstruction ins = LoadInstruction(encins, mips);
                if (version == GOOLVersion.Version3 && (ins.ID == 142 || ins.ID == 174 || encins == 0x26D6FFE4 || encins == 0x00002821 || encins == 0x34050001 || (uint)encins == 0x8C670094U))
                {
                    mips = true;
                    ins = LoadInstruction(BitConv.FromInt32(instructions, i * 4), mips);
                }
                this.instructions.Add(ins);
                if (mips)
                {
                    MIPSInstruction prev = null;
                    if (this.instructions[^2] is MIPSInstruction mips_ins)
                        prev = mips_ins;
                    if (prev != null && (prev.Value == 0x03E0A809 || prev.Value == 0x03E00008)) // native mips returns or ends here
                        mips = false;
                }
                else
                    mips = GOOLInterpreter.IsMIPSInstruction(ins);
            }
            Data = data;
            StateMap = statemap;
            externals = new();
            if (statedescriptors == null)
                this.statedescriptors = null;
            else
            {
                this.statedescriptors = new List<GOOLStateDescriptor>(statedescriptors);
                foreach (var state in this.statedescriptors)
                {
                    int ext_eid = Data[state.GOOLIndex];
                    if (ext_eid != eid && !externals.Contains(ext_eid))
                    {
                        externals.Add(ext_eid);
                    }
                }
            }
            framegroups = fgroups == null && version == GOOLVersion.Version0 ? null : new(fgroups);
        }

        public override string Title => Version switch
        {
            GOOLVersion.Version0 => $"Prototype GOOL ({EName})",
            GOOLVersion.Version1 => $"GOOL ({EName})",
            GOOLVersion.Version2 => $"GOOLv2 ({EName})",
            GOOLVersion.Version3 => $"GOOLv3 ({EName})",
            _ => $"GOOL ({EName})",
        };

        public override string ImageKey => "ThingCode";

        public override int Type => 11;

        public GOOLVersion Version { get; }

        public byte[] Header { get; }
        public int[] Data { get; }
        public short[] StateMap { get; }
        public IList<GOOLStateDescriptor> StateDescriptors => statedescriptors;
        public IList<GOOLFrameGroupBase> FrameGroups => framegroups;

        public int ID => BitConv.FromInt32(Header, 0);
        public int Class => BitConv.FromInt32(Header, 4);
        public int Format => BitConv.FromInt32(Header, 8);
        public int HeapBase => BitConv.FromInt32(Header, 12);
        public int EventCount => BitConv.FromInt32(Header, 16);
        public int EntryCount => BitConv.FromInt32(Header, 20);

        public IList<GOOLInstruction> Instructions => instructions;

        public IList<int> Externals => externals;
        public GOOLEntry ParentGOOL { get; set; }

        public void Decompile()
        {
            foreach (var ins in Instructions)
            {
                if (ins is MIPSInstruction)
                    return;
            }

            if (Format == 0)
                return;

            List<GOOLDecompFunctionInfo> funcs = new();
            for (int i = 0; i < StateDescriptors.Count; ++i)
            {
                var state = StateDescriptors[i];
                int t = state.TransHook & 0x3FFF;
                int c = state.CodeHook & 0x3FFF;
                int e = state.EventHook & 0x3FFF;
                if (t != 0x3FFF)
                {
                    funcs.Add(new($"state_{i}_trans") { Offset = t, Trans = true });
                }
                if (c != 0x3FFF)
                {
                    funcs.Add(new($"state_{i}_code") { Offset = c });
                }
                if (e != 0x3FFF)
                {
                    funcs.Add(new($"state_{i}_event") { Offset = e });
                }
            }

            List<int> labels = new();
            void try_add_label(int ofs)
            {
                if (!labels.Contains(ofs))
                    labels.Add(ofs);
            }

            foreach (var func in funcs)
            {
                try_add_label(func.Offset);
            }
            for (int i = 0; i < Instructions.Count; ++i)
            {
                var ins = Instructions[i];
                if ((ins.GetName() == "BRA" && ins.Args['I'].Value != 0) || ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
                {
                    try_add_label(i + 1 + ins.Args['I'].Value);
                    try_add_label(i + 1);
                }
                else if ((ins.Type == typeof(Rjev) || ins.Type == typeof(Acev)) && ins.Args['T'].Value == 0)
                {
                    try_add_label(i + 1 + ins.Args['I'].Value);
                    try_add_label(i + 1);
                }
                else if (ins.GetName() == "CALL" || ins.GetName() == "SETC")
                {
                    int ofs = ins.Args['I'].Value;
                    try_add_label(ofs);
                    if (funcs.Find(x => x.Offset == ofs) == null)
                    {
                        funcs.Add(new($"func_{ofs}") { Offset = ofs });
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    try_add_label(i + 1);
                }
            }

            labels.Sort();
            try_add_label(Instructions.Count);

            List<GOOLDecompBlock> blocks = new();
            for (int i = 0; i < labels.Count - 1; ++i)
            {
                GOOLDecompBlock block = new($"B{i}_{labels[i]}_{labels[i+1]}");
                block.begin = labels[i];
                block.end = labels[i+1];
                block.instructions.AddRange(Instructions.Skip(block.begin).Take(block.end - block.begin));
                blocks.Add(block);
            }
            GOOLDecompBlock block_from_index(int index)
            {
                return blocks[labels.IndexOf(index)];
            }
            foreach (var block in blocks)
            {
                var ins = block.instructions.Last();
                int i = Instructions.IndexOf(ins);
                if (ins.GetName() == "BRA" && ins.Args['I'].Value != 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.next.Add(block_from_index(i + 1 + branch));
                    block.Type = BranchType.Goto;
                    // dead code detection: assume unconditional backwards-facing branches have code in front of them
                    // we still consider the branch unconditional!
                    if (branch < 0)
                    {
                        block.next.Add(block_from_index(i + 1));
                    }
                }
                else if (ins.GetName() == "BNEZ" || ins.GetName() == "BEQZ")
                {
                    block.next.Add(block_from_index(i + 1 + ins.Args['I'].Value));
                    block.next.Add(block_from_index(i + 1));
                    block.Type = BranchType.If;
                }
                else if ((ins.Type == typeof(Rjev) || ins.Type == typeof(Acev)) && ins.Args['T'].Value == 0)
                {
                    int branch = ins.Args['I'].Value;
                    block.next.Add(block_from_index(i + 1 + branch));
                    if (ins.Args['C'].Value != 0)
                    {
                        block.next.Add(block_from_index(i + 1));
                        block.Type = BranchType.If;
                    }
                    else
                    {
                        block.Type = BranchType.Goto;
                        // dead code detection: assume unconditional backwards-facing branches have code in front of them
                        // we still consider the branch unconditional!
                        if (branch < 0)
                        {
                            block.next.Add(block_from_index(i + 1));
                        }
                    }
                }
                else if (ins.GetName() == "RET")
                {
                    block.Type = BranchType.Return;
                }
                else
                {
                    block.next.Add(block_from_index(i + 1));
                    block.Type = BranchType.None;
                }

                foreach(var next in block.next)
                {
                    next.prev.Add(block);
                }
            }

            GOOLDecompFunctionInfo? find_func_with_block_name(string name)
            {
                foreach (var func in funcs)
                {
                    if (func.BlockList.Any((block) => block.name == name))
                        return func;
                }
                return null;
            }
            foreach (var func in funcs)
            {
                func.start = block_from_index(func.Offset);
            }

            // generate block list and (post-)dominators for each function's blocks and check for this
            // we do this for each function instead of every block in file because O(n^2) scaling and there is no inter-procedural CFG (that we care about)
            foreach (var func in funcs)
            {
                func.Initialize();
            }
            foreach (var block in blocks)
            {
                if (block.instructions.Count >= 3 && block.Type == BranchType.Goto && block.next.Count == 1 &&
                    block.instructions[^2].GetName() == "SETF" &&
                    block.instructions[^3].GetName() == "ADD" &&
                    block.instructions[^2].Arguments == "tpc,[sp]" &&
                   // so lazy!!
                   (block.instructions[^3].Arguments == "pc,(8)" || block.instructions[^3].Arguments == "pc,8"))
                {
                    var real_trans = block_from_index(block.end);
                    var enter_func = find_func_with_block_name(block.name);
                    if (enter_func != null)
                    {
                        block.next[0].prev.Remove(block);
                        block.next.Clear();
                        block.instructions.RemoveLast();
                        block.instructions.RemoveLast();
                        block.instructions.RemoveLast();
                        GOOLDecompFunctionInfo trans_func = new(enter_func.Name) { Offset = block.end };
                        funcs.Add(trans_func);
                        trans_func.start = block_from_index(trans_func.Offset);
                        enter_func.Name = enter_func.Name.Replace("trans", "enter");
                        // regenerate CFG
                        trans_func.Initialize();
                        enter_func.Initialize();
                    }
                }
            }
            // generate statements in each block
            foreach (var block in blocks)
            {
                block.statements.Clear();
                GOOLStatement cursment = null;
                int stackwant = 0;
                for (int i = block.instructions.Count - 1; i >= 0; --i)
                {
                    var ins = block.instructions[i];
                    cursment ??= new GOOLStatement();

                    if (cursment.Instructions.Count != 0)
                    {
                        stackwant -= ins.GetStackPush();
                    }

                    stackwant += ins.GetStackPop();

                    cursment.Instructions.Add(ins);

                    if (stackwant == 0)
                    {
                        // no more instructions to add to the statement, move on
                        block.statements.Insert(0, cursment);
                        cursment = null;
                    }

                    int ccc = 999;
                }

                if (cursment != null)
                {
                    Console.WriteLine($"Failed to build statements for block {block.name}: did not pop {stackwant} values!");
                }

                foreach (var statement in block.statements)
                {
                    statement.DecompileToLispFull();
                    string tempy = statement.LispOut.Print();
                    Console.WriteLine(tempy);
                    int eee = 999;
                }

                int ddd = 999;
            }
            foreach (var func in funcs)
            {
                // generate dominators for each block
                bool changed;
                GOOLDecompDomVector tested = new(func.BlockList.Count);
                do
                {
                    changed = false;
                    foreach(var block in func.BlockList)
                    {
                        foreach (var prev in block.prev)
                        {
                            tested.ClearAll();
                            tested.Merge(block.Dominators);
                            block.Dominators.Mask(prev.Dominators);
                            block.Dominators.Set(block.DomID);
                            if (!block.Dominators.Equal(tested))
                            {
                                changed = true;
                            }
                        }
                    }
                } while (changed);
                // generate post-dominators for each block
                do
                {
                    changed = false;
                    foreach (var block in func.BlockList)
                    {
                        foreach (var next in block.next)
                        {
                            tested.ClearAll();
                            tested.Merge(block.PostDominators);
                            block.PostDominators.Mask(next.PostDominators);
                            block.PostDominators.Set(block.DomID);
                            if (!block.PostDominators.Equal(tested))
                            {
                                changed = true;
                            }
                        }
                    }
                } while (changed);
                // grab immediate (post-)dominators
                foreach (var block in func.BlockList)
                {
                    List<GOOLDecompBlock> pdoms = func.BlockList.Where(b => b.DomID != block.DomID && block.PostDominators[b.DomID]).ToList();
                    int pdoms_count = block.PostDominators.Count;
                    foreach (var pdom in pdoms)
                    {
                        bool immediate = true;
                        foreach (var other in pdoms)
                        {
                            if (pdom.DomID != other.DomID && !other.PostDominators[pdom.DomID])
                            {
                                immediate = false;
                                break;
                            }
                        }
                        if (immediate)
                        {
                            block.ImmPostDom = pdom;
                        }
                    }
                    List<GOOLDecompBlock> doms = func.BlockList.Where(b => b.DomID != block.DomID && block.Dominators[b.DomID]).ToList();
                    int doms_count = block.Dominators.Count;
                    foreach (var dom in doms)
                    {
                        bool immediate = true;
                        foreach (var other in doms)
                        {
                            if (dom.DomID != other.DomID && !other.Dominators[dom.DomID])
                            {
                                immediate = false;
                                break;
                            }
                        }
                        if (immediate)
                        {
                            block.ImmDom = dom;
                        }
                    }
                }

                // check for loops
                // ---------------

                // loop gen function
                GOOLDecompLoop natural_loop_for_edge(GOOLDecompBlock header, GOOLDecompBlock tail)
                {
                    Stack<GOOLDecompBlock> workList = new();
                    GOOLDecompLoop loop = new(header, tail);
                    loop.BlockList.Add(header);
                    if (header != tail)
                    {
                        loop.BlockList.Add(tail);
                        workList.Push(tail);
                    }
                    while (workList.Count > 0)
                    {
                        var block = workList.Pop();
                        foreach (var prev in block.prev)
                        {
                            if (!loop.BlockList.Contains(prev))
                            {
                                loop.BlockList.Add(prev);
                                workList.Push(prev);
                            }
                        }
                    }
                    loop.BlockList.Sort((a, b) => a.begin - b.begin);
                    return loop;
                }
                //find loops in function
                for (int i = 1; i < func.BlockList.Count; ++i)
                {
                    var block = func.BlockList[i];
                    foreach (var next in block.next)
                    {
                        if (block.Dominators[next.DomID] /* next.begin <= block.begin */)
                        {
                            func.LoopList.Add(natural_loop_for_edge(next, block));
                        }
                    }
                }

                foreach(var loop in func.LoopList)
                {
                    loop.LoopDepth = 0;
                    foreach (var otherloop in func.LoopList)
                    {
                        if (otherloop == loop) continue;
                        if (loop.BlockList.All(otherloop.BlockList.Contains))
                        {
                            loop.LoopDepth++;
                            if (loop.Parent == null || otherloop.BlockList.Count < loop.Parent.BlockList.Count)
                                loop.Parent = otherloop;
                        }
                    }
                }

                // sort loops by 'depth' (descending) and assign children
                func.LoopList.Sort((a, b) => b.LoopDepth - a.LoopDepth);
                foreach (var loop in func.LoopList)
                {
                    loop.Parent?.Children.Add(loop);
                }

                continue;
                int l = 0;
                foreach (var loop in func.LoopList)
                {
                    var do_while = new GOOLDecompBlockDoWhile("dowhile_" + l++ + "_" + loop.BlockList[0].begin, loop, loop.BlockList[^1]);
                    do_while.DomID = do_while.Continue.DomID;
                    do_while.Dominators = do_while.Continue.Dominators;
                    do_while.PostDominators = do_while.Continue.PostDominators;
                    do_while.ImmDom = do_while.Continue.ImmDom;
                    do_while.ImmPostDom = do_while.Continue.ImmPostDom;

                    if (do_while.Continue.statements.Count != 1)
                    {
                        // we did not generate a block for a continue statement, so there must not be one!
                        do_while.Continue = null;
                    }
                    foreach (var fblk in func.BlockList)
                    {
                        if (!loop.BlockList.Contains(fblk))
                        {
                            for (int i = fblk.prev.Count-1; i >= 0; --i)
                            {
                                var blk = fblk.prev[i];
                                if (loop.BlockList.Contains(blk))
                                {
                                    fblk.prev.RemoveAt(i);
                                    if (!fblk.prev.Contains(do_while))
                                    {
                                        fblk.prev.Add(do_while);
                                    }
                                }
                            }
                            for (int i = fblk.next.Count-1; i >= 0; --i)
                            {
                                var blk = fblk.next[i];
                                if (loop.BlockList.Contains(blk))
                                {
                                    fblk.next.RemoveAt(i);
                                    if (!fblk.next.Contains(do_while))
                                    {
                                        fblk.next.Add(do_while);
                                    }
                                }
                            }
                        }
                    }
                    if (loop.BlockList.Contains(func.start))
                    {
                        func.start = do_while;
                    }
                    foreach (var lblk in loop.BlockList)
                    {
                        func.BlockList.Remove(lblk);
                        //blocks.Remove(lblk);
                        do_while.instructions.AddRange(lblk.instructions);
                        do_while.next.AddRange(lblk.next.Where((a) => !loop.BlockList.Contains(a)));
                        do_while.prev.AddRange(lblk.prev.Where((a) => !loop.BlockList.Contains(a)));
                        lblk.next.Clear();
                        lblk.prev.Clear();
                    }
                    do_while.next = do_while.next.Distinct().ToList();
                    do_while.prev = do_while.prev.Distinct().ToList();
                    blocks.Add(do_while);
                }

                int b = 9999;
            }

            string debug = "digraph CFG {\n";
            debug += "  node [style=filled shape=ellipse]\n";
            foreach (var block in blocks)
            {
                debug += $"  {block.name} [ fillcolor={(block is GOOLDecompBlockDoWhile ? "red" : "lightgray")} label=\"{block.name}\\n";
                foreach (var ins in block.instructions)
                {
                    debug += string.Format("{0,-6} {1,-28}\\n", ins.GetName(), ins.Arguments);
                }
                debug += $"\" ];\n";
                foreach (var next in block.next)
                {
                    debug += $"  {block.name} -> {next.name} [color={(next.begin <= block.begin ? "red" : "black")}]\n";
                }
            }
            debug += "\n";
            foreach (var func in funcs)
            {
                debug += $"  {func.Name} -> {func.start.name} [color=purple]\n";
                foreach (var block in func.BlockList)
                {
                    if (block.ImmPostDom != null)
                    {
                        //debug += $"  {block.name} -> {block.ImmPostDom.name} [color=green]\n";
                    }
                    if (block.ImmDom != null)
                    {
                        //debug += $"  {block.name} -> {block.ImmDom.name} [color=cyan]\n";
                    }
                    for (int i = 0; i < func.BlockList.Count; ++i)
                    {
                        var targetblock = func.BlockList[i];
                        // every node dominates and postdominates itself
                        if (block == targetblock) continue;
                        if (block.Dominators[targetblock.DomID])
                        {
                            //debug += $"  {block.name} -> {targetblock.name} [color=yellow]\n";
                        }
                        if (block.PostDominators[targetblock.DomID])
                        {
                            //debug += $"  {block.name} -> {targetblock.name} [color=blue]\n";
                        }
                    }
                }
            }
            int li = 0;
            foreach (var func in funcs)
            {
                void print_loop(int indent, GOOLDecompLoop loop)
                {
                    var istr = new string(' ', indent);
                    debug += istr + $"  subgraph cluster_loop_{li} {{\n";
                    debug += istr + $"    label = \"loop_{li}\\nheader: {loop.Header.name}\\ntail: {loop.Tail.name}\";\n";
                    li++;
                    debug += istr + $"    style = filled;\n";
                    debug += istr + $"    fontsize = \"25pt\";\n";
                    debug += istr + $"    fillcolor = lightyellow;\n";
                    foreach (var ch in loop.Children)
                    {
                        print_loop(indent + 2, ch);
                    }
                    foreach (var block in loop.BlockList.Where(a => !loop.Children.Any(c => c.BlockList.Contains(a))))
                    {
                        if (block == loop.Header && block == loop.Tail)
                            debug += istr + $"    {block.name} [fillcolor=yellow]\n";
                        else if (block == loop.Tail)
                            debug += istr + $"    {block.name} [fillcolor=pink]\n";
                        else if (block == loop.Header)
                            debug += istr + $"    {block.name} [fillcolor=lightgreen]\n";
                        else
                            debug += istr + $"    {block.name}\n";
                    }
                    debug += istr + $"  }}\n";
                }
                foreach (var loop in func.LoopList)
                {
                    if (loop.Parent == null) print_loop(0, loop);
                }
            }
            debug += "}\n";

            File.WriteAllText("test-decomp.txt", debug);
            Console.Write(debug);
            Clipboard.SetText(debug);
            Console.WriteLine("Copied to clipboard!");

            int aaaaaa = 0;
        }

        public override UnprocessedEntry Unprocess()
        {
            int itemcount = Format == 1 ? (FrameGroups == null ? 5 : 6) : 3;

            byte[][] items = new byte[itemcount][];
            items[0] = Header;
            items[1] = new byte[instructions.Count * 4];
            for (int i = 0; i < instructions.Count; ++i)
            {
                BitConv.ToInt32(items[1], i * 4, instructions[i].Save());
            }
            items[2] = new byte[Data.Length * 4];
            for (int i = 0; i < Data.Length; ++i)
            {
                BitConv.ToInt32(items[2], i * 4, Data[i]);
            }
            if (Format == 1)
            {
                items[3] = new byte[StateMap.Length * 2];
                for (int i = 0; i < StateMap.Length; ++i)
                {
                    BitConv.ToInt16(items[3], i * 2, StateMap[i]);
                }
                items[4] = new byte[statedescriptors.Count * 0x10];
                for (int i = 0; i < statedescriptors.Count; ++i)
                {
                    statedescriptors[i].Save().CopyTo(items[4], i * 0x10);
                }
                if (FrameGroups != null)
                {
                    items[5] = new byte[0];
                    foreach (var group in FrameGroups)
                    {
                        var data = group.Save();
                        Array.Resize(ref items[5], data.Length + items[5].Length);
                        Array.Copy(data, 0, items[5], items[5].Length - data.Length, data.Length);
                    }
                }
            }
            return new UnprocessedEntry(items, EID, Type);
        }
    }
}
