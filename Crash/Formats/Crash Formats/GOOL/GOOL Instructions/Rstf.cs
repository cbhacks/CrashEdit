namespace Crash.GOOLIns
{
    [GOOLInstruction(137,GameVersion.Crash1)]
    [GOOLInstruction(137,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(137,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(137,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(62,GameVersion.Crash2)]
    [GOOLInstruction(62,GameVersion.Crash3)]
    public sealed class Rstf : GOOLInstruction
    {
        public Rstf(int value, GOOLEntry gool) : base(value,gool) {}

        public override string Name => "RSTF";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string Comment => string.Empty;
    }
}
