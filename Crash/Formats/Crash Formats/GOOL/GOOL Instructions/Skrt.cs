namespace Crash.GOOLIns
{
    [GOOLInstruction(37,GameVersion.Crash1)]
    [GOOLInstruction(37,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(37,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(37,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(37,GameVersion.Crash2)]
    [GOOLInstruction(37,GameVersion.Crash3)]
    public sealed class Skrt : GOOLInstruction
    {
        public Skrt(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SKRT";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"seekrot({GetArg('L')}, " + (Args['R'].Value == DoubleStackRef ? "[top], [top-1], 0)" : $"{GetArg('R')}, 0x100, 0)");
    }
}
