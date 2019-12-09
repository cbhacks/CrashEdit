namespace Crash.GOOLIns
{
    [GOOLInstruction(31,GameVersion.Crash1)]
    [GOOLInstruction(31,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(31,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(31,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(31,GameVersion.Crash2)]
    [GOOLInstruction(31,GameVersion.Crash3)]
    public sealed class Rgl : GOOLInstruction
    {
        public Rgl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "RGL";
        public override string Format => "[IIIIIIIIIIII] 000001111101";
        public override string Comment => $"global[{GetArg('I')}]";
    }
}
