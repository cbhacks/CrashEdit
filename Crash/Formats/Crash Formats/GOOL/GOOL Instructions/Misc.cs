namespace Crash.GOOLIns
{
    [GOOLInstruction(28,GameVersion.Crash1)]
    [GOOLInstruction(28,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(28,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(28,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(28,GameVersion.Crash2)]
    [GOOLInstruction(28,GameVersion.Crash3)]
    public sealed class Misc : GOOLInstruction
    {
        public Misc(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "MISC";
        public override string Format => "[XXXXXXXXXXXX] (LLL) SSSSS PPPP";
        public override string Comment => string.Empty;
    }
}
