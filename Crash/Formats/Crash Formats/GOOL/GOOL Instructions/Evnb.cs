namespace Crash.GOOLIns
{
    [GOOLInstruction(143,GameVersion.Crash1)]
    [GOOLInstruction(143,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(143,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(143,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(68,GameVersion.Crash2)]
    [GOOLInstruction(68,GameVersion.Crash3)]
    public sealed class Evnb : GOOLInstruction
    {
        public Evnb(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "EVNB";
        public override string Format => "[EEEEEEEEEEEE] (RRRRRR) AAA LLL";
        public override string Comment => $"{(Args['R'].Value > 0 ? $"if {GetArg('R')}, " : "")}send event {GetArg('E')} type {GetArg('L')} to every object" + (Args['A'].Value > 0 ? $" with {GetArg('A')} argument(s)" : "");
    }
}
