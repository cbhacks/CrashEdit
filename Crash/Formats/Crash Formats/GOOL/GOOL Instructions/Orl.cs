namespace Crash.GOOLIns
{
    [GOOLInstruction(6,GameVersion.Crash1)]
    [GOOLInstruction(6,GameVersion.Crash1Beta1995)]
    [GOOLInstruction(6,GameVersion.Crash1BetaMAR08)]
    [GOOLInstruction(6,GameVersion.Crash1BetaMAY11)]
    [GOOLInstruction(6,GameVersion.Crash2)]
    [GOOLInstruction(6,GameVersion.Crash3)]
    public sealed class Orl : GOOLInstruction
    {
        public Orl(int value,GOOLEntry gool) : base(value,gool) { }

        public override string Name => "ORL";
        public override string Format => DefaultFormatLR;
        public override string Comment => $"{GetArg('L')} || {GetArg('R')}";
    }
}
