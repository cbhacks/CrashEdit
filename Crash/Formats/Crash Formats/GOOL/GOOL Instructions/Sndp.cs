namespace Crash.GOOLIns
{
    [GOOLInstruction(140,GameVersion.Crash1)]
    [GOOLInstruction(140,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(140,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(140,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(65,GameVersion.Crash2)]
    [GOOLInstruction(65,GameVersion.Crash3)]
    public sealed class Sndp : GOOLInstruction
    {
        public Sndp(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SNDP";
        public override string Format => DefaultFormat;
        public override string Comment => $"play sound {GetArg('A')} at {GetArg('B')} volume";
    }
}
