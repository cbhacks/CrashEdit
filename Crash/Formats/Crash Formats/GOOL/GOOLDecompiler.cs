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

        public bool visited = false;

        public int DomID = -1;
        public GOOLDecompDomVector Dominators { get; set; } = null;
        public GOOLDecompDomVector PostDominators { get; set; } = null;
        public GOOLDecompBlock ImmDom { get; set; } = null;
        public GOOLDecompBlock ImmPostDom { get; set; } = null;

        public int begin;
        public int end;

        public string name = name;

        public void VisitForward(Action<GOOLDecompBlock>? preVisit = null, Action<GOOLDecompBlock>? postVisit = null)
        {
            visited = true;

            // pre-visit work
            preVisit?.Invoke(this);

            foreach (var block in next)
            {
                if (!block.visited) block.VisitForward(preVisit, postVisit);
            }

            // post-visit work
            postVisit?.Invoke(this);
        }

        public int EdgeCountToBlockBack(GOOLDecompBlock target, int edgeCount = 0)
        {
            visited = true;

            if (prev.Contains(target))
                return edgeCount+1;

            int lowestEdges = -1;
            foreach (var block in prev)
            {
                if (block.visited)
                    continue;
                int edges = block.EdgeCountToBlockBack(target, edgeCount+1);
                if (edges != -1)
                {
                    if (lowestEdges == -1 || edges < lowestEdges)
                        lowestEdges = edges;
                }
            }
            return lowestEdges;
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
    }

    public class GOOLDecompLoop(GOOLDecompBlock header, GOOLDecompBlock tail)
    {
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

        public void Initialize()
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

    public class GOOLDecompBlockDoWhile(string name, GOOLDecompLoop loop, GOOLDecompBlock cont) : GOOLDecompBlock(name)
    {
        public GOOLDecompLoop Loop { get; set; } = loop;
        // the target of (continue)
        public GOOLDecompBlock Continue { get; set; } = cont;
        // the branch condition
        public GOOLStatement Expression { get; set; } = cont.statements[^1];
        // the target of (break)
        public GOOLDecompBlock Break { get; set; } = cont.ImmPostDom;
    }

    public class GOOLDecompiler
    {
    }
}
