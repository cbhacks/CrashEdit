namespace Crash.GOOLIns
{
    [GOOLInstruction(138,GameVersion.Crash1)]
    [GOOLInstruction(138,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(138,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(138,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(63,GameVersion.Crash2)]
    [GOOLInstruction(63,GameVersion.Crash3)]
    public sealed class Chld : GOOLInstruction
    {
        public Chld(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "CHLD";
        public override string Format => "CCCCCC SSSSSS TTTTTTTT AAAA";
        public override string Comment => string.Empty;
    }
}
