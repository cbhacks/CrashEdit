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

    public class GOOLDecompBlock(string name, int begin, int end)
    {
        public List<GOOLInstruction> Instructions { get; } = new();
        public List<GOOLStatement> Statements { get; } = new();

        public List<GOOLDecompBlock> Prev { get; } = new();
        public List<GOOLDecompBlock> Next { get; } = new();

        public GoolBranchType Type { get; set; }

        public int DomID { get; set; } = -1;
        public int PostOrderID { get; set; } = -1;
        public GOOLDecompDomVector Dominators { get; set; } = null;
        public GOOLDecompDomVector PostDominators { get; set; } = null;
        public GOOLDecompBlock ImmDom { get; set; } = null;
        public GOOLDecompBlock ImmPostDom { get; set; } = null;

        public int StackPop { get; set; } = -1;

        public int OfsBegin { get; set; } = begin;
        public int OfsEnd { get; set; } = end;

        public string Name { get; private set; } = name;

        public string GetFullName() => Name + $" ({OfsBegin} ~ {OfsEnd})";

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

            foreach (var block in Next)
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

        public virtual string GetNodeColor()
        {
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

        public virtual void PrintForGraph(ref string res)
        {
            res += $"  {Name} [ fillcolor={GetNodeColor()} label=\"{GetFullName()}\\n{DomID} | {PostOrderID}\" ];\n";
            foreach (var next in Next)
            {
                res += $"  {Name} -> {next.Name} [color={(next.OfsBegin < OfsEnd ? "red" : "black")}]\n";
            }
            if (ImmPostDom != null)
            {
                res += $"  {ImmPostDom.Name} -> {Name} [color=green]\n";
            }
            if (ImmDom != null)
            {
                res += $"  {ImmDom.Name} -> {Name} [color=orange]\n";
            }
        }

        public string PrintRecursiveAsRoot()
        {
            string res = "";
            VisitForward(preVisit: (block) => block.PrintForGraph(ref res));
            return res;
        }

        public virtual void PrintLispOut(int indent, ref string fout, HashSet<GOOLDecompBlock> visited)
        {
            visited.Add(this);
            var istr = new string(' ', indent);
            foreach (var stmt in Statements)
            {
                fout += istr + stmt.LispOut.Print() + "\n";
            }

            foreach (var n in Next)
            {
                if (!visited.Contains(n)) n.PrintLispOut(indent, ref fout, visited);
            }
        }
    }

    public abstract class GOOLDecompBlockSubGraph(string name, int begin, int end) : GOOLDecompBlock(name, begin, end), IGOOLDecompBlockIterator
    {
        public GOOLDecompBlock Entry { get; protected set; }
        public List<GOOLDecompBlock> BlockList { get; } = new();

        public virtual void PatchStructuredIf(Dictionary<GOOLDecompBlock, GOOLDecompBlock> processed_ifs) { }

        public virtual void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Entry);
        }

        public virtual void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public virtual void StructureLets()
        {
            throw new NotImplementedException();
        }

        public virtual void StructureLoops()
        {
            throw new NotImplementedException();
        }
    }

    public class GOOLDecompBlockIf : GOOLDecompBlockSubGraph
    {
        // the branch condition
        public GOOLStatement Condition { get; }

        public GOOLDecompBlock TrueCase { get; }
        public GOOLDecompBlock? ElseCase { get; }

        public GOOLDecompBlockIf(string name, GOOLDecompBlock entry, GOOLDecompBlock follow, GOOLDecompBlock true_case, GOOLDecompBlock? else_case) : base(name, entry.OfsBegin, follow.OfsBegin)
        {
            Entry = entry;
            TrueCase = true_case;
            ElseCase = else_case;

            Condition = Entry.Statements.Last();
            Entry.Statements.RemoveLast();

            var true_tail = follow.Prev.Find(TrueCase.Dominates)!;
            true_tail.Next.Clear();
            if (ElseCase != null)
            {
                var else_tail = follow.Prev.Find(ElseCase.Dominates)!;
                else_tail.Next.Clear();
                true_tail.Statements.RemoveLast();
            }
            else
            {
                Entry.Next.Remove(follow);
            }

            foreach (var p in Entry.Prev)
            {
                for (int i = 0; i < p.Next.Count; ++i)
                {
                    if (p.Next[i] == Entry) p.Next[i] = this;
                }
            }
            Next.Add(follow);
        }

        public override string GetNodeColor() => "deeppink";

        public override void PrintForGraph(ref string res)
        {
            base.PrintForGraph(ref res);
            res += $"  subgraph cluster_{Name} {{\n";
            res += $"    label = \"{GetFullName()}\";\n";
            res += $"    style = filled;\n";
            res += $"    fontsize = \"25pt\";\n";
            res += $"    fillcolor = indianred;\n";
            res += Entry.PrintRecursiveAsRoot();
            res += $"  }}\n";
        }

        public override void PrintLispOut(int indent, ref string fout, HashSet<GOOLDecompBlock> visited)
        {
            visited.Add(this);

            var istr = new string(' ', indent);
            var stmts = Entry.Statements;
            for (int i = 0; i < stmts.Count; ++i)
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
                        TrueCase.PrintLispOut(indent + 2, ref fout, visited);
                        fout += istr + "  )\n";
                    }
                    else if (stmtif.Forms[0].Print() == "b-if")
                    {
                        fout += istr + "(unless " + cond.Print() + "\n";
                        TrueCase.PrintLispOut(indent + 2, ref fout, visited);
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
                    TrueCase.PrintLispOut(indent + 3, ref fout, visited);
                    fout += istr + "   )\n";
                    fout += istr + "  (else\n";
                    ElseCase.PrintLispOut(indent + 3, ref fout, visited);
                    fout += istr + "   )\n";
                    fout += istr + "  )\n";
                }
            }

            foreach (var n in Next)
            {
                if (!visited.Contains(n)) n.PrintLispOut(indent, ref fout, visited);
            }
        }
    }

    public class GOOLDecompBlockDoWhile : GOOLDecompBlockSubGraph
    {
        // the target of (continue)
        public GOOLDecompBlock? Continue { get; }
        // the target of (break)
        public GOOLDecompBlock Break => ImmPostDom;
        // the branch condition
        public GOOLStatement Condition { get; }
        public bool PreTested => PreBranch != null;
        public GOOLDecompBlock? PreBranch { get; }
        public GOOLDecompBlock Header { get; private set; }
        public GOOLDecompBlock Tail { get; }
        public GOOLDecompBlock Exit { get; }

        public GOOLDecompBlockDoWhile(string name, GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock? prebranch) : base(name, (prebranch ?? header).OfsBegin, tail.OfsEnd)
        {
            Entry = new GOOLDecompBlock(name + "_entry", OfsBegin, OfsBegin);
            Exit = new GOOLDecompBlock(name + "_exit", OfsEnd, OfsEnd);

            Header = header;
            Tail = tail;
            PreBranch = prebranch;
            Condition = tail.Statements[^1];
            tail.Statements.RemoveLast();

            var entry_block = prebranch ?? header;

            // if there are statements in the tail that aren't just the branch condition, we did not generate a block for a continue statement, so there must not be one!
            // however, if there is a prebranch, we always generate a continue to fix the CFG
            if (prebranch != null)
            {
                Continue = new(name + "_cont", tail.OfsBegin, tail.OfsBegin);
                Continue.Next.Add(tail);
                foreach (var p in tail.Prev)
                {
                    if (p == prebranch) continue;
                    for (int i = 0; i < p.Next.Count; ++i)
                    {
                        if (p.Next[i] == tail) p.Next[i] = Continue;
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
            Next.Add(tail.Next[1]);
            foreach (var p in entry_block.Prev)
            {
                if (entry_block.Dominates(p)) continue;
                for (int i = 0; i < p.Next.Count; ++i)
                {
                    if (p.Next[i] == entry_block) p.Next[i] = this;
                }
            }

            Entry.Next.Add(entry_block);
            tail.Next[1] = Exit;
        }

        public void StructureBreakContinue()
        {
            // we do not use Entry here because we never want to check the prebranch
            Header.VisitForward((block) =>
            {
                if (block == Continue || block == Break || block.Type == GoolBranchType.None)
                    return;
                if (block.Next.Contains(Break))
                {
                    Console.WriteLine("break detected");
                    block.Next.Remove(Break);
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.BreakIf : GoolBranchType.Break;
                }
                else if (Continue != null && block.OfsEnd != Continue.OfsBegin && block.Next.Contains(Continue)) // make sure it's not from fallthrough (no branch)
                {
                    if (block.Type == GoolBranchType.If) block.Next.Remove(Continue);
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.ContinueIf : GoolBranchType.Continue;
                }
            });
        }

        public override void PatchStructuredIf(Dictionary<GOOLDecompBlock, GOOLDecompBlock> processed_ifs)
        {
            if (processed_ifs.TryGetValue(Header, out var value))
            {
                Header = value;
            }
        }

        public override string GetNodeColor() => "red";

        public override void PrintForGraph(ref string res)
        {
            base.PrintForGraph(ref res);
            res += $"  subgraph cluster_{Name} {{\n";
            res += $"    label = \"{GetFullName()}\";\n";
            res += $"    style = filled;\n";
            res += $"    fontsize = \"25pt\";\n";
            res += $"    fillcolor = lightyellow;\n";
            res += Entry.PrintRecursiveAsRoot();
            res += $"  }}\n";
        }

        public override void PrintLispOut(int indent, ref string fout, HashSet<GOOLDecompBlock> visited)
        {
            visited.Add(this);

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
            Header.PrintLispOut(indent + 2, ref fout, visited);
            fout += istr + "  )\n";

            foreach (var n in Next)
            {
                if (!visited.Contains(n)) n.PrintLispOut(indent, ref fout, visited);
            }
        }
    }

    public class GOOLDecompBlockRegion : GOOLDecompBlockSubGraph
    {
        public GOOLDecompBlock Exit { get; }

        public GOOLDecompBlockRegion(string name, GOOLDecompBlock entry, GOOLDecompBlock exit) : base(name, entry.OfsBegin, exit.OfsEnd)
        {
            Entry = entry;
            Exit = exit;
            Next.AddRange(exit.Next);
            // disconnect entry and exit node from outside region. patch things to connect to the region instead!
            foreach (var p in entry.Prev)
            {
                for (int i = 0; i < p.Next.Count; ++i)
                {
                    if (p.Next[i] == entry) p.Next[i] = this;
                }
            }
            exit.Next.Clear();
        }

        public override string GetNodeColor() => "cyan";

        public override void PrintForGraph(ref string res)
        {
            base.PrintForGraph(ref res);
            res += $"  subgraph cluster_{Name} {{\n";
            res += $"    label = \"{GetFullName()}\";\n";
            res += $"    style = filled;\n";
            res += $"    fontsize = \"25pt\";\n";
            res += $"    fillcolor = cyan3;\n";
            res += Entry.PrintRecursiveAsRoot();
            res += $"  }}\n";
        }

        public override void StructureLets()
        {
            var polist = (this as IGOOLDecompBlockIterator).AsPostOrderList();
            polist.Remove(Entry);
            polist.Remove(Exit);
            (this as IGOOLDecompBlockIterator).StructureLetsInt(polist);
        }

        public override void StructureLoops()
        {
            var polist = (this as IGOOLDecompBlockIterator).AsPostOrderList();
            polist.Remove(Entry);
            polist.Remove(Exit);
            (this as IGOOLDecompBlockIterator).StructureLoopsInt(polist);
        }
    }
}
