namespace Crash.GOOLIns
{
    [GOOLInstruction(144,GameVersion.Crash1)]
    [GOOLInstruction(144,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(144,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(144,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(69,GameVersion.Crash2)]
    [GOOLInstruction(69,GameVersion.Crash3)]
    public sealed class Evnc : GOOLInstruction
    {
        public Evnc(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "EVNC";
        public override string Format => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public override string Comment => $"{(Args['R'].Value > 0 ? $"if {GetArg('R')}, " : "")}cascade event {GetArg('E')} from {GetArg('L')}" + (Args['A'].Value > 0 ? $" with {GetArg('A')} argument(s)" : "");
    }
}
