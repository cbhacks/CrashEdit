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
        BreakIf
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

        public int begin;
        public int end;

        public string name = name;

        public bool Dominates(GOOLDecompBlock other) => other.Dominators[DomID];
        public bool PostDominates(GOOLDecompBlock other) => other.PostDominators[DomID];
        public bool ImmediateDominates(GOOLDecompBlock other) => other.ImmDom == this;
        public bool ImmediatePostDominates(GOOLDecompBlock other) => other.ImmPostDom == this;

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

        public void GenerateStatements()
        {
            Statements.Clear();
            GOOLStatement cursment = null;
            int stackwant = 0;
            for (int i = Instructions.Count - 1; i >= 0; --i)
            {
                var ins = Instructions[i];
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
                    Statements.Insert(0, cursment);
                    cursment = null;
                }
            }

            if (cursment != null)
            {
                Console.WriteLine($"Failed to build statements for block {name}: did not pop {stackwant} values!");
            }

            foreach (var statement in Statements)
            {
                statement.DecompileToLispFull();
            }
        }

        public void SpliceNodeInterval(GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock preheader, GOOLDecompBlock posttail)
        {
            // disconnect the inner nodes
            if (header != preheader)
                tail.prev.Remove(preheader); // this is the first pre-test in a while loop
            tail.next.Remove(header); // tail -> header
            header.prev.Remove(tail); // header -> tail

            posttail.prev.Remove(tail); // break -> tail
            tail.next.Remove(posttail); // tail -> break

            // connect the interval itself
            next.Add(posttail);
            posttail.prev.Add(this);

            prev.AddRange(preheader.prev);
            foreach (var prev in preheader.prev)
            {
                prev.next.Remove(preheader); // also disconnects nodes that lead into loop header
                prev.next.Add(this);
            }
            preheader.prev.Clear();
        }

        private string GetNodeColor()
        {
            if (this is GOOLDecompBlockDoWhile) return "red";
            switch (Type)
            {
                default: return "lightgray";
                case GoolBranchType.If: return "yellow";
                case GoolBranchType.Goto: return "lightblue";
                case GoolBranchType.Return: return "orange";
                case GoolBranchType.Continue: return "magenta";
                case GoolBranchType.ContinueIf: return "pink";
                case GoolBranchType.Break: return "purple";
                case GoolBranchType.BreakIf: return "indigo";
            }
        }

        public string PrintRecursiveAsRoot()
        {
            string debug = "";
            VisitForward(preVisit: (block) =>
            {
                debug += $"  {block.name} [ fillcolor={block.GetNodeColor()} label=\"{block.name}\\n";
                foreach (var ins in block.Instructions)
                {
                    //debug += string.Format("{0,-6} {1,-28}\\n", ins.GetName(), ins.Arguments);
                }
                debug += $"\" ];\n";
                foreach (var next in block.next)
                {
                    debug += $"  {block.name} -> {next.name} [color={(next.begin <= block.begin ? "red" : "black")}]\n";
                }
                return;
                if (block.ImmPostDom != null)
                {
                    debug += $"  {block.ImmPostDom.name} -> {block.name} [color=green]\n";
                }
                if (block.ImmDom != null)
                {
                    debug += $"  {block.ImmDom.name} -> {block.name} [color=orange]\n";
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

    public class GOOLDecompBlockContainer : GOOLDecompBlock, IGOOLDecompBlockIterator
    {
        public GOOLDecompBlock Header { get; }
        public GOOLDecompBlock Tail { get; }
        public List<GOOLDecompBlock> BlockList { get; } = new();

        public GOOLDecompBlockContainer(string name, GOOLDecompBlock header, GOOLDecompBlock tail, GOOLDecompBlock posttail) : base(name)
        {
            Header = header;
            Tail = tail;

            SpliceNodeInterval(header, tail, header, posttail);
        }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Header);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public override void PrintLispOut(int indent, ref string fout)
        {
            Header.PrintLispOut(indent, ref fout);
        }
    }

    public class GOOLDecompBlockIf : GOOLDecompBlock
    {
        new public List<GOOLInstruction> Instructions => Header.Instructions;
        new public List<GOOLStatement> Statements => Header.Statements;

        public bool HasElse => Clauses.Length == 2;

        public GOOLDecompBlock Header { get; }
        public GOOLDecompBlock[] Clauses { get; }

        public GOOLDecompBlockIf(string name, GOOLDecompBlock header, GOOLDecompBlock follow, params GOOLDecompBlock[] clauses) : base(name)
        {
            Header = header;

            Clauses = clauses;

            Header.next.Clear();
            foreach (GOOLDecompBlockContainer clause in clauses)
            {
                if (clause.next.Count == 1 && clause.next[0] == follow && clause.Tail.Statements.Count > 0 && clause.Tail.Statements.Last().LispOut.Print().StartsWith("(b "))
                {
                    clause.Tail.Statements.RemoveLast();
                }
                Header.next.Add(clause);
                follow.prev.Remove(clause);
                clause.next.Clear();
                clause.prev.Clear();
            }
            follow.prev.Remove(Header);
            follow.prev.Add(this);
            next.Add(follow);
            foreach (var p in Header.prev)
            {
                p.next.Remove(Header);
                p.next.Add(this);
                prev.Add(p);
            }
            Header.prev.Clear();
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
            var stmtif = stmts[^1].LispOut as ListObj;
            if (!HasElse)
            {
                if (stmtif != null && stmtif.Forms.Count == 3)
                {
                    var cond = stmtif.Forms[2];
                    if (stmtif.Forms[0].Print() == "b-unless")
                    {
                        fout += istr + "(when " + cond.Print() + "\n";
                        Clauses[0].PrintLispOut(indent + 2, ref fout);
                        fout += istr + "  )\n";
                    }
                    else if (stmtif.Forms[0].Print() == "b-if")
                    {
                        fout += istr + "(unless " + cond.Print() + "\n";
                        Clauses[0].PrintLispOut(indent + 2, ref fout);
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
                    Clauses[0].PrintLispOut(indent + 3, ref fout);
                    fout += istr + "   )\n";
                    fout += istr + "  (else\n";
                    Clauses[1].PrintLispOut(indent + 3, ref fout);
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
        public GOOLDecompLoop Loop { get; set; }
        // the target of (continue)
        public GOOLDecompBlock? Continue { get; set; }
        // the target of (break)
        public GOOLDecompBlock Break { get; set; }
        // the branch condition
        public GOOLStatement Condition { get; set; }
        public bool PreTested => PreBranch != null;
        public GOOLDecompBlock? PreBranch { get; }
        public GOOLDecompBlock Header => Loop.Header;
        public GOOLDecompBlock Tail => Loop.Tail;
        public List<GOOLDecompBlock> BlockList => Loop.BlockList;

        public GOOLDecompBlockDoWhile(string name, GOOLDecompLoop loop, GOOLDecompBlock cont) : base(name)
        {
            Loop = loop;
            Continue = cont;
            Break = cont.ImmPostDom;
            Condition = cont.Statements[^1];
            PreBranch = loop.PreBranch;
            cont.Statements.RemoveLast();

            begin = Header.begin;
            end = Tail.end;

            if (Continue.Statements.Count != 0)
            {
                // we did not generate a block for a continue statement, so there must not be one!
                Continue = null;
            }

            // note: you MUST regenerate the CFG and domination trees after this!
            if (loop.PreBranch == null)
            {
                SpliceNodeInterval(Header, Tail, Header, Break);
            }
            else
            {
                SpliceNodeInterval(Header, Tail, loop.PreBranch, Break);
                // we're gonna keep the prebranch for now because it may contain real code before the branch
                // in the future we kick it out of the interval and turn it into a suspiciously branchless block.
                // that way it can be output as 'just code' before the loop!
            }
        }

        public void StructureBreakContinue()
        {
            Header.VisitForward((block) =>
            {
                if (block == Continue || block == Break || block.Type == GoolBranchType.None)
                    return;
                if (block.next.Contains(Break))
                {
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.BreakIf : GoolBranchType.Break;
                }
                else if (Continue != null && block.end != Continue.begin && block.next.Contains(Continue)) // make sure it's not from fallthrough (no branch)
                {
                    block.Type = block.Type == GoolBranchType.If ? GoolBranchType.ContinueIf : GoolBranchType.Continue;
                }
            });
        }

        public void GenerateCFG()
        {
            (this as IGOOLDecompBlockIterator).GenerateCFGInt(Header);
        }

        public void GenerateDominationTree()
        {
            (this as IGOOLDecompBlockIterator).GenerateDominationTreeInt();
        }

        public override void PrintLispOut(int indent, ref string fout)
        {
            var istr = new string(' ', indent);
            if (PreTested)
            {
                var stmts = Loop.PreBranch.Statements;
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
}
