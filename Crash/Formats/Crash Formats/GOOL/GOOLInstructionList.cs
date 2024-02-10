/*
 * Every single known GOOL instruction.
 * 
 */

namespace Crash.GOOLIns
{
    [GOOLInstruction(0, GameVersion.Crash1)]
    [GOOLInstruction(0, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(0, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(0, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(0, GameVersion.Crash2)]
    [GOOLInstruction(0, GameVersion.Crash3)]
    public sealed class Add
    {
        public static string GetName(GOOLInstruction ins) => "ADD";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} + {ins.GetArg('R')}";
    }

    [GOOLInstruction(1, GameVersion.Crash1)]
    [GOOLInstruction(1, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(1, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(1, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(1, GameVersion.Crash2)]
    [GOOLInstruction(1, GameVersion.Crash3)]
    public sealed class Sub
    {
        public static string GetName(GOOLInstruction ins) => "SUB";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} - {ins.GetArg('R')}";
    }

    [GOOLInstruction(2, GameVersion.Crash1)]
    [GOOLInstruction(2, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(2, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(2, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(2, GameVersion.Crash2)]
    [GOOLInstruction(2, GameVersion.Crash3)]
    public sealed class Mul
    {
        public static string GetName(GOOLInstruction ins) => "MUL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} * {ins.GetArg('R')}";
    }

    [GOOLInstruction(3, GameVersion.Crash1)]
    [GOOLInstruction(3, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(3, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(3, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(3, GameVersion.Crash2)]
    [GOOLInstruction(3, GameVersion.Crash3)]
    public sealed class Div
    {
        public static string GetName(GOOLInstruction ins) => "DIV";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} / {ins.GetArg('R')}";
    }

    [GOOLInstruction(4, GameVersion.Crash1)]
    [GOOLInstruction(4, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(4, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(4, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(4, GameVersion.Crash2)]
    [GOOLInstruction(4, GameVersion.Crash3)]
    public sealed class Eql
    {
        public static string GetName(GOOLInstruction ins) => "EQL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} == {ins.GetArg('R')}";
    }

    [GOOLInstruction(5, GameVersion.Crash1)]
    [GOOLInstruction(5, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(5, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(5, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(5, GameVersion.Crash2)]
    [GOOLInstruction(5, GameVersion.Crash3)]
    public sealed class And
    {
        public static string GetName(GOOLInstruction ins) => "AND";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} && {ins.GetArg('R')}";
    }

    [GOOLInstruction(6, GameVersion.Crash1)]
    [GOOLInstruction(6, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(6, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(6, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(6, GameVersion.Crash2)]
    [GOOLInstruction(6, GameVersion.Crash3)]
    public sealed class Or
    {
        public static string GetName(GOOLInstruction ins) => "OR";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} || {ins.GetArg('R')}";
    }

    [GOOLInstruction(7, GameVersion.Crash1)]
    [GOOLInstruction(7, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(7, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(7, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(7, GameVersion.Crash2)]
    [GOOLInstruction(7, GameVersion.Crash3)]
    public sealed class Andl
    {
        public static string GetName(GOOLInstruction ins) => "ANDL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} & {ins.GetArg('R')}";
    }

    [GOOLInstruction(8, GameVersion.Crash1)]
    [GOOLInstruction(8, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(8, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(8, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(8, GameVersion.Crash2)]
    [GOOLInstruction(8, GameVersion.Crash3)]
    public sealed class Orl
    {
        public static string GetName(GOOLInstruction ins) => "ORL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} | {ins.GetArg('R')}";
    }

    [GOOLInstruction(9, GameVersion.Crash1)]
    [GOOLInstruction(9, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(9, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(9, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(9, GameVersion.Crash2)]
    [GOOLInstruction(9, GameVersion.Crash3)]
    public sealed class Lt
    {
        public static string GetName(GOOLInstruction ins) => "LT";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} < {ins.GetArg('R')}";
    }

    [GOOLInstruction(10, GameVersion.Crash1)]
    [GOOLInstruction(10, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(10, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(10, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(10, GameVersion.Crash2)]
    [GOOLInstruction(10, GameVersion.Crash3)]
    public sealed class Lte
    {
        public static string GetName(GOOLInstruction ins) => "LTE";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} <= {ins.GetArg('R')}";
    }

    [GOOLInstruction(11, GameVersion.Crash1)]
    [GOOLInstruction(11, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(11, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(11, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(11, GameVersion.Crash2)]
    [GOOLInstruction(11, GameVersion.Crash3)]
    public sealed class Gt
    {
        public static string GetName(GOOLInstruction ins) => "GT";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} > {ins.GetArg('R')}";
    }

    [GOOLInstruction(12, GameVersion.Crash1)]
    [GOOLInstruction(12, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(12, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(12, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(12, GameVersion.Crash2)]
    [GOOLInstruction(12, GameVersion.Crash3)]
    public sealed class Gte
    {
        public static string GetName(GOOLInstruction ins) => "GTE";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} >= {ins.GetArg('R')}";
    }

    [GOOLInstruction(13, GameVersion.Crash1)]
    [GOOLInstruction(13, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(13, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(13, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(13, GameVersion.Crash2)]
    [GOOLInstruction(13, GameVersion.Crash3)]
    public sealed class Mod
    {
        public static string GetName(GOOLInstruction ins) => "MOD";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} % {ins.GetArg('R')}";
    }

    [GOOLInstruction(14, GameVersion.Crash1)]
    [GOOLInstruction(14, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(14, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(14, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(14, GameVersion.Crash2)]
    [GOOLInstruction(14, GameVersion.Crash3)]
    public sealed class Xorl
    {
        public static string GetName(GOOLInstruction ins) => "XORL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} ^ {ins.GetArg('R')}";
    }

    [GOOLInstruction(15, GameVersion.Crash1)]
    [GOOLInstruction(15, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(15, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(15, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(15, GameVersion.Crash2)]
    [GOOLInstruction(15, GameVersion.Crash3)]
    public sealed class Tsta
    {
        public static string GetName(GOOLInstruction ins) => "TSTA";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} has {ins.GetArg('R')}";
    }

    [GOOLInstruction(16, GameVersion.Crash1)]
    [GOOLInstruction(16, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(16, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(16, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(16, GameVersion.Crash2)]
    [GOOLInstruction(16, GameVersion.Crash3)]
    public sealed class Rand
    {
        public static string GetName(GOOLInstruction ins) => "RAND";
        public static string GetFormat() => "[BBBBBBBBBBBB] [AAAAAAAAAAAA]";
        public static string GetComment(GOOLInstruction ins) => $"rand({ins.GetArg('B')}, {ins.GetArg('A')})";
    }

    [GOOLInstruction(17, GameVersion.Crash1)]
    [GOOLInstruction(17, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(17, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(17, GameVersion.Crash1BetaMAY11)]
    public sealed class Setf
    {
        public static string GetName(GOOLInstruction ins) => "SETF";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = {ins.GetArg('S')}";
    }

    [GOOLInstruction(17, GameVersion.Crash2)]
    [GOOLInstruction(17, GameVersion.Crash3)]
    public sealed class Setf2
    {
        public static string GetName(GOOLInstruction ins) => "SETF";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = {ins.GetArg('S')}";
    }

    [GOOLInstruction(18, GameVersion.Crash1)]
    [GOOLInstruction(18, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(18, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(18, GameVersion.Crash1BetaMAY11)]
    public sealed class Not
    {
        public static string GetName(GOOLInstruction ins) => "NOT";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = !{ins.GetArg('S')}";
    }

    [GOOLInstruction(18, GameVersion.Crash2)]
    [GOOLInstruction(18, GameVersion.Crash3)]
    public sealed class Not2
    {
        public static string GetName(GOOLInstruction ins) => "NOT";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = !{ins.GetArg('S')}";
    }

    [GOOLInstruction(19, GameVersion.Crash1)]
    [GOOLInstruction(19, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(19, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(19, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(19, GameVersion.Crash2)]
    [GOOLInstruction(19, GameVersion.Crash3)]
    public sealed class Loop
    {
        public static string GetName(GOOLInstruction ins) => "LOOP";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"loop({ins.GetArg('L')}, " + (ins.Args['R'].Value == GOOLInstruction.DoubleStackRef ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(20, GameVersion.Crash1)]
    [GOOLInstruction(20, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(20, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(20, GameVersion.Crash1BetaMAY11)]
    public sealed class Lea
    {
        public static string GetName(GOOLInstruction ins) => "LEA";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &{ins.GetArg('S')}";
    }

    [GOOLInstruction(20, GameVersion.Crash2)]
    [GOOLInstruction(20, GameVersion.Crash3)]
    public sealed class Lea2
    {
        public static string GetName(GOOLInstruction ins) => "LEA";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &{ins.GetArg('S')}";
    }

    [GOOLInstruction(21, GameVersion.Crash1)]
    [GOOLInstruction(21, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(21, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(21, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(21, GameVersion.Crash2)]
    [GOOLInstruction(21, GameVersion.Crash3)]
    public sealed class Ash
    {
        public static string GetName(GOOLInstruction ins) => "ASH";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')} << {ins.GetArg('R')}";
    }

    [GOOLInstruction(22, GameVersion.Crash1)]
    [GOOLInstruction(22, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(22, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(22, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(22, GameVersion.Crash2)]
    [GOOLInstruction(22, GameVersion.Crash3)]
    public sealed class Push
    {
        public static string GetName(GOOLInstruction ins) => "PUSH";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins)
        {
            if (ins.Args['B'].Value != GOOLInstruction.NullRef)
                return $"push {ins.GetArg('A')} and {ins.GetArg('B')}";
            else
                return $"push {ins.GetArg('A')}";
        }
    }

    [GOOLInstruction(23, GameVersion.Crash1)]
    [GOOLInstruction(23, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(23, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(23, GameVersion.Crash1BetaMAY11)]
    public sealed class Notl
    {
        public static string GetName(GOOLInstruction ins) => "NOTL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = ~{ins.GetArg('S')}";
    }

    [GOOLInstruction(23, GameVersion.Crash2)]
    [GOOLInstruction(23, GameVersion.Crash3)]
    public sealed class Notl2
    {
        public static string GetName(GOOLInstruction ins) => "NOTL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = ~{ins.GetArg('S')}";
    }

    [GOOLInstruction(24, GameVersion.Crash1)]
    [GOOLInstruction(24, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(24, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(24, GameVersion.Crash1BetaMAY11)]
    public sealed class Setc
    {
        public static string GetName(GOOLInstruction ins) => "SETC";
        public static string GetFormat() => "IIIIIIIIIIIIII (RRRRRR) 0000";
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('R')} = ins[{ins.GetArg('I')}]";
    }

    [GOOLInstruction(24, GameVersion.Crash2)]
    [GOOLInstruction(24, GameVersion.Crash3)]
    public sealed class Setc2
    {
        public static string GetName(GOOLInstruction ins) => "SETC";
        public static string GetFormat() => "IIIIIIIIIIIIII E 000 (RRRRRR)";
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('R')} = {(ins.Args['E'].Value == 0 ? "ins" : "ext")}[{ins.GetArg('I')}]";
    }

    [GOOLInstruction(25, GameVersion.Crash1)]
    [GOOLInstruction(25, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(25, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(25, GameVersion.Crash1BetaMAY11)]
    public static class Abs
    {
        public static string GetName(GOOLInstruction ins) => "ABS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = abs({ins.GetArg('S')})";
    }

    [GOOLInstruction(25, GameVersion.Crash2)]
    [GOOLInstruction(25, GameVersion.Crash3)]
    public static class Abs2
    {
        public static string GetName(GOOLInstruction ins) => "ABS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = abs({ins.GetArg('S')})";
    }

    [GOOLInstruction(26, GameVersion.Crash1)]
    [GOOLInstruction(26, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(26, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(26, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(26, GameVersion.Crash2)]
    [GOOLInstruction(26, GameVersion.Crash3)]
    public sealed class Cpad
    {
        public static string GetName(GOOLInstruction ins) => "CPAD";
        public static string GetFormat() => "BBBBBBBBBBBB PP SS DDDD T 000";
        public static string GetComment(GOOLInstruction ins) => $"";
    }

    [GOOLInstruction(27, GameVersion.Crash1)]
    [GOOLInstruction(27, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(27, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(27, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(27, GameVersion.Crash2)]
    [GOOLInstruction(27, GameVersion.Crash3)]
    public sealed class Vel
    {
        public static string GetName(GOOLInstruction ins) => "VEL";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"VEL({ins.GetArg('A')}, {ins.GetArg('B')})";
    }

    [GOOLInstruction(28, GameVersion.Crash1)]
    [GOOLInstruction(28, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(28, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(28, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(28, GameVersion.Crash2)]
    [GOOLInstruction(28, GameVersion.Crash3)]
    public sealed class Misc
    {
        public static string GetName(GOOLInstruction ins) => "MISC";
        public static string GetFormat() => "[XXXXXXXXXXXX] (LLL) SSSSS PPPP";
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(29, GameVersion.Crash1)]
    [GOOLInstruction(29, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(29, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(29, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(29, GameVersion.Crash2)]
    [GOOLInstruction(29, GameVersion.Crash3)]
    public sealed class Psin
    {
        public static string GetName(GOOLInstruction ins) => "PSIN";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"psin({ins.GetArg('A')},{ins.GetArg('B')})";
    }

    [GOOLInstruction(30, GameVersion.Crash1)]
    [GOOLInstruction(30, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(30, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(30, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(30, GameVersion.Crash2)]
    [GOOLInstruction(30, GameVersion.Crash3)]
    public sealed class Sync
    {
        public static string GetName(GOOLInstruction ins) => "SYNC";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"({ins.GetArg('R')} + time) % {ins.GetArg('L')}";
    }

    [GOOLInstruction(31, GameVersion.Crash1)]
    [GOOLInstruction(31, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(31, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(31, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(31, GameVersion.Crash2)]
    [GOOLInstruction(31, GameVersion.Crash3)]
    public sealed class Gvar
    {
        public static string GetName(GOOLInstruction ins) => "GVAR";
        public static string GetFormat() => "[IIIIIIIIIIII] 000001111101";
        public static string GetComment(GOOLInstruction ins) => $"push global[{ins.GetArg('I')}]";
    }

    [GOOLInstruction(32, GameVersion.Crash1)]
    [GOOLInstruction(32, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(32, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(32, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(32, GameVersion.Crash2)]
    [GOOLInstruction(32, GameVersion.Crash3)]
    public sealed class Gvaw
    {
        public static string GetName(GOOLInstruction ins) => "GVAW";
        public static string GetFormat() => "[IIIIIIIIIIII] [SSSSSSSSSSSS]";
        public static string GetComment(GOOLInstruction ins) => $"global[{ins.GetArg('I')}] = {ins.GetArg('S')}";
    }

    [GOOLInstruction(33, GameVersion.Crash1)]
    [GOOLInstruction(33, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(33, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(33, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(33, GameVersion.Crash2)]
    [GOOLInstruction(33, GameVersion.Crash3)]
    public sealed class Degd
    {
        public static string GetName(GOOLInstruction ins) => "DEGD";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"degdiff({ins.GetArg('L')},{ins.GetArg('R')})";
    }

    [GOOLInstruction(34, GameVersion.Crash1)]
    [GOOLInstruction(34, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(34, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(34, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(34, GameVersion.Crash2)]
    [GOOLInstruction(34, GameVersion.Crash3)]
    public sealed class Seek
    {
        public static string GetName(GOOLInstruction ins) => "SEEK";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"seek({ins.GetArg('L')}, " + (ins.Args['R'].Value == GOOLInstruction.DoubleStackRef ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(35, GameVersion.Crash1)]
    [GOOLInstruction(35, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(35, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(35, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(35, GameVersion.Crash2)]
    [GOOLInstruction(35, GameVersion.Crash3)]
    public sealed class Colr
    {
        public static string GetName(GOOLInstruction ins) => "COLR";
        public static string GetFormat() => "000001111101 LLL IIIIII 000";
        public static string GetComment(GOOLInstruction ins) => $"{(ObjectFields)ins.Args['L'].Value}->{GOOLInterpreter.GetColor(ins.GOOL.Version, ins.Args['I'].Value)}";
    }

    [GOOLInstruction(36, GameVersion.Crash1)]
    [GOOLInstruction(36, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(36, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(36, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(36, GameVersion.Crash2)]
    [GOOLInstruction(36, GameVersion.Crash3)]
    public sealed class Colw
    {
        public static string GetName(GOOLInstruction ins) => "COLW";
        public static string GetFormat() => "[CCCCCCCCCCCC] LLL IIIIII 000";
        public static string GetComment(GOOLInstruction ins) => $"{(ObjectFields)ins.Args['L'].Value}->{GOOLInterpreter.GetColor(ins.GOOL.Version, ins.Args['I'].Value)} = {ins.GetArg('C')}";
    }

    [GOOLInstruction(37, GameVersion.Crash1)]
    [GOOLInstruction(37, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(37, GameVersion.Crash2)]
    [GOOLInstruction(37, GameVersion.Crash3)]
    public sealed class Dsek
    {
        public static string GetName(GOOLInstruction ins) => "DSEK";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"degseek({ins.GetArg('L')}, " + (ins.Args['R'].Value == GOOLInstruction.DoubleStackRef ? "[sp-1], [sp])" : $"{ins.GetArg('R')}, 0x100)");
    }

    [GOOLInstruction(38, GameVersion.Crash1)]
    [GOOLInstruction(38, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(38, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(38, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(38, GameVersion.Crash2)]
    [GOOLInstruction(38, GameVersion.Crash3)]
    public sealed class Pshp
    {
        public static string GetName(GOOLInstruction ins) => "PSHP";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins)
        {
            if (ins.Args['B'].Value != GOOLInstruction.NullRef)
                return $"push &{ins.GetArg('A')} and &{ins.GetArg('B')}";
            else
                return $"push &{ins.GetArg('A')}";
        }
    }

    [GOOLInstruction(39, GameVersion.Crash1)]
    [GOOLInstruction(39, GameVersion.Crash1BetaMAY11)]
    public sealed class Anis
    {
        public static string GetName(GOOLInstruction ins) => "ANIS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &anim[{ins.GetArg('S')}]";
    }

    [GOOLInstruction(39, GameVersion.Crash2)]
    [GOOLInstruction(39, GameVersion.Crash3)]
    public sealed class Anis2
    {
        public static string GetName(GOOLInstruction ins) => "ANIS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = &anim[{ins.GetArg('S')}]";
    }

    [GOOLInstruction(40, GameVersion.Crash2)]
    [GOOLInstruction(40, GameVersion.Crash3)]
    public sealed class Eflr
    {
        public static string GetName(GOOLInstruction ins) => $"EFLR";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"push entity field {ins.GetArg('A')} row {ins.GetArg('B')}";
    }

    [GOOLInstruction(41, GameVersion.Crash2)]
    [GOOLInstruction(41, GameVersion.Crash3)]
    public sealed class Eflv
    {
        public static string GetName(GOOLInstruction ins) => $"EFLV";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('A')} = entity field {ins.GetArg('B')}";
    }

    [GOOLInstruction(42, GameVersion.Crash2)]
    [GOOLInstruction(42, GameVersion.Crash3)]
    public sealed class Arrl
    {
        public static string GetName(GOOLInstruction ins) => "ARRL";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')}[{ins.GetArg('R')}]";
    }

    [GOOLInstruction(43, GameVersion.Crash2)]
    [GOOLInstruction(43, GameVersion.Crash3)]
    public sealed class Sin
    {
        public static string GetName(GOOLInstruction ins) => "SIN";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = sin({ins.GetArg('S')})";
    }

    [GOOLInstruction(44, GameVersion.Crash2)]
    [GOOLInstruction(44, GameVersion.Crash3)]
    public sealed class Cos
    {
        public static string GetName(GOOLInstruction ins) => "COS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('D')} = cos({ins.GetArg('S')})";
    }

    [GOOLInstruction(45, GameVersion.Crash2)]
    [GOOLInstruction(45, GameVersion.Crash3)]
    public sealed class Atan
    {
        public static string GetName(GOOLInstruction ins) => "ATAN";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"atan2({ins.GetArg('A')}, {ins.GetArg('B')})";
    }

    [GOOLInstruction(129, GameVersion.Crash1)]
    [GOOLInstruction(129, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(129, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(129, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(47, GameVersion.Crash2)]
    [GOOLInstruction(47, GameVersion.Crash3)]
    public sealed class Nop
    {
        public static string GetName(GOOLInstruction ins) => "NOP";
        public static string GetFormat() => "101111100000101111100000".Reverse();
        public static string GetComment(GOOLInstruction ins) => "no operation";
    }

    [GOOLInstruction(128, GameVersion.Crash1)]
    [GOOLInstruction(128, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(128, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(128, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(48, GameVersion.Crash2)]
    [GOOLInstruction(48, GameVersion.Crash3)]
    public sealed class Dbg
    {
        public static string GetName(GOOLInstruction ins) => "DBG";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"print {ins.GetArg('A')} and {ins.GetArg('B')}";
    }

    [GOOLInstruction(49, GameVersion.Crash2)]
    [GOOLInstruction(49, GameVersion.Crash3)]
    public sealed class Ret
    {
        public static string GetName(GOOLInstruction ins) => "RET";
        public static string GetFormat() => "0000000000 0000 100110 00 01";
        public static string GetComment(GOOLInstruction ins) => $"return";
    }

    [GOOLInstruction(50, GameVersion.Crash2)]
    [GOOLInstruction(50, GameVersion.Crash3)]
    public sealed class Bra
    {
        public static string GetName(GOOLInstruction ins) => "BRA";
        public static string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 00 00";
        public static string GetComment(GOOLInstruction ins)
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
    }

    [GOOLInstruction(51, GameVersion.Crash2)]
    [GOOLInstruction(51, GameVersion.Crash3)]
    public sealed class Bnez
    {
        public static string GetName(GOOLInstruction ins) => "BNEZ";
        public static string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 10 00";
        public static string GetComment(GOOLInstruction ins)
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
    }

    [GOOLInstruction(52, GameVersion.Crash2)]
    [GOOLInstruction(52, GameVersion.Crash3)]
    public sealed class Beqz
    {
        public static string GetName(GOOLInstruction ins) => "BEQZ";
        public static string GetFormat() => "<IIIIIIIIII> VVVV (RRRRRR) 01 00";
        public static string GetComment(GOOLInstruction ins)
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
    }

    [GOOLInstruction(53, GameVersion.Crash2)]
    [GOOLInstruction(53, GameVersion.Crash3)]
    public sealed class Cst
    {
        public static string GetName(GOOLInstruction ins) => "CST";
        public static string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 00 10";
        public static string GetComment(GOOLInstruction ins) => $"change to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(54, GameVersion.Crash2)]
    [GOOLInstruction(54, GameVersion.Crash3)]
    public sealed class Cnez
    {
        public static string GetName(GOOLInstruction ins) => "CNEZ";
        public static string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 10 10";
        public static string GetComment(GOOLInstruction ins) => $"if {(ObjectFields)ins.Args['R'].Value} is true, change to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(55, GameVersion.Crash2)]
    [GOOLInstruction(55, GameVersion.Crash3)]
    public sealed class Ceqz
    {
        public static string GetName(GOOLInstruction ins) => "CEQZ";
        public static string GetFormat() => "SSSSSSSSSS VVVV (RRRRRR) 01 10";
        public static string GetComment(GOOLInstruction ins) => $"if {(ObjectFields)ins.Args['R'].Value} is false, change to state {ins.GetArg('S')}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(130, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(130, GameVersion.Crash1BetaMAR08)]
    public sealed class Cfl_95
    {
        public static string GetName(GOOLInstruction ins)
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
                            return "CST";
                        case 1:
                            return "CNEZ";
                        case 2:
                            return "CEQZ";
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

        public static string GetFormat() => "<IIIIIIIII> VVVVV (RRRRRR) CC TT";
        public static string GetComment(GOOLInstruction ins)
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
                    return str + $"change to state {ins.GetArg('I')}" + (v > 0 ? $" with {ins.GetArg('V')} arguments" : string.Empty);
                case 2:
                    return str + "return";
            }
            return "invalid instruction";
        }
    }

    [GOOLInstruction(130, GameVersion.Crash1)]
    [GOOLInstruction(130, GameVersion.Crash1BetaMAY11)]
    public sealed class Cfl
    {
        public static string GetName(GOOLInstruction ins)
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
                            return "CST";
                        case 1:
                            return "CNEZ";
                        case 2:
                            return "CEQZ";
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

        public static string GetFormat() => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public static string GetComment(GOOLInstruction ins)
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
                    return str + $"change to state {ins.GetArg('I')}" + (v > 0 ? $" with {ins.GetArg('V')} arguments" : string.Empty);
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
    public sealed class Anim
    {
        private static readonly string[] FlipComments = { "(force non-mirror)", "(force mirror)", "(mirror)", "" };

        public static string GetName(GOOLInstruction ins) => "ANIM";
        public static string GetFormat() => "FFFFFFF SSSSSSSSS TTTTTT HH";
        public static string GetComment(GOOLInstruction ins) => $"play anim &{ins.GetArg('S')} frame {ins.GetArg('F')} for {ins.GetArg('T')} frames {FlipComments[ins.Args['H'].Value]}";
    }

    [GOOLInstruction(132, GameVersion.Crash1)]
    [GOOLInstruction(132, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(132, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(132, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(57, GameVersion.Crash2)]
    [GOOLInstruction(57, GameVersion.Crash3)]
    public sealed class Anif
    {
        private static readonly string[] FlipComments = { "(force non-mirror)", "(force mirror)", "(mirror)", "" };

        public static string GetName(GOOLInstruction ins) => "ANIF";
        public static string GetFormat() => "[FFFFFFFFFFFF] 0000 TTTTTT HH";
        public static string GetComment(GOOLInstruction ins) => $"play frame {ins.GetArg('F')} for {ins.GetArg('T')} frames {FlipComments[ins.Args['H'].Value]}";
    }

    [GOOLInstruction(133, GameVersion.Crash1)]
    [GOOLInstruction(133, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(133, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(133, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(58, GameVersion.Crash2)]
    [GOOLInstruction(58, GameVersion.Crash3)]
    public sealed class Vec
    {
        public static string GetName(GOOLInstruction ins) => "VEC";
        public static string GetFormat() => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(134, GameVersion.Crash1)]
    [GOOLInstruction(134, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(134, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(134, GameVersion.Crash1BetaMAY11)]
    public sealed class Call
    {
        public static string GetName(GOOLInstruction ins) => "CALL";
        public static string GetFormat() => "IIIIIIIIIIIIII (RRRRRR) VVVV";
        public static string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.Args['I'].Value}" + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(59, GameVersion.Crash2)]
    [GOOLInstruction(59, GameVersion.Crash3)]
    public sealed class Call2
    {
        public static string GetName(GOOLInstruction ins) => "CALL";
        public static string GetFormat() => "IIIIIIIIIIIIII E 00000 VVVV";
        public static string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.Args['I'].Value}" + (ins.Args['E'].Value == 1 ? " (external)" : string.Empty) + (ins.Args['V'].Value > 0 ? $" with {ins.GetArg('V')} argument(s)" : string.Empty);
    }

    [GOOLInstruction(135, GameVersion.Crash1)]
    [GOOLInstruction(135, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(135, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(135, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(60, GameVersion.Crash2)]
    [GOOLInstruction(60, GameVersion.Crash3)]
    public sealed class Sevt
    {
        public static string GetName(GOOLInstruction ins) => "SEVT";
        public static string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public static string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}send event {ins.GetArg('E')} to {ins.GetArg('L')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");
    }

    [GOOLInstruction(136, GameVersion.Crash1)]
    [GOOLInstruction(136, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(136, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(136, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(61, GameVersion.Crash2)]
    [GOOLInstruction(61, GameVersion.Crash3)]
    public sealed class Rjev
    {
        public static string GetName(GOOLInstruction ins) => "RJEV";
        public static string GetFormat() => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public static string GetComment(GOOLInstruction ins)
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
    public sealed class Acev
    {
        public static string GetName(GOOLInstruction ins) => "ACEV";
        public static string GetFormat() => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public static string GetComment(GOOLInstruction ins)
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
    public sealed class Spwn
    {
        public static string GetName(GOOLInstruction ins) => "SPWN";
        public static string GetFormat() => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public static string GetComment(GOOLInstruction ins) => $"spawn {(ins.Args['C'].Value != 0 ? ins.GetArg('C') : "[sp]")}x object {ins.GetArg('T')} subtype {ins.GetArg('S')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} arguments" : "");
    }

    [GOOLInstruction(139, GameVersion.Crash1)]
    [GOOLInstruction(139, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(139, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(139, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(64, GameVersion.Crash2)]
    [GOOLInstruction(64, GameVersion.Crash3)]
    public sealed class Chnk
    {
        public static string GetName(GOOLInstruction ins) => "CHNK";
        public static string GetFormat() => "[EEEEEEEEEEEE] [TTTTTTTTTTTT]";
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(140, GameVersion.Crash1)]
    [GOOLInstruction(140, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(140, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(140, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(65, GameVersion.Crash2)]
    [GOOLInstruction(65, GameVersion.Crash3)]
    public sealed class Sndp
    {
        public static string GetName(GOOLInstruction ins) => "SNDP";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"play sound {ins.GetArg('A')} at {ins.GetArg('B')} volume";
    }

    [GOOLInstruction(141, GameVersion.Crash1)]
    [GOOLInstruction(141, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(141, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(141, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(66, GameVersion.Crash2)]
    [GOOLInstruction(66, GameVersion.Crash3)]
    public sealed class Snda
    {
        public static string GetName(GOOLInstruction ins) => "SNDA";
        public static string GetFormat() => "[SSSSSSSSSSSS] (RRRRRR) FF TTTT";
        public static string GetComment(GOOLInstruction ins)
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
    public sealed class Coll
    {
        public static string GetName(GOOLInstruction ins) => "COLL";
        public static string GetFormat() => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(143, GameVersion.Crash1)]
    [GOOLInstruction(143, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(143, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(143, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(68, GameVersion.Crash2)]
    [GOOLInstruction(68, GameVersion.Crash3)]
    public sealed class Bevt
    {
        public static string GetName(GOOLInstruction ins) => "BEVT";
        public static string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA LLL";
        public static string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}send event {ins.GetArg('E')} type {ins.GetArg('L')} to every object" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");
    }

    [GOOLInstruction(144, GameVersion.Crash1)]
    [GOOLInstruction(144, GameVersion.Crash1Beta1995)]
    [GOOLInstruction(144, GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(144, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(69, GameVersion.Crash2)]
    [GOOLInstruction(69, GameVersion.Crash3)]
    public sealed class Cevt
    {
        public static string GetName(GOOLInstruction ins) => "CEVT";
        public static string GetFormat() => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public static string GetComment(GOOLInstruction ins) => $"{(ins.Args['R'].Value > 0 ? $"if {ins.GetArg('R')}, " : "")}cascade event {ins.GetArg('E')} from {ins.GetArg('L')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} argument(s)" : "");
    }

    [GOOLInstruction(145, GameVersion.Crash1)]
    [GOOLInstruction(145, GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(70, GameVersion.Crash2)]
    [GOOLInstruction(70, GameVersion.Crash3)]
    public sealed class Spwf
    {
        public static string GetName(GOOLInstruction ins) => "SPWF";
        public static string GetFormat() => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public static string GetComment(GOOLInstruction ins) => $"force spawn {(ins.Args['C'].Value != 0 ? ins.GetArg('C') : "[sp]")}x object {ins.GetArg('T')} subtype {ins.GetArg('S')}" + (ins.Args['A'].Value > 0 ? $" with {ins.GetArg('A')} arguments" : "");
    }

    [GOOLInstruction(71, GameVersion.Crash2)]
    [GOOLInstruction(71, GameVersion.Crash3)]
    public sealed class Calr
    {
        public static string GetName(GOOLInstruction ins) => $"CALR";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => $"call subroutine at {ins.GetArg('A')} with {ins.GetArg('B')} argument(s)";
    }

    [GOOLInstruction(73, GameVersion.Crash2)]
    [GOOLInstruction(73, GameVersion.Crash3)]
    public sealed class Mips
    {
        public static string GetName(GOOLInstruction ins) => "MIPS";
        public static string GetFormat() => "101111100000101111100000".Reverse();
        public static string GetComment(GOOLInstruction ins) => "begin R3000A native bytecode";
    }

    [GOOLInstruction(78, GameVersion.Crash2)]
    [GOOLInstruction(78, GameVersion.Crash3)]
    public sealed class Arrs
    {
        public static string GetName(GOOLInstruction ins) => "ARRS";
        public static string GetFormat() => GOOLInstruction.DefaultFormatLR;
        public static string GetComment(GOOLInstruction ins) => $"{ins.GetArg('L')}[{ins.GetArg('R')}] = [sp]";
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
    public sealed class Unk
    {
        public static string GetName(GOOLInstruction ins) => $"UNK{ins.Opcode}";
        public static string GetFormat() => GOOLInstruction.DefaultFormat;
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }

    [GOOLInstruction(46, GameVersion.Crash2)]
    [GOOLInstruction(46, GameVersion.Crash3)]
    public sealed class Unk46
    {
        public static string GetName(GOOLInstruction ins) => $"UNK46";
        public static string GetFormat() => GOOLInstruction.DefaultFormatDS2;
        public static string GetComment(GOOLInstruction ins) => string.Empty;
    }
}