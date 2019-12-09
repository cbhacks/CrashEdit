namespace Crash.GOOLIns
{
    [GOOLInstruction(19,GameVersion.Crash1)]
    [GOOLInstruction(19,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(19,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(19,GameVersion.Crash2)]
    [GOOLInstruction(19,GameVersion.Crash3)]
    public sealed class Path : GOOLInstruction
    {
        public Path(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "PATH";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"seekrot({GetArg('L')}, " + (Args['R'].Value == DoubleStackRef ? "[top], [top-1])" : $"{GetArg('R')}, 0x100)");
    }
}
