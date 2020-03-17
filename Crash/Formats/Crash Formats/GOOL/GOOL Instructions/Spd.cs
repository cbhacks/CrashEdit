namespace Crash.GOOLIns
{
    [GOOLInstruction(27,GameVersion.Crash1)]
    [GOOLInstruction(27,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(27,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(27,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(27,GameVersion.Crash2)]
    [GOOLInstruction(27,GameVersion.Crash3)]
    public sealed class Spd : GOOLInstruction
    {
        public Spd(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SPD";
        public override string Format => DefaultFormat;
        public override string Comment => $"spd({GetArg('A')}, {GetArg('B')})";
    }
}
