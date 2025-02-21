using System;
using System.Windows.Documents;

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
        public List<GOOLInstruction> instructions = new();
        public List<GOOLStatement> statements = new();

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
            statements.Clear();
            GOOLStatement cursment = null;
            int stackwant = 0;
            for (int i = instructions.Count - 1; i >= 0; --i)
            {
                var ins = instructions[i];
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
                    statements.Insert(0, cursment);
                    cursment = null;
                }
            }

            if (cursment != null)
            {
                Console.WriteLine($"Failed to build statements for block {name}: did not pop {stackwant} values!");
            }

            foreach (var statement in statements)
            {
                statement.DecompileToLispFull();
                string tempy = statement.LispOut.Print();
                Console.WriteLine(tempy);
            }
        }

        public void GenerateCFGAsRoot(List<GOOLDecompBlock> blocks)
        {
            // generate function block list
            blocks.Clear();
            int poid = 0;
            VisitForward(preVisit: (block) =>
            {
                block.DomID = blocks.Count;
                blocks.Add(block);
            }, postVisit: (block) =>
            {
                block.PostOrderID = poid++;
            });
            blocks.Sort((a, b) => a.begin - b.begin);
            // initialize dominators and postdominators
            blocks.ForEach((block) =>
            {
                block.Dominators = new(blocks.Count);
                if (block == this)
                {
                    // this is the entry node
                    block.Dominators.ClearAll();
                    block.Dominators.Set(block.DomID);
                }
                else
                {
                    block.Dominators.SetAll();
                }
                block.PostDominators = new(blocks.Count);
                if (block.next.Count == 0)
                {
                    // this is an exit node since it cannot go any further
                    block.PostDominators.ClearAll();
                    block.PostDominators.Set(block.DomID);
                }
                else
                {
                    block.PostDominators.SetAll();
                }
            });
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
                debug += $"  {block.name} [ fillcolor={block.GetNodeColor()} label=\"{block.PostOrderID}\\n";
                foreach (var ins in block.instructions)
                {
                    //debug += string.Format("{0,-6} {1,-28}\\n", ins.GetName(), ins.Arguments);
                }
                debug += $"\" ];\n";
                foreach (var next in block.next)
                {
                    debug += $"  {block.name} -> {next.name} [color={(next.begin <= block.begin ? "red" : "black")}]\n";
                }
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
    }

    public class GOOLDecompBlockDoWhile : GOOLDecompBlock
    {
        public GOOLDecompLoop Loop { get; set; }
        // the target of (continue)
        public GOOLDecompBlock? Continue { get; set; }
        // the target of (break)
        public GOOLDecompBlock Break { get; set; }
        // the branch condition
        public GOOLStatement Condition { get; set; }
        public bool PreTested { get; }
        public GOOLDecompBlock Header => Loop.Header;
        public GOOLDecompBlock Tail => Loop.Tail;

        public GOOLDecompBlockDoWhile(string name, GOOLDecompLoop loop, GOOLDecompBlock cont) : base(name)
        {
            Loop = loop;
            Continue = cont;
            Break = cont.ImmPostDom;
            Condition = cont.statements[^1];
            PreTested = loop.PreBranch != null;

            begin = Header.begin;
            end = Tail.end;

            if (Continue.statements.Count != 1)
            {
                // we did not generate a block for a continue statement, so there must not be one!
                Continue = null;
            }

            if (loop.PreBranch == null)
            {
                // disconnect the inner nodes
                Tail.next.Remove(Header); // tail -> header
                Header.prev.Remove(Tail); // header -> tail

                Break.prev.Remove(Tail); // break -> tail
                Tail.next.Remove(Break); // tail -> break

                // connect the interval itself
                next.Add(Break);
                Break.prev.Add(this);

                prev.AddRange(Header.prev);
                foreach (var prev in Header.prev)
                {
                    prev.next.Remove(Header); // also disconnects nodes that lead into loop header
                    prev.next.Add(this);
                }
                Header.prev.Clear();
            }
            else
            {
                // disconnect the inner nodes
                Tail.next.Remove(Header); // tail -> header
                Header.prev.Remove(Tail); // header -> tail

                Break.prev.Remove(Tail); // break -> tail
                Tail.next.Remove(Break); // tail -> break

                // connect the interval itself
                next.Add(Break);
                Break.prev.Add(this);

                prev.AddRange(loop.PreBranch.prev);
                foreach (var prev in loop.PreBranch.prev)
                {
                    prev.next.Remove(loop.PreBranch); // also disconnects nodes that lead into loop header
                    prev.next.Add(this);
                }
                loop.PreBranch.prev.Clear();
                // we're gonna keep the prebranch for now because it may contain real code before the branch
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

        public void GenerateDominationTree()
        {
            GOOLDecompiler.GenerateDominationTree(Loop.BlockList);
            foreach (var block in Loop.BlockList)
            {
                if (block is GOOLDecompBlockDoWhile dw)
                    dw.GenerateDominationTree();
            }
        }
    }
}
