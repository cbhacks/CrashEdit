using System;
using System.Xml.Linq;

namespace CrashEdit.Crash
{
    public abstract class GObj
    {
        public abstract string Print();
    }

    public sealed class ListObj : GObj
    {
        public List<GObj> Forms { get; set; }

        public ListObj(params GObj[] forms)
        {
            Forms = new(forms);
        }

        public override string Print()
        {
            if (Forms.Count == 0)
            {
                return "()";
            }
            else
            {
                string res = "(";
                bool first = true;
                foreach (GObj obj in Forms)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res += " ";
                    }
                    res += obj.Print();
                }
                return res + ")";
            }
        }
    }

    public sealed class TokenObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return Value;
        }
    }

    public sealed class SymbolObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return "'" + Value;
        }
    }

    public sealed class StringObj(string val) : GObj
    {
        public string Value { get; set; } = val;

        public override string Print()
        {
            return '"' + Value + '"';
        }
    }

    public sealed class NumberObj(int val) : GObj
    {
        public int Value { get; set; } = val;

        public override string Print()
        {
            return Value.TransformedStringLisp();
        }
    }

    public class GOOLStatement
    {
        public List<GOOLInstruction> Instructions { get; } = [];
        public GObj? LispOut { get; private set; } = null;

        public GObj DecompileSingleInsToLisp(ref int i)
        {
            return Instructions[i].DecompileToLisp(this, ref i);
        }

        public void DecompileToLispFull()
        {
            int i = 0;
            LispOut = DecompileSingleInsToLisp(ref i);
        }
    }

    public class GOOLDecompBlock(string name)
    {
        public List<GOOLInstruction> instructions = new();
        public List<GOOLStatement> statements = new();

        public List<GOOLDecompBlock> prev = new();
        public List<GOOLDecompBlock> next = new();

        public enum BranchType
        {
            None,
            Goto,
            If,
            Return
        }
        public BranchType Type { get; set; }

        public int DomID = -1;
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
    }

    public class GOOLDecompDomVector : List<bool>
    {
        public GOOLDecompDomVector(int size) : base(new bool[size]) {
        }

        public void Set(int i)
        {
            this[i] = true;
        }

        public void Clear(int i)
        {
            this[i] = false;
        }

        public void SetAll()
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i] = true;
            }
        }

        public void ClearAll()
        {
            for (int i = 0; i < Count; ++i)
            {
                this[i] = false;
            }
        }

        public bool Equal(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return false;
            for (int i = 0; i < Count; ++i)
            {
                if (this[i] != other[i]) return false;
            }
            return true;
        }

        public void Merge(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] |= other[i];
            }
        }

        public void Mask(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] &= other[i];
            }
        }

        public void Overwrite(GOOLDecompDomVector other)
        {
            if (Count != other.Count) return;
            for (int i = 0; i < Count; ++i)
            {
                this[i] = other[i];
            }
        }
    }

    public class GOOLDecompLoop(GOOLDecompBlock header, GOOLDecompBlock tail)
    {
        // (while...) blocks are pre-tested by having an unconditional branch straight to the test
        public GOOLDecompBlock? PreBranch { get; set; } = null;
        public GOOLDecompBlock Header { get; set; } = header;
        public GOOLDecompBlock Tail { get; set; } = tail;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public int LoopDepth { get; set; } = -1;
        public GOOLDecompLoop Parent { get; set; } = null;
        public List<GOOLDecompLoop> Children { get; } = new();
    }

    public class GOOLDecompFunctionInfo(string name)
    {
        public int Offset { get; set; }
        public bool Trans { get; set; }
        public string Name { get; set; } = name;
        public List<GOOLDecompBlock> BlockList { get; } = new();
        public List<GOOLDecompLoop> LoopList { get; } = new();

        public GOOLDecompBlock start;

        public void GenerateCFG()
        {
            // generate function block list
            BlockList.Clear();
            start.VisitForward(preVisit: (block) =>
            {
                block.DomID = BlockList.Count;
                BlockList.Add(block);
            });
            BlockList.Sort((a, b) => a.begin - b.begin);
            // initialize dominators and postdominators
            BlockList.ForEach((block) =>
            {
                block.Dominators = new(BlockList.Count);
                if (block == start)
                {
                    // this is the entry node
                    block.Dominators.ClearAll();
                    block.Dominators.Set(block.DomID);
                }
                else
                {
                    block.Dominators.SetAll();
                }
                block.PostDominators = new(BlockList.Count);
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

            if (Continue.statements.Count != 1)
            {
                // we did not generate a block for a continue statement, so there must not be one!
                Continue = null;
            }
        }
    }

    public class GOOLDecompiler
    {
    }
}
