namespace Crash.GOOLIns
{
    [GOOLInstruction(142,GameVersion.Crash1)]
    [GOOLInstruction(142,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(142,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(142,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(67,GameVersion.Crash2)]
    [GOOLInstruction(67,GameVersion.Crash3)]
    public sealed class Vecb : GOOLInstruction
    {
        public Vecb(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "VECB";
        public override string Format => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";
        public override string Comment => string.Empty;
    }
}
