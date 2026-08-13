/*
 * Every single known GOOL instruction.
 * 
 */

namespace CrashEdit.Crash.GOOLIns
{
    public abstract class GOOLInsOpcode
    {
        protected static readonly string[] FlipComments = ["(force non-mirror)", "(force mirror)", "(mirror)", ""];
        protected static readonly string[] PadButtons = ["L2", "R2", "L1", "R1", "Triangle", "Circle", "X", "Square", "Select", "L3", "R3", "Start", "Up", "Right", "Down", "Left"];
        protected static readonly string[] PadDirs = ["Up", "Up+Right", "Right", "Down+Right", "Down", "Down+Left", "Left", "Up+Left", "None", "D-Pad Up", "D-Pad Right", "D-Pad Down", "D-Pad Left"];

        public abstract string GetName(GOOLInstruction ins);
        public abstract string GetFormat();
        public abstract string GetComment(GOOLInstruction ins);

        public virtual int StackPush(GOOLInstruction ins)
        {
            int amount = 0;
            foreach (var arg in ins.Args)
            {
                if (arg.Value.Type == GOOLArgumentTypes.DestRef && arg.Value.Value == GOOLInstruction.StackRef)
                {
                    amount++;
                }
            }
            return amount;
        }

        public virtual int StackPop(GOOLInstruction ins)
        {
            int amount = 0;
            foreach (var arg in ins.Args)
            {
                if (arg.Value.Type != GOOLArgumentTypes.DestRef)
                {
                    if (ins.IsStackRef(arg.Key))
                    {
                        amount++;
                    }
                    else if (ins.IsDoubleStackRef(arg.Key))
                    {
                        amount += 2;
                    }
                }
            }
            return amount;
        }

        public virtual int BranchPop(GOOLInstruction ins) => 0;

        public virtual GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            Console.WriteLine("DecompileToLisp unimplemented for " + ins.GetName() + " " + ins.Arguments + (ins.GetStackPush() > 0 ? " (fix immediately as this pushes to stack)" : ""));
            return new ListObj([new TokenObj("NYI-" + ins.GetName())]);
        }

        protected static GObj DecompileArgStandard(GOOLInstruction ins, char a, GOOLStatement statement, ref int i)
        {
            if (ins.IsStackRef(a))
            {
                i++;
                return statement.DecompileSingleInsToLisp(ref i);
            }
            else
            {
                return ins.ArgToLisp(a);
            }
        }

        protected static List<GObj> DecompileVarargs(int argc, GOOLStatement statement, ref int i)
        {
            List<GObj> args = [];
            for (var a = 0; a < argc;)
            {
                i++;
                var argins = statement.Instructions[i];
                args.Add(statement.DecompileSingleInsToLisp(ref i));
                a += argins.GetStackPush();
            }
            args.Reverse();
            return args;
        }
    }

    /// <summary>
    /// GOOL op that changes control flow within a procedure
    /// </summary>
    public abstract class GOOLInsBranch : GOOLInsOpcode
    {
        public override int BranchPop(GOOLInstruction ins) => ins.Args['V'].Value;
    }

    /// <summary>
    /// GOOL op that takes number of arguments in operand A
    /// </summary>
    public abstract class GOOLInsArgs : GOOLInsOpcode
    {
        public override int StackPop(GOOLInstruction ins) => base.StackPop(ins) + ins.Args['A'].Value;
    }

    /// <summary>
    /// GOOL op that takes two parameters (GOOL refs): L and R, and pushes the result to stack
    /// </summary>
    public abstract class GOOLInsLR : GOOLInsOpcode
    {
        public abstract string LispHead { get; }
        public virtual bool Mergeable => false;
        public override string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public override int StackPush(GOOLInstruction ins) => 1;

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var r = DecompileArgStandard(ins, 'R', statement, ref i);
            var l = DecompileArgStandard(ins, 'L', statement, ref i);
            if (Mergeable && r is ListObj ll && ll.Forms.Count > 0 && ll.Forms[0] is TokenObj llt && llt.Value == LispHead)
            {
                var res = new ListObj([new TokenObj(LispHead)]);
                res.Forms.AddRange(ll.Forms.GetRange(1, ll.Forms.Count - 1));
                res.Forms.Add(l);
                return res;
            }
            return new ListObj([new TokenObj(LispHead), l, r]);
        }
    }

    /// <summary>
    /// GOOL op that takes two parameters (GOOL refs): D and S, storing the result in D
    /// </summary>
    public abstract class GOOLInsDestCrash1 : GOOLInsOpcode
    {
        public abstract string LispHead { get; }
        public override string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public override int StackPush(GOOLInstruction ins) => ins.IsStackRef('D') ? 1 : 0;
        public override int StackPop(GOOLInstruction ins)
        {
            if (ins.IsStackRef('S'))
            {
                return 1;
            }
            else if (ins.IsDoubleStackRef('S'))
            {
                return 2;
            }
            return 0;
        }

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            if (statement.Type == GoolStatementType.LetEnd && ins.IsStackRef('S') && ins.GetName() == "SETF")
            {
                return new ListObj([new TokenObj("setf"), ins.ArgToLisp('D'), new TokenObj("stack-pop-temp")]);
            }
            GObj s = DecompileArgStandard(ins, 'S', statement, ref i);
            if (ins.IsStackRef('D'))
            {
                if (ins.GetName() == "SETF")
                    return s;
                else
                    return new ListObj([new TokenObj(LispHead), s]);
            }
            else
            {
                if (ins.GetName() == "SETF")
                    return new ListObj([new TokenObj("setf"), ins.ArgToLisp('D'), s]);
                else
                    return new ListObj([new TokenObj("setf"), ins.ArgToLisp('D'), new ListObj([new TokenObj(LispHead), s])]);
            }
        }
    }

    /// <summary>
    /// GOOL op that takes two parameters (GOOL refs): D and S, storing the result in D
    /// Crash 2 version that handles the dest ref specifically for that purpose
    /// </summary>
    public abstract class GOOLInsDestCrash2 : GOOLInsDestCrash1
    {
        public override string GetFormat() => GOOLInstruction.DefaultFormatDS2;
    }



    [GOOLInstruction(0, GameVersion.Crash1)]
    [GOOLInstruction(0, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(0, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(0, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(0, GameVersion.Crash2)]
    [GOOLInstruction(0, GameVersion.Crash3)]
    public sealed class Add : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "+";
        public override string GetName(GOOLInstruction ins) => "ADD";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} + {ins.GetArg('R')}";
    }

    [GOOLInstruction(1, GameVersion.Crash1)]
    [GOOLInstruction(1, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(1, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(1, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(1, GameVersion.Crash2)]
    [GOOLInstruction(1, GameVersion.Crash3)]
    public sealed class Sub : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "-";
        public override string GetName(GOOLInstruction ins) => "SUB";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} - {ins.GetArg('R')}";
    }

    [GOOLInstruction(2, GameVersion.Crash1)]
    [GOOLInstruction(2, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(2, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(2, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(2, GameVersion.Crash2)]
    [GOOLInstruction(2, GameVersion.Crash3)]
    public sealed class Mul : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "*";
        public override string GetName(GOOLInstruction ins) => "MUL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} * {ins.GetArg('R')}";
    }

    [GOOLInstruction(3, GameVersion.Crash1)]
    [GOOLInstruction(3, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(3, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(3, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(3, GameVersion.Crash2)]
    [GOOLInstruction(3, GameVersion.Crash3)]
    public sealed class Div : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "/";
        public override string GetName(GOOLInstruction ins) => "DIV";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} / {ins.GetArg('R')}";
    }

    [GOOLInstruction(4, GameVersion.Crash1)]
    [GOOLInstruction(4, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(4, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(4, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(4, GameVersion.Crash2)]
    [GOOLInstruction(4, GameVersion.Crash3)]
    public sealed class Eql : GOOLInsLR
    {
        public override string LispHead => "=";
        public override string GetName(GOOLInstruction ins) => "EQL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} == {ins.GetArg('R')}";
    }

    [GOOLInstruction(5, GameVersion.Crash1)]
    [GOOLInstruction(5, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(5, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(5, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(5, GameVersion.Crash2)]
    [GOOLInstruction(5, GameVersion.Crash3)]
    public sealed class And : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "and";
        public override string GetName(GOOLInstruction ins) => "AND";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} && {ins.GetArg('R')}";
    }

    [GOOLInstruction(6, GameVersion.Crash1)]
    [GOOLInstruction(6, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(6, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(6, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(6, GameVersion.Crash2)]
    [GOOLInstruction(6, GameVersion.Crash3)]
    public sealed class Or : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "or";
        public override string GetName(GOOLInstruction ins) => "OR";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} || {ins.GetArg('R')}";
    }

    [GOOLInstruction(7, GameVersion.Crash1)]
    [GOOLInstruction(7, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(7, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(7, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(7, GameVersion.Crash2)]
    [GOOLInstruction(7, GameVersion.Crash3)]
    public sealed class Andl : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "logand";
        public override string GetName(GOOLInstruction ins) => "ANDL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} & {ins.GetArg('R')}";
    }

    [GOOLInstruction(8, GameVersion.Crash1)]
    [GOOLInstruction(8, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(8, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(8, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(8, GameVersion.Crash2)]
    [GOOLInstruction(8, GameVersion.Crash3)]
    public sealed class Orl : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "logior";
        public override string GetName(GOOLInstruction ins) => "ORL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} | {ins.GetArg('R')}";
    }

    [GOOLInstruction(9, GameVersion.Crash1)]
    [GOOLInstruction(9, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(9, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(9, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(9, GameVersion.Crash2)]
    [GOOLInstruction(9, GameVersion.Crash3)]
    public sealed class Lt : GOOLInsLR
    {
        public override string LispHead => "<";
        public override string GetName(GOOLInstruction ins) => "LT";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} < {ins.GetArg('R')}";
    }

    [GOOLInstruction(10, GameVersion.Crash1)]
    [GOOLInstruction(10, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(10, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(10, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(10, GameVersion.Crash2)]
    [GOOLInstruction(10, GameVersion.Crash3)]
    public sealed class Lte : GOOLInsLR
    {
        public override string LispHead => "<=";
        public override string GetName(GOOLInstruction ins) => "LTE";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} <= {ins.GetArg('R')}";
    }

    [GOOLInstruction(11, GameVersion.Crash1)]
    [GOOLInstruction(11, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(11, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(11, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(11, GameVersion.Crash2)]
    [GOOLInstruction(11, GameVersion.Crash3)]
    public sealed class Gt : GOOLInsLR
    {
        public override string LispHead => ">";
        public override string GetName(GOOLInstruction ins) => "GT";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} > {ins.GetArg('R')}";
    }

    [GOOLInstruction(12, GameVersion.Crash1)]
    [GOOLInstruction(12, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(12, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(12, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(12, GameVersion.Crash2)]
    [GOOLInstruction(12, GameVersion.Crash3)]
    public sealed class Gte : GOOLInsLR
    {
        public override string LispHead => ">=";
        public override string GetName(GOOLInstruction ins) => "GTE";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} >= {ins.GetArg('R')}";
    }

    [GOOLInstruction(13, GameVersion.Crash1)]
    [GOOLInstruction(13, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(13, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(13, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(13, GameVersion.Crash2)]
    [GOOLInstruction(13, GameVersion.Crash3)]
    public sealed class Mod : GOOLInsLR
    {
        public override string LispHead => "%";
        public override string GetName(GOOLInstruction ins) => "MOD";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} % {ins.GetArg('R')}";
    }

    [GOOLInstruction(14, GameVersion.Crash1)]
    [GOOLInstruction(14, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(14, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(14, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(14, GameVersion.Crash2)]
    [GOOLInstruction(14, GameVersion.Crash3)]
    public sealed class Xorl : GOOLInsLR
    {
        public override bool Mergeable => true;
        public override string LispHead => "logxor";
        public override string GetName(GOOLInstruction ins) => "XORL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} ^ {ins.GetArg('R')}";
    }

    [GOOLInstruction(15, GameVersion.Crash1)]
    [GOOLInstruction(15, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(15, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(15, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(15, GameVersion.Crash2)]
    [GOOLInstruction(15, GameVersion.Crash3)]
    public sealed class Tsta : GOOLInsLR
    {
        public override string LispHead => "logbitsp";
        public override string GetName(GOOLInstruction ins) => "BITS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} has {ins.GetArg('R')}";
    }

    [GOOLInstruction(16, GameVersion.Crash1)]
    [GOOLInstruction(16, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(16, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(16, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(16, GameVersion.Crash2)]
    [GOOLInstruction(16, GameVersion.Crash3)]
    public sealed class Rand : GOOLInsLR
    {
        public override string LispHead => "rand";
        public override string GetName(GOOLInstruction ins) => "RAND";
        public override string GetComment(GOOLInstruction ins) => $"rand({ins.GetArg('L')}, {ins.GetArg('R')})";

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            if (ins.TryGetImmediate('L', out int min) && min == 0)
            {
                return new ListObj(new TokenObj(LispHead), DecompileArgStandard(ins, 'R', statement, ref i));
            }
            return base.DecompileToLisp(statement, ref i);
        }
    }

    [GOOLInstruction(17, GameVersion.Crash1)]
    [GOOLInstruction(17, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(17, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(17, GameVersion.Crash1BetaMAY11)]
    public sealed class Setf : GOOLInsDestCrash1
    {
        public override string LispHead => "setf";
        public override string GetName(GOOLInstruction ins) => "SETF";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = {ins.GetArg('S')}";
    }

    [GOOLInstruction(17, GameVersion.Crash2)]
    [GOOLInstruction(17, GameVersion.Crash3)]
    public sealed class Setf2 : GOOLInsDestCrash2
    {
        public override string LispHead => "setf";
        public override string GetName(GOOLInstruction ins) => "SETF";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = {ins.GetArg('S')}";
    }

    [GOOLInstruction(18, GameVersion.Crash1)]
    [GOOLInstruction(18, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(18, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(18, GameVersion.Crash1BetaMAY11)]
    public sealed class Not : GOOLInsDestCrash1
    {
        public override string LispHead => "not";
        public override string GetName(GOOLInstruction ins) => "NOT";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = !{ins.GetArg('S')}";
    }

    [GOOLInstruction(18, GameVersion.Crash2)]
    [GOOLInstruction(18, GameVersion.Crash3)]
    public sealed class Not2 : GOOLInsDestCrash2
    {
        public override string LispHead => "not";
        public override string GetName(GOOLInstruction ins) => "NOT";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = !{ins.GetArg('S')}";
    }

    [GOOLInstruction(19, GameVersion.Crash1)]
    [GOOLInstruction(19, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(19, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(19, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(19, GameVersion.Crash2)]
    [GOOLInstruction(19, GameVersion.Crash3)]
    public sealed class Loop : GOOLInsLR
    {
        public override string LispHead => "loop";
        public override string GetName(GOOLInstruction ins) => "LOOP";
        public override string GetComment(GOOLInstruction ins) => $"loop({ins.GetArg('L')}, " + (ins.IsDoubleStackRef('R') ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(20, GameVersion.Crash1)]
    [GOOLInstruction(20, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(20, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(20, GameVersion.Crash1BetaMAY11)]
    public sealed class Lea : GOOLInsDestCrash1
    {
        public override string LispHead => "&";
        public override string GetName(GOOLInstruction ins) => "LEA";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &{ins.GetArg('S')}";
    }

    [GOOLInstruction(20, GameVersion.Crash2)]
    [GOOLInstruction(20, GameVersion.Crash3)]
    public sealed class Lea2 : GOOLInsDestCrash2
    {
        public override string LispHead => "&";
        public override string GetName(GOOLInstruction ins) => "LEA";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &{ins.GetArg('S')}";
    }

    [GOOLInstruction(21, GameVersion.Crash1)]
    [GOOLInstruction(21, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(21, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(21, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(21, GameVersion.Crash2)]
    [GOOLInstruction(21, GameVersion.Crash3)]
    public sealed class Ash : GOOLInsLR
    {
        public override string LispHead => "ash";
        public override string GetName(GOOLInstruction ins) => "ASH";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} << {ins.GetArg('R')}";
    }

    [GOOLInstruction(22, GameVersion.Crash1)]
    [GOOLInstruction(22, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(22, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(22, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(22, GameVersion.Crash2)]
    [GOOLInstruction(22, GameVersion.Crash3)]
    public sealed class Push : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "PUSH";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins)
        {
            if (!ins.IsNullRef('B'))
                return $"push {ins.GetArg('A')} and {ins.GetArg('B')}";
            else
                return $"push {ins.GetArg('A')}";
        }
        public override int StackPush(GOOLInstruction ins) => ins.IsNullRef('B') ? 1 : 2;

        private bool decompHack = false;
        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            if (!decompHack)
            {
                if (!ins.IsNullRef('B'))
                {
                    var val = DecompileArgStandard(ins, 'B', statement, ref i);
                    decompHack = true;
                    i--;
                    return val;
                }
                return DecompileArgStandard(ins, 'A', statement, ref i);
            }
            else
            {
                decompHack = false;
                return DecompileArgStandard(ins, 'A', statement, ref i);
            }
        }
    }

    [GOOLInstruction(23, GameVersion.Crash1)]
    [GOOLInstruction(23, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(23, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(23, GameVersion.Crash1BetaMAY11)]
    public sealed class Notl : GOOLInsDestCrash1
    {
        public override string LispHead => "lognor";
        public override string GetName(GOOLInstruction ins) => "NOTL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = ~{ins.GetArg('S')}";
    }

    [GOOLInstruction(23, GameVersion.Crash2)]
    [GOOLInstruction(23, GameVersion.Crash3)]
    public sealed class Notl2 : GOOLInsDestCrash2
    {
        public override string LispHead => "lognor";
        public override string GetName(GOOLInstruction ins) => "NOTL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = ~{ins.GetArg('S')}";
    }

    [GOOLInstruction(24, GameVersion.Crash1)]
    [GOOLInstruction(24, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(24, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(24, GameVersion.Crash1BetaMAY11)]
    public sealed class Setc : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "SETC";
        public override string GetFormat() => "IIIIIIIIIIIIII (RRRRRR) 0000";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('R')} = ins[{ins.GetArgNoHex('I')}]";
        public override int StackPush(GOOLInstruction ins) => ins.IsStackRef('R') ? 1 : 0;
    }

    [GOOLInstruction(24, GameVersion.Crash2)]
    [GOOLInstruction(24, GameVersion.Crash3)]
    public sealed class Setc2 : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "SETC";
        public override string GetFormat() => "IIIIIIIIIIIIII E 000 (RRRRRR)";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('R')} = {(ins.Args['E'].Value == 0 ? "ins" : "ext")}[{ins.GetArgNoHex('I')}]";
        public override int StackPush(GOOLInstruction ins) => ins.IsStackRef('R') ? 1 : 0;
    }

    [GOOLInstruction(25, GameVersion.Crash1)]
    [GOOLInstruction(25, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(25, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(25, GameVersion.Crash1BetaMAY11)]
    public sealed class Abs : GOOLInsDestCrash1
    {
        public override string LispHead => "abs";
        public override string GetName(GOOLInstruction ins) => "ABS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = abs({ins.GetArg('S')})";
    }

    [GOOLInstruction(25, GameVersion.Crash2)]
    [GOOLInstruction(25, GameVersion.Crash3)]
    public sealed class Abs2 : GOOLInsDestCrash2
    {
        public override string LispHead => "abs";
        public override string GetName(GOOLInstruction ins) => "ABS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = abs({ins.GetArg('S')})";
    }

    [GOOLInstruction(26, GameVersion.Crash1)]
    [GOOLInstruction(26, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(26, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(26, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(26, GameVersion.Crash2)]
    [GOOLInstruction(26, GameVersion.Crash3)]
    public sealed class Cpad : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "CPAD";
        public override string GetFormat() => "BBBBBBBBBBBB PP SS DDDD T 000";
        public override string GetComment(GOOLInstruction ins) => $"";
        public override int StackPush(GOOLInstruction ins) => 1;
    }

    [GOOLInstruction(27, GameVersion.Crash1)]
    [GOOLInstruction(27, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(27, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(27, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(27, GameVersion.Crash2)]
    [GOOLInstruction(27, GameVersion.Crash3)]
    public sealed class Vel : GOOLInsLR
    {
        public override string LispHead => "vel";
        public override string GetName(GOOLInstruction ins) => "VEL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} + VEL({ins.GetArg('R')})";
    }

    [GOOLInstruction(28, GameVersion.Crash1)]
    [GOOLInstruction(28, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(28, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(28, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(28, GameVersion.Crash2)]
    [GOOLInstruction(28, GameVersion.Crash3)]
    public sealed class Misc : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "MISC";
        public override string GetFormat() => "[XXXXXXXXXXXX] (LLL) SSSSS PPPP";
        public override string GetComment(GOOLInstruction ins)
        {
            int kind = ins.Args['P'].Value;
            int subkind = ins.Args['S'].Value;
            switch (kind)
            {
                case 0:
                    // this became ARRL
                    return ins.GOOL.Version >= GOOLVersion.Version2 ? string.Empty : $"push word {ins.GetArg('S')} of array {ins.GetArg('X')}";
                case 1:
                    switch (subkind)
                    {
                        case 0: return $"push approximate distance to {ins.GetArg('L')}";
                        case 1: return $"push exact distance to {ins.GetArg('L')}";
                        case 2: return $"push approximate XZ distance to {ins.GetArg('L')}";
                        case 3: return $"push exact XZ distance to {ins.GetArg('L')}";
                    }
                    break;
                case 6:
                    switch (subkind)
                    {
                        case 0: return $"push approximate distance from vector {ins.GetArg('X')} to {ins.GetArg('L')}";
                        case 1: return $"push exact distance from vector {ins.GetArg('X')} to {ins.GetArg('L')}";
                        case 2: return $"push approximate XZ distance from vector {ins.GetArg('X')} to {ins.GetArg('L')}";
                        case 3: return $"push exact XZ distance from vector {ins.GetArg('X')} to {ins.GetArg('L')}";
                    }
                    break;
                case 2:
                    return $"push angle difference between {ins.GetArg('L')} and vector {ins.GetArg('X')}";
                case 3:
                    return $"push field {ins.GetArg('X')} of object {ins.GetArg('L')}";
                case 4:
                    return $"pop into field {ins.GetArg('X')} of object {ins.GetArg('L')}";
                case 5:
                    return $"push XZ plane angle difference to {ins.GetArg('L')}";
                case 7:
                    return $"push object in proc tree 3 or 4 with ID {ins.GetArg('X')}";
                case 8:
                    return $"set entity ID {ins.GetArg('X')} status bit 2 to {ins.GetArg('S')}";
                case 9:
                    return ins.IsNullRef('X') ? $"set {ins.GetArg('L')}'s parent zone to current zone"
                                              : $"set {ins.GetArg('L')}'s parent zone to zone at {ins.GetArg('X')}";
                case 10:
                    switch (subkind)
                    {
                        case 0: return $"clear entity ID {ins.GetArg('X')} status bit 3";
                        case 1: return $"set entity ID {ins.GetArg('X')} status bit 3";
                        case 2: return $"clear entity ID {ins.GetArg('X')} status bit 4";
                        case 3: return $"set entity ID {ins.GetArg('X')} status bit 4";
                        case 4: return $"clear entity ID {ins.GetArg('X')} status bit 4 and perm flag";
                        case 5: return $"set entity ID {ins.GetArg('X')} status bit 4 and perm flag";
                        case 8: return $"clear entity ID {ins.GetArg('X')} status bit 1";
                        case 9: return $"set entity ID {ins.GetArg('X')} status bit 1";
                    }
                    break;
                case 11:
                    switch (subkind)
                    {
                        case 1: return $"push entity ID {ins.GetArg('X')} status bit 2";
                        case 2: return $"push entity ID {ins.GetArg('X')} status bit 3";
                        case 3: return $"push entity ID {ins.GetArg('X')} status bit 4";
                    }
                    break;
                case 12:
                    if (ins.GOOL.Version < GOOLVersion.Version2)
                    {
                        switch (subkind)
                        {
                            case 0: return $"save continue point";
                            case 1: return $"load continue point";
                            case 2: return $"move object to active process list {ins.GetArg('X')}";
                            case 3: return $"unused function with argument {ins.GetArg('X')}";
                            case 4: return $"move to current object zone?";
                            case 5: return $"MIDI reset fade";
                            case 6: return $"MIDI playback";
                            case 7: return $"kill all objects outside current zone";
                            case 8: return $"push 3D angle difference between this and {ins.GetArg('L')}";
                            case 9: return $"change to level {ins.GetArg('X')}";
                            case 10: return $"CD seek to level {ins.GetArg('X')}";
                            case 11: return $"reset game info global variables";
                        }
                    }
                    else
                    {
                        switch (subkind)
                        {
                            case 0: return $"save continue point with params {ins.GetArg('X')}";
                            case 1: return $"load continue point {ins.GetArg('X')}";
                            case 2: return $"move object to active process list {ins.GetArg('X')}";
                            case 3: return $"unused function with argument {ins.GetArg('X')}";
                            case 4: return $"move to current object zone?";
                            case 5: return $"MIDI reset fade";
                            case 6: return $"MIDI playback";
                            case 7: return $"kill all objects outside current zone";
                            case 8: return $"push 3D angle difference between this and {ins.GetArg('L')}";
                            case 9: return $"change to level {ins.GetArg('X')}";
                            case 10: return $"CD seek to level {ins.GetArg('X')}";
                            case 11: return $"reset game info global variables";
                            case 12: return $"store current time in ms in {ins.GetArg('X')}";
                            case 13: return $"set current time in ms to {ins.GetArg('X')}";
                            case 14: return $"set {ins.GetArg('L')}'s external GOOL file to {ins.GetArg('X')}";
                            case 15: return $"push {ins.GetArg('L')}'s external GOOL file";
                            case 16: return $"set {ins.GetArg('L')}'s main GOOL file to {ins.GetArg('X')}";
                            case 17: return $"push {ins.GetArg('L')}'s main GOOL file";
                            case 18: return $"kill victims";
                            case 20: return $"store left stick coords in {ins.GetArg('X')}";
                            case 21: return $"unknown animation operation";
                        }
                    }
                    break;
                case 13: return $"push object nearest to {ins.GetArg('L')}";
                case 14: return $"push collision result in {ins.GetArg('L')} with point {ins.GetArg('X')}";
                case 15:
                    return $"memory card operation {subkind} (TBD)";
                    switch (subkind)
                    {
                        case 2: return $"unknown operation 15-2(0x80), set misc to 1";
                        case 3: return $"set misc to unknown operation 15-3({ins.GetArg('X')})";
                        case 5: return $"set misc to unknown operation 15-5";
                        case 7: return $"set misc to unknown operation 15-7";
                        case 8: return $"set misc to unknown operation 15-8";
                        case 10: return $"set misc to unknown operation 15-10";
                        case 12: return $"set misc to unknown operation 15-12";
                        case 13: return $"set misc to unknown operation 15-13({ins.GetArg('X')})";
                        case 14: return $"set misc to unknown operation 15-14({ins.GetArg('X')})";
                    }
                    return $"set misc to 1";
            }
            return string.Empty;
        }
        public override int StackPush(GOOLInstruction ins)
        {
            switch (ins.Args['P'].Value)
            {
                case 0:
                    return ins.GOOL.Version >= GOOLVersion.Version2 ? 0 : 1;
                case 12:
                    return ins.Args['S'].Value == 8 || ins.Args['S'].Value == 15 || ins.Args['S'].Value == 17 ? 1 : 0;
                case 1:
                case 2:
                case 3:
                case 5:
                case 6:
                case 7:
                case 13:
                case 14:
                    return 1;
                case 11:
                    return (ins.Args['S'].Value >= 1 && ins.Args['S'].Value <= 3) ? 1 : 0;
            }
            return 0;
        }
        public override int StackPop(GOOLInstruction ins)
        {
            switch (ins.Args['P'].Value)
            {
                case 4: return 1 + base.StackPop(ins);
            }
            return base.StackPop(ins);
        }
    }

    [GOOLInstruction(29, GameVersion.Crash1)]
    [GOOLInstruction(29, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(29, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(29, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(29, GameVersion.Crash2)]
    [GOOLInstruction(29, GameVersion.Crash3)]
    public sealed class Psin : GOOLInsLR
    {
        public override string LispHead => "psin";
        public override string GetName(GOOLInstruction ins) => "PSIN";
        public override string GetComment(GOOLInstruction ins) => $"psin({ins.GetArg('L')},{ins.GetArg('R')})";
    }

    [GOOLInstruction(30, GameVersion.Crash1)]
    [GOOLInstruction(30, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(30, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(30, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(30, GameVersion.Crash2)]
    [GOOLInstruction(30, GameVersion.Crash3)]
    public sealed class Sync : GOOLInsLR
    {
        public override string LispHead => "sync";
        public override string GetName(GOOLInstruction ins) => "SYNC";
        public override string GetComment(GOOLInstruction ins) => $"({ins.GetArg('R')} + time) % {ins.GetArg('L')}";
    }

    [GOOLInstruction(31, GameVersion.Crash1)]
    [GOOLInstruction(31, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(31, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(31, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(31, GameVersion.Crash2)]
    [GOOLInstruction(31, GameVersion.Crash3)]
    public sealed class Gvar : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "GVAR";
        public override string GetFormat() => "[IIIIIIIIIIII] 000001111101";
        public override string GetComment(GOOLInstruction ins)
        {
            if (ins.TryGetImmediate('I', out int gindex))
            {
                var name = GOOLInterpreter.GetGlobalName(ins.GOOL.Version, gindex >> 8);
                if (name != null)
                    return $"push global {name}";
            }
            return $"push global[{ins.GetArg('I')}]";
        }
        public override int StackPush(GOOLInstruction ins) => 1;

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            if (ins.TryGetImmediate('I', out int gindex))
            {
                var name = GOOLInterpreter.GetGlobalName(ins.GOOL.Version, gindex >> 8);
                if (name == null)
                {
                    name = "*global-" + (gindex >> 8).ToString() + "*";
                }
                return new TokenObj(name);
            }
            return new ListObj(new TokenObj("global"), DecompileArgStandard(ins, 'I', statement, ref i));
        }
    }

    [GOOLInstruction(32, GameVersion.Crash1)]
    [GOOLInstruction(32, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(32, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(32, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(32, GameVersion.Crash2)]
    [GOOLInstruction(32, GameVersion.Crash3)]
    public sealed class Gvaw : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "GVAW";
        public override string GetFormat() => "[IIIIIIIIIIII] [SSSSSSSSSSSS]";
        public override string GetComment(GOOLInstruction ins)
        {
            if (ins.TryGetImmediate('I', out int gindex))
            {
                var name = GOOLInterpreter.GetGlobalName(ins.GOOL.Version, gindex >> 8);
                if (name != null)
                    return $"global {name} = {ins.GetArg('S')}";
            }
            return $"global[{ins.GetArg('I')}] = {ins.GetArg('S')}";
        }
        public override int StackPush(GOOLInstruction ins) => 0;
    }

    [GOOLInstruction(33, GameVersion.Crash1)]
    [GOOLInstruction(33, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(33, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(33, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(33, GameVersion.Crash2)]
    [GOOLInstruction(33, GameVersion.Crash3)]
    public sealed class Degd : GOOLInsLR
    {
        public override string LispHead => "deg-diff";
        public override string GetName(GOOLInstruction ins) => "DEGD";
        public override string GetComment(GOOLInstruction ins) => $"degdiff({ins.GetArg('L')},{ins.GetArg('R')})";
    }

    [GOOLInstruction(34, GameVersion.Crash1)]
    [GOOLInstruction(34, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(34, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(34, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(34, GameVersion.Crash2)]
    [GOOLInstruction(34, GameVersion.Crash3)]
    public sealed class Seek : GOOLInsLR
    {
        public override string LispHead => "seek";
        public override string GetName(GOOLInstruction ins) => "SEEK";
        public override string GetComment(GOOLInstruction ins) => $"seek({ins.GetArg('L')}, " + (ins.IsDoubleStackRef('R') ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(35, GameVersion.Crash1)]
    [GOOLInstruction(35, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(35, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(35, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(35, GameVersion.Crash2)]
    [GOOLInstruction(35, GameVersion.Crash3)]
    public sealed class Colr : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "COLR";
        public override string GetFormat() => "000001111101 LLL IIIIII 000";
        public override string GetComment(GOOLInstruction ins) => $"{(ObjectFields)ins.Args['L'].Value}->{GOOLInterpreter.GetColor(ins.GOOL.Version, ins.Args['I'].Value)}";
        public override int StackPush(GOOLInstruction ins) => 1;
    }

    [GOOLInstruction(36, GameVersion.Crash1)]
    [GOOLInstruction(36, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(36, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(36, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(36, GameVersion.Crash2)]
    [GOOLInstruction(36, GameVersion.Crash3)]
    public sealed class Colw : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "COLW";
        public override string GetFormat() => "[CCCCCCCCCCCC] LLL IIIIII 000";
        public override string GetComment(GOOLInstruction ins) => $"{(ObjectFields)ins.Args['L'].Value}->{GOOLInterpreter.GetColor(ins.GOOL.Version, ins.Args['I'].Value)} = {ins.GetArg('C')}";
        public override int StackPush(GOOLInstruction ins) => 0;
    }

    [GOOLInstruction(37, GameVersion.Crash1)]
    [GOOLInstruction(37, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(37, GameVersion.Crash2)]
    [GOOLInstruction(37, GameVersion.Crash3)]
    public sealed class Dsek : GOOLInsLR
    {
        public override string LispHead => "degseek";
        public override string GetName(GOOLInstruction ins) => "DSEK";
        public override string GetComment(GOOLInstruction ins) => $"degseek({ins.GetArg('L')}, " + (ins.IsDoubleStackRef('R') ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(38, GameVersion.Crash1)]
    [GOOLInstruction(38, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(38, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(38, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(38, GameVersion.Crash2)]
    [GOOLInstruction(38, GameVersion.Crash3)]
    public sealed class Pshp : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "PSHP";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins)
        {
            if (ins.Args['B'].Value != GOOLInstruction.NullRef)
                return $"push &{ins.GetArg('A')} and &{ins.GetArg('B')}";
            else
                return $"push &{ins.GetArg('A')}";
        }
        public override int StackPush(GOOLInstruction ins) => ins.Args['B'].Value == GOOLInstruction.NullRef ? 1 : 2;
    }

    [GOOLInstruction(39, GameVersion.Crash1)]
    [GOOLInstruction(39, GameVersion.Crash1BetaMAY11)]
    public sealed class Anis : GOOLInsDestCrash1
    {
        public override string LispHead => "frame-group";
        public override string GetName(GOOLInstruction ins) => "ANIS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &fgroups[{ins.GetArg('S')}]";
    }

    [GOOLInstruction(39, GameVersion.Crash2)]
    [GOOLInstruction(39, GameVersion.Crash3)]
    public sealed class Anis2 : GOOLInsDestCrash2
    {
        public override string LispHead => "frame-group";
        public override string GetName(GOOLInstruction ins) => "ANIS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &fgroups[{ins.GetArg('S')}]";
    }

    [GOOLInstruction(40, GameVersion.Crash2)]
    [GOOLInstruction(40, GameVersion.Crash3)]
    public sealed class Eflr : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => $"EFLR";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => $"push entity field {ins.GetArg('A')} row {ins.GetArg('B')}";
        public override int StackPush(GOOLInstruction ins) => 1;
    }

    [GOOLInstruction(41, GameVersion.Crash2)]
    [GOOLInstruction(41, GameVersion.Crash3)]
    public sealed class Eflv : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => $"EFLV";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('A')} = entity field {ins.GetArg('B')}";
        public override int StackPush(GOOLInstruction ins) => ins.IsStackRef('A') ? 1 : 0;
    }

    [GOOLInstruction(42, GameVersion.Crash2)]
    [GOOLInstruction(42, GameVersion.Crash3)]
    public sealed class Arrl : GOOLInsLR
    {
        public override string LispHead => "->";
        public override string GetName(GOOLInstruction ins) => "ARRL";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')}[{ins.GetArg('R')}]";
    }

    [GOOLInstruction(43, GameVersion.Crash2)]
    [GOOLInstruction(43, GameVersion.Crash3)]
    public sealed class Sin : GOOLInsDestCrash2
    {
        public override string LispHead => "sin";
        public override string GetName(GOOLInstruction ins) => "SIN";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = sin({ins.GetArg('S')})";
    }

    [GOOLInstruction(44, GameVersion.Crash2)]
    [GOOLInstruction(44, GameVersion.Crash3)]
    public sealed class Cos : GOOLInsDestCrash2
    {
        public override string LispHead => "cos";
        public override string GetName(GOOLInstruction ins) => "COS";
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = cos({ins.GetArg('S')})";
    }

    [GOOLInstruction(45, GameVersion.Crash2)]
    [GOOLInstruction(45, GameVersion.Crash3)]
    public sealed class Atan : GOOLInsLR
    {
        public override string LispHead => "atan2";
        public override string GetName(GOOLInstruction ins) => "ATAN";
        public override string GetComment(GOOLInstruction ins) => $"atan2({ins.GetArg('L')}, {ins.GetArg('R')})";
    }

    [GOOLInstruction(129, GameVersion.Crash1)]
    [GOOLInstruction(129, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(129, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(129, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(47, GameVersion.Crash2)]
    [GOOLInstruction(47, GameVersion.Crash3)]
    public sealed class Nop : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "NOP";
        public override string GetFormat() => "101111100000101111100000".Reverse();
        public override string GetComment(GOOLInstruction ins) => "no operation";
        public override int StackPush(GOOLInstruction ins) => 0;
    }

    [GOOLInstruction(128, GameVersion.Crash1)]
    [GOOLInstruction(128, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(128, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(128, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(48, GameVersion.Crash2)]
    [GOOLInstruction(48, GameVersion.Crash3)]
    public sealed class Dbg : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "DBG";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => $"print {ins.GetArg('A')} and {ins.GetArg('B')}";
        public override int StackPush(GOOLInstruction ins) => 0;
    }

    [GOOLInstruction(49, GameVersion.Crash2)]
    [GOOLInstruction(49, GameVersion.Crash3)]
    public sealed class Ret : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "RET";
        public override string GetFormat() => "0000000000 0000 100110 00 01";
        public override string GetComment(GOOLInstruction ins) => $"return";

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            return new ListObj(new TokenObj("return"));
        }
    }

    [GOOLInstruction(50, GameVersion.Crash2)]
    [GOOLInstruction(50, GameVersion.Crash3)]
    public sealed class Bra : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "BRA";
        public override string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 00 00";
        public override string GetComment(GOOLInstruction ins)
        {
            int v = ins.Args['V'].Value;
            int i = ins.Args['I'].Value;
            if (v != 0 && i != 0)
            {
                return $"move {i} instructions and pop {v} values";
            }
            else if (v == 0)
            {
                return $"move {i} instructions";
            }
            else
            {
                return $"pop {v} values";
            }
        }
        public override int StackPush(GOOLInstruction ins) => 0;

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            int v = ins.Args['V'].Value;
            int o = ins.Args['I'].Value;
            var res = new ListObj(new TokenObj("b"), new NumberObj(o));
            if (v != 0)
            {
                if (o != 0)
                {
                    int zzzzz = 999 + 1;
                    Console.Write("");
                }
                res.Forms.Add(new TokenObj(":pop"));
                res.Forms.Add(new NumberObj(v));
            }
            return res;
        }
    }

    [GOOLInstruction(51, GameVersion.Crash2)]
    [GOOLInstruction(51, GameVersion.Crash3)]
    public sealed class Bnez : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "BNEZ";
        public override string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 10 00";
        public override string GetComment(GOOLInstruction ins)
        {
            int v = ins.Args['V'].Value;
            int i = ins.Args['I'].Value;
            string str = $"if {(ObjectFields)ins.Args['R'].Value} is true, ";
            if (v != 0 && i != 0)
            {
                return str + $"move {i} instructions and pop {v} values";
            }
            else if (v == 0)
            {
                return str + $"move {i} instructions";
            }
            else
            {
                return str + $"pop {v} values";
            }
        }

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            int v = ins.Args['V'].Value;
            int o = ins.Args['I'].Value;
            var res = new ListObj(new TokenObj("b-if"), new NumberObj(o), DecompileArgStandard(ins, 'R', statement, ref i));
            if (v != 0)
            {
                res.Forms.Add(new TokenObj(":pop"));
                res.Forms.Add(new NumberObj(v));
            }
            return res;
        }
    }

    [GOOLInstruction(52, GameVersion.Crash2)]
    [GOOLInstruction(52, GameVersion.Crash3)]
    public sealed class Beqz : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "BEQZ";
        public override string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 01 00";
        public override string GetComment(GOOLInstruction ins)
        {
            int v = ins.Args['V'].Value;
            int i = ins.Args['I'].Value;
            string str = $"if {(ObjectFields)ins.Args['R'].Value} is false, ";
            if (v != 0 && i != 0)
            {
                return str + $"move {i} instructions and pop {v} values";
            }
            else if (v == 0)
            {
                return str + $"move {i} instructions";
            }
            else
            {
                return str + $"pop {v} values";
            }
        }

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            int v = ins.Args['V'].Value;
            int o = ins.Args['I'].Value;
            var res = new ListObj(new TokenObj("b-unless"), new NumberObj(o), DecompileArgStandard(ins, 'R', statement, ref i));
            if (v != 0)
            {
                res.Forms.Add(new TokenObj(":pop"));
                res.Forms.Add(new NumberObj(v));
            }
            return res;
        }
    }

    [GOOLInstruction(53, GameVersion.Crash2)]
    [GOOLInstruction(53, GameVersion.Crash3)]
    public class Go : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "GO";
        public override string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 00 10";
        public override string GetComment(GOOLInstruction ins) => $"go to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
        public override int StackPop(GOOLInstruction ins) => base.StackPop(ins) + ins.Args['V'].Value;

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var res = new ListObj(new TokenObj("go"), new TokenObj("state-" + ins.Args['S'].Value.ToString()));
            res.Forms.AddRange(DecompileVarargs(ins.Args['V'].Value, statement, ref i));
            return res;
            // (go state-0 ...)
        }
    }

    [GOOLInstruction(54, GameVersion.Crash2)]
    [GOOLInstruction(54, GameVersion.Crash3)]
    public sealed class Gnez : Go
    {
        public override string GetName(GOOLInstruction ins) => "GNEZ";
        public override string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 10 10";
        public override string GetComment(GOOLInstruction ins) => $"if {(ObjectFields)ins.Args['R'].Value} is true, go to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var res = new ListObj(new TokenObj("if"), DecompileArgStandard(ins, 'R', statement, ref i), new ListObj(new TokenObj("go"), new TokenObj("state-" + ins.Args['S'].Value.ToString())));
            res.Forms.AddRange(DecompileVarargs(ins.Args['V'].Value, statement, ref i));
            return res;
            // (if c (go state-0 ...))
        }
    }

    [GOOLInstruction(55, GameVersion.Crash2)]
    [GOOLInstruction(55, GameVersion.Crash3)]
    public sealed class Geqz : Go
    {
        public override string GetName(GOOLInstruction ins) => "GEQZ";
        public override string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 01 10";
        public override string GetComment(GOOLInstruction ins) => $"if {(ObjectFields)ins.Args['R'].Value} is false, go to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var res = new ListObj(new TokenObj("unless"), DecompileArgStandard(ins, 'R', statement, ref i), new ListObj(new TokenObj("go"), new TokenObj("state-" + ins.Args['S'].Value.ToString())));
            res.Forms.AddRange(DecompileVarargs(ins.Args['V'].Value, statement, ref i));
            return res;
            // (unless c (go state-0 ...))
        }
    }

    [GOOLInstruction(130, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(130, GameVersion.Crash1BetaMAR08)]
    public sealed class Cfl_95 : Cfl
    {
        public override string GetFormat() => "<IIIIIIIII> VVVVV (RRRRRR) CC TT";
    }

    [GOOLInstruction(130, GameVersion.Crash1)]
    [GOOLInstruction(130, GameVersion.Crash1BetaMAY11)]
    public class Cfl : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins)
        {
            switch (ins.Args['T'].Value)
            {
                case 0:
                    switch (ins.Args['C'].Value)
                    {
                        case 0:
                            return "BRA";
                        case 1:
                            return "BNEZ";
                        case 2:
                            return "BEQZ";
                    }
                    break;
                case 1:
                    switch (ins.Args['C'].Value)
                    {
                        case 0:
                            return "GO";
                        case 1:
                            return "GNEZ";
                        case 2:
                            return "GEQZ";
                    }
                    break;
                case 2:
                    switch (ins.Args['C'].Value)
                    {
                        case 0:
                            return "RET";
                        case 1:
                            return "RNEZ";
                        case 2:
                            return "REQZ";
                    }
                    break;
            }
            return "CFL";
        }

        public override string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) CC TT";
        public override string GetComment(GOOLInstruction ins)
        {
            int v = ins.Args['V'].Value;
            int i = ins.Args['I'].Value;
            string str = string.Empty;
            switch (ins.Args['C'].Value)
            {
                case 1:
                    str = $"if {(ObjectFields)ins.Args['R'].Value} is true, ";
                    break;
                case 2:
                    str = $"if {(ObjectFields)ins.Args['R'].Value} is false, ";
                    break;
            }
            switch (ins.Args['T'].Value)
            {
                case 0:
                    if (v != 0 && i != 0)
                    {
                        return str + $"move {i} instructions and pop {v} values";
                    }
                    else if (v == 0)
                    {
                        return str + $"move {i} instructions";
                    }
                    else
                    {
                        return str + $"pop {v} values";
                    }
                case 1:
                    return str + $"go to state {ins.GetArg('I')}" + (v > 0 ? $" with {ins.GetArg('V')} arguments" : string.Empty);
                case 2:
                    return str + "return";
            }
            return "invalid instruction";
        }
    }

    [GOOLInstruction(131, GameVersion.Crash1)]
    [GOOLInstruction(131, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(131, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(131, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(56, GameVersion.Crash2)]
    [GOOLInstruction(56, GameVersion.Crash3)]
    public sealed class Anim : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "ANIM";
        public override string GetFormat() => "FFFFFFF SSSSSSSSS TTTTTT HH";
        public override string GetComment(GOOLInstruction ins) => $"play anim &{ins.GetArg('S')} frame {ins.GetArg('F')} for {ins.GetArg('T')} frames {FlipComments[ins.Args['H'].Value]}";

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var f = ins.Args['F'].Value;
            var s = ins.Args['S'].Value;
            var t = ins.Args['T'].Value;
            var h = ins.Args['H'].Value;
            var res = new ListObj(new TokenObj("play-frame"), new NumberObj(f), new TokenObj(":group"), new NumberObj(s));
            if (t != 1)
            {
                res.Forms.Add(new TokenObj(":duration"));
                res.Forms.Add(new NumberObj(t));
            }
            if (h != 3)
            {
                res.Forms.Add(new TokenObj(":flip"));
                res.Forms.Add(new NumberObj(h));
            }
            return res;
        }
    }

    [GOOLInstruction(132, GameVersion.Crash1)]
    [GOOLInstruction(132, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(132, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(132, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(57, GameVersion.Crash2)]
    [GOOLInstruction(57, GameVersion.Crash3)]
    public sealed class Anif : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "ANIF";
        public override string GetFormat() => "[FFFFFFFFFFFF] 0000 TTTTTT HH";
        public override string GetComment(GOOLInstruction ins) => $"play frame {ins.GetArg('F')} for {ins.GetArg('T')} frames {FlipComments[ins.Args['H'].Value]}";

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var f = DecompileArgStandard(ins, 'F', statement, ref i);
            var t = ins.Args['T'].Value;
            var h = ins.Args['H'].Value;
            var res = new ListObj(new TokenObj("play-frame"), f);
            if (t != 1)
            {
                res.Forms.Add(new TokenObj(":duration"));
                res.Forms.Add(new NumberObj(t));
            }
            if (h != 3)
            {
                res.Forms.Add(new TokenObj(":flip"));
                res.Forms.Add(new NumberObj(h));
            }
            return res;
        }
    }

    [GOOLInstruction(133, GameVersion.Crash1)]
    [GOOLInstruction(133, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(133, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(133, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(58, GameVersion.Crash2)]
    [GOOLInstruction(58, GameVersion.Crash3)]
    public sealed class Vec : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "VEC";
        public override string GetFormat() => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";

        public override int StackPop(GOOLInstruction ins)
        {
            int val = 0;
            switch (ins.Args['T'].Value)
            {
                case 4:
                case 5:
                    val = 2; break;
            }
            return base.StackPop(ins) + val;
        }

        public override string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(134, GameVersion.Crash1)]
    [GOOLInstruction(134, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(134, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(134, GameVersion.Crash1BetaMAY11)]
    public sealed class Call : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "CALL";
        public override string GetFormat() => "IIIIIIIIIIIIII (RRRRRR) VVVV";
        public override string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.Args['I'].Value}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var res = new ListObj(new TokenObj("sub-" + ins.Args['I'].Value.ToString()));
            res.Forms.AddRange(DecompileVarargs(ins.Args['V'].Value, statement, ref i));
            return res;
        }
    }

    [GOOLInstruction(59, GameVersion.Crash2)]
    [GOOLInstruction(59, GameVersion.Crash3)]
    public sealed class Call2 : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "CALL";
        public override string GetFormat() => "IIIIIIIIIIIIII E 00000 VVVV";
        public override string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.Args['I'].Value}" + (ins.Args['E'].Value == 1 ? " (external)" : string.Empty) + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var res = new ListObj(new TokenObj("sub-" + ins.Args['I'].Value.ToString()));
            res.Forms.AddRange(DecompileVarargs(ins.Args['V'].Value, statement, ref i));
            return res;
        }
    }

    [GOOLInstruction(135, GameVersion.Crash1)]
    [GOOLInstruction(135, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(135, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(135, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(60, GameVersion.Crash2)]
    [GOOLInstruction(60, GameVersion.Crash3)]
    public sealed class Sevt : GOOLInsArgs
    {
        public override string GetName(GOOLInstruction ins) => "SEVT";
        public override string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public override string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}send event {ins.GetArg('E')} to {ins.GetArg('L')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");

        public override GObj DecompileToLisp(GOOLStatement statement, ref int i)
        {
            var ins = statement.Instructions[i];
            var e = DecompileArgStandard(ins, 'E', statement, ref i);
            var c = ins.Args['R'].Value == 0 ? null : DecompileArgStandard(ins, 'R', statement, ref i);
            var res = new ListObj(new TokenObj("send-event"), ins.ArgToLisp('L'), e);
            res.Forms.AddRange(DecompileVarargs(ins.Args['A'].Value, statement, ref i));
            return c == null ? res : new ListObj(new TokenObj("if"), res);
            // (send-event a b ...)
        }
    }

    [GOOLInstruction(136, GameVersion.Crash1)]
    [GOOLInstruction(136, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(136, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(136, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(61, GameVersion.Crash2)]
    [GOOLInstruction(61, GameVersion.Crash3)]
    public sealed class Rjev : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "RJEV";
        public override string GetFormat() => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string GetComment(GOOLInstruction ins)
        {
            string str = string.Empty;
            if (ins.Args['C'].Value != 0)
            {
                if (ins.Args['C'].Value == 1)
                    str += $"if {ins.GetArg('R')}, ";
                else if (ins.Args['C'].Value == 2)
                    str += $"if not {ins.GetArg('R')}, ";
            }
            str += "reject event";

            if (ins.Args['T'].Value == 0)
            {
                str += $" or move {ins.GetArg('I')} instructions";
            }
            else if (ins.Args['T'].Value == 1)
            {
                str += $" and change state to {ins.GetArg('I')}";
            }
            else if (ins.Args['T'].Value == 2)
            {
                str += $" and return";
            }

            return str;
        }
    }

    [GOOLInstruction(137, GameVersion.Crash1)]
    [GOOLInstruction(137, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(137, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(137, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(62, GameVersion.Crash2)]
    [GOOLInstruction(62, GameVersion.Crash3)]
    public sealed class Acev : GOOLInsBranch
    {
        public override string GetName(GOOLInstruction ins) => "ACEV";
        public override string GetFormat() => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string GetComment(GOOLInstruction ins)
        {
            string str = string.Empty;
            if (ins.Args['C'].Value != 0)
            {
                if (ins.Args['C'].Value == 1)
                    str += $"if {ins.GetArg('R')}, ";
                else if (ins.Args['C'].Value == 2)
                    str += $"if not {ins.GetArg('R')}, ";
            }
            str += "accept event";

            if (ins.Args['T'].Value == 0)
            {
                str += $" or move {ins.GetArg('I')} instructions";
            }
            else if (ins.Args['T'].Value == 1)
            {
                str += $" and change state to {ins.GetArg('I')}";
            }
            else if (ins.Args['T'].Value == 2)
            {
                str += $" and return";
            }

            return str;
        }
    }

    [GOOLInstruction(138, GameVersion.Crash1)]
    [GOOLInstruction(138, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(138, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(138, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(63, GameVersion.Crash2)]
    [GOOLInstruction(63, GameVersion.Crash3)]
    public sealed class Spwn : GOOLInsArgs
    {
        public override string GetName(GOOLInstruction ins) => "SPWN";
        public override string GetFormat() => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public override string GetComment(GOOLInstruction ins) => $"spawn {(ins.Args['C'].Value != 0 ? ins.GetArg('C') : "[sp]")}x object {ins.GetArg('T')} subtype {ins.GetArg('S')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} arguments" : "");
        public override int StackPop(GOOLInstruction ins) => (ins.Args['C'].Value == 0 ? 1 : 0) + base.StackPop(ins);
    }

    [GOOLInstruction(139, GameVersion.Crash1)]
    [GOOLInstruction(139, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(139, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(139, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(64, GameVersion.Crash2)]
    [GOOLInstruction(64, GameVersion.Crash3)]
    public sealed class Chnk : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "CHNK";
        public override string GetFormat() => "[EEEEEEEEEEEE] [TTTTTTTTTTTT]";
        public override string GetComment(GOOLInstruction ins)
        {
            if (ins.TryGetImmediate('T', out int kind))
            {
                switch (kind)
                {
                    case 1: return $"open file {ins.GetArg('E')}, async";
                    case 2: return $"close file {ins.GetArg('E')}";
                    case 3: return $"push file {ins.GetArg('E')} is loaded";
                    case 4: return $"push available page amount";
                    case 5: return $"push page amount for {ins.GetArg('E')} files";
                    case 6: return $"open file {ins.GetArg('E')}, blocking";
                }
            }
            return string.Empty;
        }
        public override int StackPush(GOOLInstruction ins)
        {
            if (ins.TryGetImmediate('T', out int kind))
            {
                switch (kind)
                {
                    case 3:
                    case 4:
                    case 5:
                        return 1;
                }
            }
            return 0;
        }
        public override int StackPop(GOOLInstruction ins)
        {
            if (ins.TryGetImmediate('T', out int kind) && kind == 3 && ins.TryGetImmediate('E', out int amount))
            {
                // if this fails then fuck everything, what are you even doing!
                return base.StackPop(ins) + amount;
            }
            return base.StackPop(ins);
        }
    }

    [GOOLInstruction(140, GameVersion.Crash1)]
    [GOOLInstruction(140, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(140, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(140, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(65, GameVersion.Crash2)]
    [GOOLInstruction(65, GameVersion.Crash3)]
    public sealed class Sndp : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "SNDP";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => $"play sound {ins.GetArg('A')} at {ins.GetArg('B')} volume";
    }

    [GOOLInstruction(141, GameVersion.Crash1)]
    [GOOLInstruction(141, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(141, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(141, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(66, GameVersion.Crash2)]
    [GOOLInstruction(66, GameVersion.Crash3)]
    public sealed class Snda : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "SNDA";
        public override string GetFormat() => "[SSSSSSSSSSSS] (RRRRRR) FF TTTT";
        public override string GetComment(GOOLInstruction ins)
        {
            switch (ins.Args['T'].Value)
            {
                case 1: return $"set audio pitch to {ins.GetArg('S')}";
                case 4: return $"set audio count to {ins.GetArg('S')}";
                case 7: return $"set audio delay to {ins.GetArg('S')}";
                case 12: return $"set audio decay rate to {ins.GetArg('S')}";
                default: return string.Empty;
            }
        }
    }

    [GOOLInstruction(142, GameVersion.Crash1)]
    [GOOLInstruction(142, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(142, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(142, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(67, GameVersion.Crash2)]
    [GOOLInstruction(67, GameVersion.Crash3)]
    public sealed class Coll : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "COLL";
        public override string GetFormat() => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";
        public override string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(143, GameVersion.Crash1)]
    [GOOLInstruction(143, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(143, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(143, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(68, GameVersion.Crash2)]
    [GOOLInstruction(68, GameVersion.Crash3)]
    public sealed class Bevt : GOOLInsArgs
    {
        public override string GetName(GOOLInstruction ins) => "BEVT";
        public override string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA LLL";
        public override string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}send event {ins.GetArg('E')} type {ins.GetArg('L')} to every object" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");
    }

    [GOOLInstruction(144, GameVersion.Crash1)]
    [GOOLInstruction(144, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(144, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(144, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(69, GameVersion.Crash2)]
    [GOOLInstruction(69, GameVersion.Crash3)]
    public sealed class Cevt : GOOLInsArgs
    {
        public override string GetName(GOOLInstruction ins) => "CEVT";
        public override string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public override string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}cascade event {ins.GetArg('E')} from {ins.GetArg('L')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");
    }

    [GOOLInstruction(145, GameVersion.Crash1)]
    [GOOLInstruction(145, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(70, GameVersion.Crash2)]
    [GOOLInstruction(70, GameVersion.Crash3)]
    public sealed class Spwf : GOOLInsArgs
    {
        public override string GetName(GOOLInstruction ins) => "SPWF";
        public override string GetFormat() => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public override string GetComment(GOOLInstruction ins) => $"force spawn {(ins.Args['C'].Value != 0 ? ins.GetArg('C') : "[sp]")}x object {ins.GetArg('T')} subtype {ins.GetArg('S')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} arguments" : "");
        public override int StackPop(GOOLInstruction ins) => (ins.Args['C'].Value == 0 ? 1 : 0) + base.StackPop(ins);
    }

    [GOOLInstruction(71, GameVersion.Crash2)]
    [GOOLInstruction(71, GameVersion.Crash3)]
    public sealed class Calr : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => $"CALR";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.GetArg('A')} with {ins.GetArg('B')} argument(s)";
    }

    [GOOLInstruction(73, GameVersion.Crash2)]
    [GOOLInstruction(73, GameVersion.Crash3)]
    public sealed class Mips : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "MIPS";
        public override string GetFormat() => "101111100000101111100000".Reverse();
        public override string GetComment(GOOLInstruction ins) => "begin R3000A native bytecode";
    }

    [GOOLInstruction(78, GameVersion.Crash2)]
    [GOOLInstruction(78, GameVersion.Crash3)]
    public sealed class Arrs : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => "ARRS";
        public override string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public override string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')}[{ins.GetArg('R')}] = [sp]";
        public override int StackPop(GOOLInstruction ins) => base.StackPop(ins) + 1;
    }

    [GOOLInstruction(72, GameVersion.Crash2)]
    [GOOLInstruction(72, GameVersion.Crash3)]
    [GOOLInstruction(74, GameVersion.Crash2)]
    [GOOLInstruction(74, GameVersion.Crash3)]
    [GOOLInstruction(75, GameVersion.Crash2)]
    [GOOLInstruction(75, GameVersion.Crash3)]
    [GOOLInstruction(76, GameVersion.Crash2)]
    [GOOLInstruction(76, GameVersion.Crash3)]
    [GOOLInstruction(77, GameVersion.Crash2)]
    [GOOLInstruction(77, GameVersion.Crash3)]
    [GOOLInstruction(79, GameVersion.Crash3)]
    [GOOLInstruction(80, GameVersion.Crash3)]
    [GOOLInstruction(81, GameVersion.Crash3)]
    public sealed class Unk : GOOLInsOpcode
    {
        public override string GetName(GOOLInstruction ins) => $"UNK{ins.ID}";
        public override string GetFormat() => GOOLInstruction.DefaultFormat;
        public override string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(46, GameVersion.Crash2)]
    [GOOLInstruction(46, GameVersion.Crash3)]
    public sealed class Unk46 : GOOLInsDestCrash2
    {
        public override string LispHead => "unk46";
        public override string GetName(GOOLInstruction ins) => $"UNK46";
        public override string GetComment(GOOLInstruction ins) => string.Empty;
    }
}