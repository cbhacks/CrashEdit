namespace Crash.GOOLIns
{
    [GOOLInstruction(136,GameVersion.Crash1)]
    [GOOLInstruction(136,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(136,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(136,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(61,GameVersion.Crash2)]
    [GOOLInstruction(61,GameVersion.Crash3)]
    public sealed class Rstt : GOOLInstruction
    {
        public Rstt(int value, GOOLEntry gool) : base(value,gool) {}

        public override string Name => "RSTT";
        public override string Format => "IIIIIIIIII VVVV (RRRRRR) CC TT";
        public override string Comment => string.Empty;
    }
}
