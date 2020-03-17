namespace Crash.GOOLIns
{
    [GOOLInstruction(34,GameVersion.Crash1)]
    [GOOLInstruction(34,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(34,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(34,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(34,GameVersion.Crash2)]
    [GOOLInstruction(34,GameVersion.Crash3)]
    public sealed class Seek : GOOLInstruction
    {
        public Seek(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "SEEK";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"seek({GetArg('L')}, " + (Args['R'].Value == DoubleStackRef ? "[sp-1], [sp])" : $"{GetArg('R')}, 0x100)");
    }
}
