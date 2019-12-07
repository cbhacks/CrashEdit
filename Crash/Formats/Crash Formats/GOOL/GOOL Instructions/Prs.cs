namespace Crash.GOOLIns
{
    [GOOLInstruction(29,GameVersion.Crash1)]
    [GOOLInstruction(29,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(29,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(29,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(29,GameVersion.Crash2)]
    [GOOLInstruction(29,GameVersion.Crash3)]
    public sealed class Prs : GOOLInstruction
    {
        public Prs(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "PRS";
        public override string Format => DefaultFormat;
        public override string Comment => string.Empty;
    }
}
