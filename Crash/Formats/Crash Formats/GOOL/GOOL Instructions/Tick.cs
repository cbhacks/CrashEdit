namespace Crash.GOOLIns
{
    [GOOLInstruction(30,GameVersion.Crash1)]
    [GOOLInstruction(30,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(30,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(30,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(30,GameVersion.Crash2)]
    [GOOLInstruction(30,GameVersion.Crash3)]
    public sealed class Tick : GOOLInstruction
    {
        public Tick(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "TICK";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"({GetArg('R')} + tick) % {GetArg('L')}";
    }
}
