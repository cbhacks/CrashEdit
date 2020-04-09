namespace Crash.GOOLIns
{
    [GOOLInstruction(19,GameVersion.Crash1)]
    [GOOLInstruction(19,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(19,GameVersion.Crash2)]
    [GOOLInstruction(19,GameVersion.Crash3)]
    public sealed class Nsek : GOOLInstruction
    {
        public Nsek(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "NSEK";
        public override string Format => DefaultFormatLR;
        public override string Comment => string.Empty;
    }
}
