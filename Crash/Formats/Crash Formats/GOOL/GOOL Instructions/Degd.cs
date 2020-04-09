namespace Crash.GOOLIns
{
    [GOOLInstruction(33,GameVersion.Crash1)]
    [GOOLInstruction(33,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(33,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(33,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(33,GameVersion.Crash2)]
    [GOOLInstruction(33,GameVersion.Crash3)]
    public sealed class Degd : GOOLInstruction
    {
        public Degd(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "DEGD";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"degdist({GetArg('L')},{GetArg('R')})";
    }
}
