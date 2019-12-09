namespace Crash.GOOLIns
{
    [GOOLInstruction(26,GameVersion.Crash1)]
    [GOOLInstruction(26,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(26,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(26,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(26,GameVersion.Crash2)]
    [GOOLInstruction(26,GameVersion.Crash3)]
    public sealed class Pad : GOOLInstruction
    {
        public Pad(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "PAD";
        public override string Format => "BBBBBBBBBBBB PP SS DDDD T 000";
        public override string Comment => $"";
    }
}
