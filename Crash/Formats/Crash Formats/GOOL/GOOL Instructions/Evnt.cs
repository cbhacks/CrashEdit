namespace Crash.GOOLIns
{
    [GOOLInstruction(135,GameVersion.Crash1)]
    [GOOLInstruction(135,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(135,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(135,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(60,GameVersion.Crash2)]
    [GOOLInstruction(60,GameVersion.Crash3)]
    public sealed class Evnt : GOOLInstruction
    {
        public Evnt(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "EVNT";
        public override string Format => "[EEEEEEEEEEEE] (RRRRRR) AAA (LLL)";
        public override string Comment => $"{(Args['R'].Value > 0 ? $"if {GetArg('R')}, " : "")}send event {GetArg('E')} to {GetArg('L')}" + (Args['A'].Value > 0 ? $" with {GetArg('A')} argument(s)" : "");
    }
}
