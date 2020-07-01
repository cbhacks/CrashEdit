namespace Crash.GOOLIns
{
    [GOOLInstruction(29,GameVersion.Crash1)]
    [GOOLInstruction(29,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(29,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(29,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(29,GameVersion.Crash2)]
    [GOOLInstruction(29,GameVersion.Crash3)]
    public sealed class Psin : GOOLInstruction
    {
        public Psin(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "PSIN";
        public override string Format => DefaultFormat;
        public override string Comment => $"sin({GetArg('A')},{GetArg('B')})";
    }
}
