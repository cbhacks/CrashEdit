namespace Crash.GOOLIns
{
    [GOOLInstruction(133,GameVersion.Crash1)]
    [GOOLInstruction(133,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(133,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(133,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(58,GameVersion.Crash2)]
    [GOOLInstruction(58,GameVersion.Crash3)]
    public sealed class Veca : GOOLInstruction
    {
        public Veca(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "VECA";
        public override string Format => "[VVVVVVVVVVVV] AAA BBB TTT (LLL)";
        public override string Comment => string.Empty;
    }
}
