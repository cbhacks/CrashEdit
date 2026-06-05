namespace CrashEdit.Crash
{
    public enum GoolBranchType
    {
        None,
        Goto,
        If,
        Return,
        Continue,
        ContinueIf,
        Break,
        BreakIf,
        StackPop
    }

    public class GOOLDecompBlock(string name)
    {
        public List<GOOLInstruction> Instructions { get; } = new();
        public List<GOOLStatement> Statements { get; } = new();

        public List<GOOLDecompBlock> prev = new();
        public List<GOOLDecompBlock> next = new();

        public GoolBranchType Type { get; set; }

        public int DomID = -1;
        public int PostOrderID = -1;
        public GOOLDecompDomVector Dominators { get; set; } = null;
        public GOOLDecompDomVector PostDominators { get; set; } = null;
        public GOOLDecompBlock ImmDom { get; set; } = null;
        public GOOLDecompBlock ImmPostDom { get; set; } = null;

        public int StackPop { get; set; } = -1;

        public int begin;
        public int end;

        public string Name { get; private set; } = name;

        public string GetFullName() => Name + $" ({begin} ~ {end})";

        public void PatchNameTransToEnter()
        {
            Name = Name.Replace("trans", "enter");
        }

        public bool Dominates(GOOLDecompBlock other) => other.Dominators[DomID];
        public bool PostDominates(GOOLDecompBlock other) => other.PostDominators[DomID];
        public bool ImmediateDominates(GOOLDecompBlock other) => other.ImmDom == this;
        public bool ImmediatePostDominates(GOOLDecompBlock other) => other.ImmPostDom == this;

        public bool IsLetEnd() => Statements.Count == 1 && Statements[0].Type == GoolStatementType.LetEnd;
        public bool IsLetBegin() => Statements.Count == 1 && Statements[0].Type == GoolStatementType.StackPush;
        public bool IsLetBlock() => IsLetEnd() || IsLetBegin();
        public bool IsStackStarved() => Statements.Count == 1 && Statements[0].Type == GoolStatementType.StackStarve;
        public bool IsStackPop() => Type == GoolBranchType.StackPop || IsLetEnd() || IsStackStarved();

        public void VisitForward(Action<GOOLDecompBlock>? preVisit = null, Action<GOOLDecompBlock>? postVisit = null, HashSet<GOOLDecompBlock> visited = null)
        {
            visited ??= new();
            visited.Add(this);

            // pre-visit work
            preVisit?.Invoke(this);

            foreach (var block in next)
            {
                if (!visited.Contains(block)) block.VisitForward(preVisit, postVisit, visited);
            }

            // post-visit work
            postVisit?.Invoke(this);
        }

        // Generate the statements for the block out of its instructions.
        public void GenerateStatements()
        {
            Statements.Clear();
            GOOLStatement cursment = null;
            int stackwant = 0;
            if (Name == "B72")
            {
                int zzzzz = 99;
            }
            for (int i = Instructions.Count - 1; i >= 0; --i)
            {
                var ins = Instructions[i];
                cursment ??= new GOOLStatement();

                int push = ins.GetStackPush();
                int pop = ins.GetStackPop();
                if (cursment.Instructions.Count == 0 && push > 0)
                {
                    // statement pushes to stack (even if it has net stack value of zero)
                    //Console.WriteLine($"detected stack push in {Name} ins {i}");
                    cursment.Type = GoolStatementType.StackPush;
                    cursment.StackValue = push;
                }
                else if (cursment.Instructions.Count == 1 && pop > 0 && push == 0)
                {
                    //Console.WriteLine($"detected stack pop in {Name} ins {i}");
                    cursment.Type = GoolStatementType.LetEnd;
                    ++i; // note: we do not add this instruction, because the instruction we want is already there. instead we finish the stament and try this instruction again.
                    Statements.Insert(0, cursment);
                    cursment = null;
                    stackwant = 0;
                    continue;
                }

                if (cursment.Instructions.Count > 0)
                {
                    // we don't count the initial push because that will be consumed later(?)
                    stackwant -= push;
                }

                if (StackPop == -1 || Statements.Count > 0)
                {
                    stackwant += pop;
                }

                cursment.Instructions.Add(ins);

                if (stackwant == 0)
                {
                    // no more instructions to add to the statement, move on
                    Statements.Insert(0, cursment);
                    cursment = null;
                }
            }

            if (cursment != null)
            {
                if (cursment.Type == GoolStatementType.Normal)
                {
                    cursment.Type = GoolStatementType.StackStarve;
                    cursment.StackValue = stackwant;
                    Statements.Insert(0, cursment);
                    cursment = null;
                }
                else
                {
                    Console.WriteLine($"Failed to build statements for block {Name}: did not pop {stackwant} values!");
                }
            }

            foreach (var statement in Statements)
            {
                statement.DecompileToLispFull();
            }
        }

        private string GetNodeColor()
        {
            if (this is GOOLDecompBlockDoWhile) return "red";
            if (this is GOOLDecompBlockRegion) return "cyan";
            if (this is GOOLDecompBlockIf) return "deeppink";
            switch (Type)
            {
                default:
                    if (IsStackPop()) return "green2";
                    if (IsLetBegin()) return "cornflowerblue"; 
                    return "lightgray";
                case GoolBranchType.If: return "yellow";
                case GoolBranchType.Goto: return "lightblue";
                case GoolBranchType.Return: return "orange";
                case GoolBranchType.Continue: return "magenta";
                case GoolBranchType.ContinueIf: return "pink";
                case GoolBranchType.Break: return "purple";
                case GoolBranchType.BreakIf: return "indigo";
                case GoolBranchType.StackPop: return "green1";
            }
        }

        public string PrintRecursiveAsRoot()
        {
            string debug = "";
            VisitForward(preVisit: (block) =>
            {
                debug += $"  {block.Name} [ fillcolor={block.GetNodeColor()} label=\"{block.GetFullName()}\\n{block.DomID} | {block.PostOrderID}";
                foreach (var ins in block.Instructions)
                {
                    //debug += string.Format("{0,-6} {1,-28}\\n", ins.GetName(), ins.Arguments);
                }
                debug += $"\" ];\n";
                foreach (var next in block.next)
                {
                    debug += $"  {block.Name} -> {next.Name} [color={(next.begin < block.end ? "red" : "black")}]\n";
                }
                if (block.ImmPostDom != null)
                {
                    //debug += $"  {block.ImmPostDom.Name} -> {block.Name} [color=green]\n";
                }
                if (block.ImmDom != null)
                {
                    debug += $"  {block.ImmDom.Name} -> {block.Name} [color=orange]\n";
                }
            });
            return debug;
        }

        public virtual void PrintLispOut(int indent, ref string fout)
        {
            var istr = new string(' ', indent);
            foreach (var stmt in Statements)
            {
                fout += istr + stmt.LispOut.Print() + "\n";
            }
            // assumes no infinite loop lol.
            foreach (var n in next)
            {
                n.PrintLispOut(indent, ref fout);
            }
        }
    }

    public class GOOLDecompBlockIf : GOOLDecompBlock, IGOOLDecompBlockIterator
    {
        // the branch condition
        public GOOLStatement Condition { get; }

        public GOOLDecompBlock Header { get; }
        public GOOLDecompBlock TrueCase { get; }
        public GOOLDecompBlock? ElseCase { get; }

        public List<GOOLDecompBlock> BlockList { get; } = new();

        public GOOLDecompBlockIf(string name, GOOLDecompBlock header, GOOLDecompBlock follow, GOOLDecompBlock true_case, GOOLDecompBlock? else_case) : base(name)
        {
            Header = header;
            TrueCase = true_case;
            ElseCase = else_case;

            begin = header.begin;
            end = follow.begin;

            Condition = Header.Statements.Last();
            Header.Statements.RemoveLast();

            var true_tail = follow.prev.Find(TrueCase.Dominates)!;
            true_tail.next.Clear();
            if (ElseCase != null)
            {
                var else_tail = follow.prev.Find(ElseCase.Dominates)!;
                else_tail.next.Clear();
                true_tail.Statements.RemoveLast();
            }
            else
            {
                Header.next.Remove(follow);
            }

            foreach (var p in Header.prev)
            {
                for (int i = 0; i < p.next.Count; ++i)
                {
                    if (p.next[i] == Header) p.next[i] = this;
                }
            }
            next.Add(follow);
        }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Header);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public void StructureLets(GOOLDecompiler decompiler)
        {
            throw new NotImplementedException();
        }

        public void StructureLoops(GOOLDecompiler decompiler)
        {
            throw new NotImplementedException();
        }

        public override void PrintLispOut(int indent, ref string fout)
        {
            var istr = new string(' ', indent);
            var stmts = Header.Statements;
            for (int i = 0; i < stmts.Count - 1; ++i)
            {
                var stmt = stmts[i];
                fout += istr + stmt.LispOut.Print() + "\n";
            }
            var stmtif = Condition.LispOut as ListObj;
            if (ElseCase == null)
            {
                if (stmtif != null && stmtif.Forms.Count == 3)
                {
                    var cond = stmtif.Forms[2];
                    if (stmtif.Forms[0].Print() == "b-unless")
                    {
                        fout += istr + "(when " + cond.Print() + "\n";
                        TrueCase.PrintLispOut(indent + 2, ref fout);
                        fout += istr + "  )\n";
                    }
                    else if (stmtif.Forms[0].Print() == "b-if")
                    {
                        fout += istr + "(unless " + cond.Print() + "\n";
                        TrueCase.PrintLispOut(indent + 2, ref fout);
                        fout += istr + "  )\n";
                    }
                }
            }
            else
            {
                if (stmtif != null && stmtif.Forms.Count == 3)
                {
                    var cond = stmtif.Forms[2];
                    if (stmtif.Forms[0].Print() == "b-if")
                    {
                        cond = new ListObj(new TokenObj("not"), cond);
                    }
                    fout += istr + "(cond\n";
                    fout += istr + "  (" + cond.Print() + "\n";
                    TrueCase.PrintLispOut(indent + 3, ref fout);
                    fout += istr + "   )\n";
                    fout += istr + "  (else\n";
                    ElseCase.PrintLispOut(indent + 3, ref fout);
                    fout += istr + "   )\n";
                    fout += istr + "  )\n";
                }
            }
            // assumes no infinite loop lol.
            foreach (var n in next)
            {
                n.PrintLispOut(indent, ref fout);
            }
        }
    }

    public class GOOLDecompBlockDoWhile : GOOLDecompBlock, IGOOLDecompBlockIterator
    {
        // the target of (continue)
        public GOOLDecompBlock? Continue { get; }
        // the target of (break)
        public GOOLDecompBlock Break => ImmPostDom;
        // the branch condition
        public GOOLStatement Condition { get; }
        public bool PreTested => PreBranch != null;
        public GOOLDecompBlock? PreBranch { get; }
        public GOOLDecompBlock Header { get; }
        public GOOLDecompBlock Tail { get; }
        public GOOLDecompBlock Entry { get; }
        public GOOLDecompBlock Exit { get; }
        public List<GOOLDecompBlock> BlockList { get; } = new();

        public GOOLDecompBlockDoWhile(string name, GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock? prebranch) : base(name)
        {
            Entry = new GOOLDecompBlock(name + "_entry");
            Exit = new GOOLDecompBlock(name + "_exit");

            Header = header;
            Tail = tail;
            PreBranch = prebranch;
            Condition = tail.Statements[^1];
            tail.Statements.RemoveLast();

            var entry_block = prebranch ?? header;
            begin = entry_block.begin;
            end = tail.end;

            // if there are statements in the tail that aren't just the branch condition, we did not generate a block for a continue statement, so there must not be one!
            // however, if there is a prebranch, we always generate a continue to fix the CFG
            if (prebranch != null)
            {
                Continue = new GOOLDecompBlock(name + "_cont");
                Continue.begin = tail.begin;
                Continue.end = tail.begin;
                Continue.next.Add(tail);
                foreach (var p in tail.prev)
                {
                    if (p == prebranch) continue;
                    for (int i = 0; i < p.next.Count; ++i)
                    {
                        if (p.next[i] == tail) p.next[i] = Continue;
                    }
                }
            }
            else if (tail.Statements.Count == 0)
            {
                Continue = tail;
            }
            else
            {
                Continue = null;
            }

            // we set up the branch successors consistently, so 1 is always the branchless one
            next.Add(tail.next[1]);
            foreach (var p in entry_block.prev)
            {
                if (entry_block.Dominates(p)) continue;
                for (int i = 0; i < p.next.Count; ++i)
                {
                    if (p.next[i] == entry_block) p.next[i] = this;
                }
            }

            Entry.begin = begin;
            Entry.end = begin;
            Exit.begin = end;
            Exit.end = end;
            Entry.next.Add(entry_block);
            tail.next[1] = Exit;
        }

        public void StructureBreakContinue()
        {
            // we do not use Entry here because we never want to check the prebranch
            Header.VisitForward((block) =>
            {
                if (block == Continue || block == Break || block.Type == GoolBranchType.None)
                    return;
                if (block.next.Contains(Break))
                {
                    Console.WriteLine("break detected");
                    block.next.Remove(Break);
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.BreakIf : GoolBranchType.Break;
                }
                else if (Continue != null && block.end != Continue.begin && block.next.Contains(Continue)) // make sure it's not from fallthrough (no branch)
                {
                    if (block.Type == GoolBranchType.If) block.next.Remove(Continue);
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.ContinueIf : GoolBranchType.Continue;
                }
            });
        }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Entry);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public void StructureLets(GOOLDecompiler decompiler)
        {
            throw new NotImplementedException();
        }

        public void StructureLoops(GOOLDecompiler decompiler)
        {
            throw new NotImplementedException();
        }

        public override void PrintLispOut(int indent, ref string fout)
        {
            var istr = new string(' ', indent);
            if (PreTested)
            {
                var stmts = PreBranch!.Statements;
                for (int i = 0; i < stmts.Count - 1; ++i)
                {
                    var stmt = stmts[i];
                    fout += istr + stmt.LispOut.Print() + "\n";
                }
            }

            var cond = (Continue?.Type == GoolBranchType.Goto) ? new TokenObj(PreTested ? "t" : "nil") : Condition.LispOut;
            if (cond is ListObj lcond)
            {
                var branchop = lcond.Forms[0].Print();
                if (branchop == "b")
                {
                    cond = new TokenObj(PreTested ? "t" : "nil");
                }
                else
                {
                    bool addnot = (branchop == "b-if") ^ PreTested;
                    cond = lcond.Forms[2];
                    if (addnot)
                    {
                        cond = new ListObj(new TokenObj("not"), cond);
                    }
                }
            }
            fout += istr + "(" + (PreTested ? "while" : "until") + " " + cond.Print() + "\n";
            Header.PrintLispOut(indent + 2, ref fout);
            fout += istr + "  )\n";

            // assumes no infinite loop lol.
            foreach (var n in next)
            {
                n.PrintLispOut(indent, ref fout);
            }
        }
    }

    public class GOOLDecompBlockRegion : GOOLDecompBlock, IGOOLDecompBlockIterator
    {
        public GOOLDecompBlock Entry { get; }
        public GOOLDecompBlock Exit { get; }
        public List<GOOLDecompBlock> BlockList { get; } = new();

        public GOOLDecompBlockRegion(string name, GOOLDecompBlock entry, GOOLDecompBlock exit) : base(name)
        {
            Entry = entry;
            Exit = exit;
            begin = entry.begin;
            end = exit.end;
            next.AddRange(Exit.next);
            // disconnect entry and exit node from outside region. patch things to connect to the region instead!
            foreach (var p in Entry.prev)
            {
                for (int i = 0; i < p.next.Count; ++i)
                {
                    if (p.next[i] == Entry) p.next[i] = this;
                }
            }
            Exit.next.Clear();
        }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Entry);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public void StructureLets(GOOLDecompiler decompiler)
        {
            var polist = (this as IGOOLDecompBlockIterator).AsPostOrderList();
            polist.Remove(Entry);
            polist.Remove(Exit);
            (this as IGOOLDecompBlockIterator).StructureLetsInt(decompiler, polist);
        }

        public void StructureLoops(GOOLDecompiler decompiler)
        {
            var polist = (this as IGOOLDecompBlockIterator).AsPostOrderList();
            polist.Remove(Entry);
            polist.Remove(Exit);
            (this as IGOOLDecompBlockIterator).StructureLoopsInt(decompiler, polist);
        }
    }
}
