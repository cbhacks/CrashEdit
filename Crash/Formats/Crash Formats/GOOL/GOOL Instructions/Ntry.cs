namespace Crash.GOOLIns
{
    [GOOLInstruction(139,GameVersion.Crash1)]
    [GOOLInstruction(139,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(139,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(139,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(64,GameVersion.Crash2)]
    [GOOLInstruction(64,GameVersion.Crash3)]
    public sealed class Ntry : GOOLInstruction
    {
        public Ntry(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NTRY";
        public override string Format => "[EEEEEEEEEEEE] [TTTTTTTTTTTT]";
        public override string Comment => string.Empty;
    }
}
