namespace Crash.GOOLIns
{
    [GOOLInstruction(141,GameVersion.Crash1)]
    [GOOLInstruction(141,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(141,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(141,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(66,GameVersion.Crash2)]
    [GOOLInstruction(66,GameVersion.Crash3)]
    public sealed class Sndp : GOOLInstruction
    {
        public Sndp(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SNDP";
        public override string Format => "[SSSSSSSSSSSS] (RRRRRR) FF VVVV";
        public override string Comment => string.Empty;
    }
}
