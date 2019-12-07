namespace Crash.GOOLIns
{
    [GOOLInstruction(145,GameVersion.Crash1)]
    [GOOLInstruction(145,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(145,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(145,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(70,GameVersion.Crash2)]
    [GOOLInstruction(70,GameVersion.Crash3)]
    public sealed class Chlf : GOOLInstruction
    {
        public Chlf(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CHLF";
        public override string Format => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public override string Comment => string.Empty;
    }
}
